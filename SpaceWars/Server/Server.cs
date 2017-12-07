using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Controller;
using Model;
using SpaceWars;
using System.Xml.Linq;
using System.Text;
using Newtonsoft.Json;
using Server.Properties;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Server {
    class Server {
        // This will function as the default milliseconds between frames
        private static List<SocketState> clients;
        private static World world;
        private static Stopwatch updateStopwatch;
        private static StringBuilder worldStringBuilder;
        private static Random random;

        // Update values
        private static int universeSize;
        private static int msPerFrame;
        private static double engineStrength;
        private static int projectileSpeed;
        private static int shotDelay;
        private static int respawnRate;
        private static int starSize;
        private static int shipSize;
        private static double turningRate;

        static void Main(string[] args) {
            // Read world settings and create new world and add stars etc.
            try {
                XDocument settings = XDocument.Parse(Resources.settings);
                XElement root = settings.Root;

                universeSize = Convert.ToInt32(root.Element("UniverseSize").Value); // 750
                msPerFrame = Convert.ToInt32(root.Element("MSPerFrame").Value); // 16
                engineStrength = Convert.ToDouble(root.Element("EngineStrength").Value); // 0.08
                projectileSpeed = Convert.ToInt32(root.Element("ProjectileSpeed").Value); // 15
                shotDelay = Convert.ToInt32(root.Element("ShotDelay").Value); // 6
                respawnRate = Convert.ToInt32(root.Element("RespawnRate").Value);
                starSize = Convert.ToInt32(root.Element("StarSize").Value); // 35
                shipSize = Convert.ToInt32(root.Element("ShipSize").Value); // 20
                turningRate = Convert.ToDouble(root.Element("TurningRate").Value); // 2

                world = new World(universeSize);

                int starId = 0;
                foreach (XElement star in root.Elements("Star")) {
                    world.AddStar(new Star(starId, Convert.ToDouble(star.Element("mass").Value), Convert.ToInt32(star.Element("x").Value), Convert.ToInt32(star.Element("y").Value)));
                    ++starId;
                }
            }
            catch (Exception e) {
                Console.WriteLine("Settings could not be read: " + e.Message);
            }

            clients = new List<SocketState>();
            worldStringBuilder = new StringBuilder();
            updateStopwatch = new Stopwatch();
            random = new Random();

            // Start a thread infinitely updating the world
            Thread worldUpdater = new Thread(start: UpdateWorld);
            updateStopwatch.Start();
            worldUpdater.Start();

            // Begin receiving clients
            Network.ServerAwaitingClientLoop(HandleNewClient);

            // Keep Main running until "close" is enterd into the terminal
            while (!Console.ReadLine().ToLower().Equals("close")) { }
            // TODO dispose of anything here
        }

        private static void HandleNewClient(SocketState ss) {
            ss.CallMe = ReceiveName;
            Network.GetData(ss);
        }

        private static void ReceiveName(SocketState ss) {
            // Extract name from received data
            string name = ss.Builder.ToString();
            name = name.Remove(name.Length - 1);
            Console.WriteLine("Received name: " + name);

            Network.Send(ss.Socket, ss.ID + "\n" + world.WorldSize + "\n");
            clients.Add(ss);
            Ship newShip = new Ship(ss.ID, name, random.Next(universeSize + 1) - (universeSize / 2), random.Next(universeSize + 1) - (universeSize / 2));
            // Don't start ship in star's location
            while (Math.Abs(newShip.Loc.GetX()) < starSize && Math.Abs(newShip.Loc.GetX()) < starSize) {
                newShip.Loc = new Vector2D(random.Next(universeSize + 1) - (universeSize / 2), random.Next(universeSize + 1) - (universeSize / 2));
            }
            world.AddShip(newShip);

            ss.CallMe = ReceiveMoveRequest;
            Network.GetData(ss);
        }

        private static void ReceiveMoveRequest(SocketState ss) {
            String request = ss.Builder.ToString();
            string[] parts = Regex.Split(request, @"(?<=[\n])");

            bool thrust = false;
            bool fire = false;
            int r = 0;
            int l = 0;

            foreach (string part in parts) {
                if (part.Length == 0) {
                    continue;
                }
                if (part[part.Length - 1] != '\n') {
                    break;
                }

                if (part.Contains("T")) {
                    thrust = true;
                }
                if (part.Contains("F")) {
                    fire = true;
                }
                if (part.Contains("R")) {
                    r++;
                }
                if (part.Contains("L")) {
                    l++;
                }

                ss.Builder.Remove(0, part.Length);
            }

            Ship ship = world.GetShip(ss.ID);
            if (ship.HP > 0) {
                if (thrust) {
                    ship.Thrust = true;
                }
                else {
                    ship.Thrust = false;
                }
                if (fire) {
                    if (ship.Fire(shotDelay * msPerFrame)) {
                        world.AddProjectile(ship.id, ship.Loc.GetX(), ship.Loc.GetY(), ship.Dir.GetX(), ship.Dir.GetY());
                    }
                }
                if (r > l) {
                    ship.ToTurn = 1;
                }
                else if (l > r) {
                    ship.ToTurn = -1;
                }
                else {
                    ship.ToTurn = 0;
                }
            }

            Network.GetData(ss);
        }

        private static void UpdateWorld() {
            while (true) {
                // Spin until delay is reached
                while (updateStopwatch.ElapsedMilliseconds < msPerFrame) { }

                updateStopwatch.Restart();

                lock (world) {
                    // Move ships
                    foreach (Ship ship in world.GetShips()) {
                        // Only update connected players
                        if (IsConnected(ship.id)) {
                            if (ship.ToTurn == 1) {
                                ship.Dir.Rotate(turningRate);
                            }
                            if (ship.ToTurn == -1) {
                                ship.Dir.Rotate(-turningRate);
                            }

                            Vector2D acceleration = new Vector2D(0, 0);
                            Vector2D thrust = new Vector2D(ship.Dir);
                            if (ship.Thrust) {
                                acceleration = acceleration + (thrust * engineStrength);
                            }

                            foreach (Star star in world.GetStars()) {
                                Vector2D g = star.Loc - ship.Loc;
                                g.Normalize();
                                acceleration = acceleration + (g * star.Mass);
                            }

                            ship.Velocity = ship.Velocity + acceleration;
                            ship.Loc = ship.Loc + ship.Velocity;

                            // Wrap location
                            ShipWrapAround(ship);
                        }
                        else {
                            if (ship.Connected) {
                                Console.WriteLine("Client disconnected: " + ship.Name);
                                ship.Connected = false;
                            }
                        }
                    }

                    // Move projectiles
                    foreach (Projectile projectile in world.GetProjectiles()) {
                        if (projectile.Alive) {
                            projectile.Loc = projectile.Loc + (projectile.Dir * projectileSpeed);
                        }
                        else {
                            world.RemoveProjectile(projectile.id);
                        }
                    }

                    // Check for collisions
                    CollisionChecks();

                    // Generate world string
                    worldStringBuilder.Clear();
                    foreach (Ship ship in world.GetShips()) {
                        if (IsConnected(ship.id)) {
                            worldStringBuilder.Append(JsonConvert.SerializeObject(ship) + "\n");
                        }
                    }
                    foreach (Projectile projectile in world.GetProjectiles()) {
                        worldStringBuilder.Append(JsonConvert.SerializeObject(projectile) + "\n");
                    }
                    foreach (Star star in world.GetStars()) {
                        worldStringBuilder.Append(JsonConvert.SerializeObject(star) + "\n");
                    }
                }

                String worldString = worldStringBuilder.ToString();

                // Send updated world to all clients
                foreach (SocketState client in clients) {
                    if (client.Socket.Connected) {
                        Network.Send(client.Socket, worldString);
                    }
                }
            }
        }

        private static void CollisionChecks() {
            // Only check alive ships
            foreach (Ship ship in world.GetShips()) {
                if (IsConnected(ship.id)) {
                    if (ship.HP > 0) {
                        // Check for Collisions of Stars and ships
                        foreach (Star star in world.GetStars()) {
                            if ((star.Loc - ship.Loc).Length() < starSize) {
                                ship.HP = 0;
                                new Thread(() => Respawn(ship)).Start();
                            }
                        }

                        // Check for Collisions of projectiles and ships
                        foreach (Projectile projectile in world.GetProjectiles()) {
                            if (projectile.Owner != ship.id) {
                                if ((ship.Loc - projectile.Loc).Length() < shipSize) {
                                    ship.HP--;
                                    projectile.Alive = false;
                                    if (ship.HP == 0) {
                                        world.AddPoint(projectile.Owner);
                                        new Thread(() => Respawn(ship)).Start();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Check for Collisions of projectiles and stars
            foreach (Star star in world.GetStars()) {
                foreach (Projectile projectile in world.GetProjectiles()) {
                    if ((star.Loc - projectile.Loc).Length() < starSize) {
                        projectile.Alive = false;
                    }
                }
            }
        }

        private static void Respawn(Ship ship) {
            Thread.Sleep(respawnRate * msPerFrame);
            ship.Loc = new Vector2D(random.Next(universeSize + 1) - (universeSize / 2), random.Next(universeSize + 1) - (universeSize / 2));
            while (Math.Abs(ship.Loc.GetX()) < starSize && Math.Abs(ship.Loc.GetX()) < starSize) {
                ship.Loc = new Vector2D(random.Next(universeSize + 1) - (universeSize / 2), random.Next(universeSize + 1) - (universeSize / 2));
            }
            ship.Respawn();
        }

        private static bool IsConnected(int id) {
            if (clients[id] != null) {
                return clients[id].Socket.Connected;
            }
            return false;
        }

        /// <summary>
        /// Updates the coordinates of ship to wraparound the screen.
        /// </summary>
        /// <param name="ship"></param>
        private static void ShipWrapAround(Ship ship) {
            // NOTE: ship size is added/subracted so that it fully disapears before wrapping to the other side.
            // X-coord wrap-around
            if (ship.Loc.GetX() - shipSize >= (universeSize / 2)) { // Wrap around from right to left
                ship.Loc = new Vector2D(-(universeSize / 2) - shipSize + 1, ship.Loc.GetY());
            }
            else if (ship.Loc.GetX() + shipSize <= -(universeSize / 2)) { // Wrap around from left to right
                ship.Loc = new Vector2D((universeSize / 2) + shipSize - 1, ship.Loc.GetY());
            }

            // Y-coord wrap-around
            if (ship.Loc.GetY() - shipSize >= (universeSize / 2)) { // Wrap around from bottom to top
                ship.Loc = new Vector2D(ship.Loc.GetX(), -(universeSize / 2) - shipSize + 1);
            }
            else if (ship.Loc.GetY() + shipSize <= -(universeSize / 2)) { // Wrap around from top to bottom
                ship.Loc = new Vector2D(ship.Loc.GetX(), (universeSize / 2) + shipSize - 1);
            }
        }
    }
}
