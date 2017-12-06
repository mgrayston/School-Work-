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

namespace Server {
    // TODO add respawn
    class Server {
        // This will function as the default milliseconds between frames
        private static List<SocketState> clients;
        private static World world;
        private static Stopwatch updateStopwatch;
        private static StringBuilder worldString;
        private static Random random;

        // Update values
        private static int universeSize;
        private static int updateDelay;
        private static double engineStrength;
        private static int projectileSpeed;
        private static int shotDelay;
        private static int respawnRate;
        private static int starSize;
        private static int shipSize;

        static void Main(string[] args) {
            // Read world settings and create new world and add stars etc.
            try {
                XDocument settings = XDocument.Parse(Resources.settings);
                XElement root = settings.Root;

                universeSize = Convert.ToInt32(root.Element("UniverseSize").Value); // 750
                updateDelay = Convert.ToInt32(root.Element("MSPerFrame").Value); // 16
                engineStrength = Convert.ToDouble(root.Element("EngineStrength").Value); // 0.08
                projectileSpeed = Convert.ToInt32(root.Element("ProjectileSpeed").Value); // 15
                shotDelay = Convert.ToInt32(root.Element("ShotDelay").Value); // 6
                respawnRate = Convert.ToInt32(root.Element("RespawnRate").Value);
                starSize = Convert.ToInt32(root.Element("StarSize").Value); // 35
                shipSize = Convert.ToInt32(root.Element("ShipSize").Value); // 20

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
            worldString = new StringBuilder();
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
            world.AddShip(new Ship(ss.ID, name, random.Next(((universeSize*2) + 1)) - universeSize, random.Next(((universeSize * 2) + 1)) - universeSize));
        }

        private static void UpdateWorld() {
            while (true) {
                // Spin until delay is reached
                while (updateStopwatch.ElapsedMilliseconds < updateDelay) { }

                updateStopwatch.Restart();

                // Move ships
                foreach (Ship ship in world.GetShips()) {
                    // TODO wrap location
                    // Remove client if it has disconnected
                    if (!IsConnected(ship)) {
                        clients.RemoveAt(ship.id);
                        Console.WriteLine("Client disconnected: " + ship.Name);
                    }
                    else {
                        Vector2D acceleration = new Vector2D(0, 0);
                        Vector2D thrust = new Vector2D(ship.Dir);
                        acceleration = acceleration + (thrust * engineStrength);

                        foreach (Star star in world.GetStars()) {
                            Vector2D g = star.Loc - ship.Loc;
                            g.Normalize();
                            acceleration = acceleration + (g * star.Mass);
                        }

                        ship.Velocity = ship.Velocity + acceleration;
                        ship.Loc = ship.Loc + ship.Velocity;
                    }
                }

                // Move projectiles
                foreach (Projectile projectile in world.GetProjectiles()) {
                    projectile.Loc = projectile.Loc + (projectile.Dir * projectileSpeed);
                }

                // Check for collisions
                CollisionChecks();

                // Generate world string
                worldString.Clear();
                foreach (Ship ship in world.GetShips()) {
                    if (IsConnected(ship)) {
                        worldString.Append(JsonConvert.SerializeObject(ship) + "\n");
                    }
                }
                foreach (Projectile projectile in world.GetProjectiles()) {
                    worldString.Append(JsonConvert.SerializeObject(projectile) + "\n");
                }
                foreach (Star star in world.GetStars()) {
                    worldString.Append(JsonConvert.SerializeObject(star) + "\n");
                }

                // Send updated world to all clients
                foreach (SocketState client in clients) {
                    Network.Send(client.Socket, worldString.ToString());
                }
            }
        }

        private static void CollisionChecks() {
            // Only check alive ships
            foreach (Ship ship in world.GetShips()) {
                if (IsConnected(ship)) {
                    if (ship.HP > 0) {
                        double shipX = ship.Loc.GetX();
                        double shipY = ship.Loc.GetY();

                        // Check for Collisions of Stars and ships
                        foreach (Star star in world.GetStars()) {
                            double starX = star.Loc.GetX();
                            double starY = star.Loc.GetY();

                            if (Math.Abs(starX - ship.Loc.GetX()) <= starSize || Math.Abs(starY - ship.Loc.GetY()) <= starSize) {
                                ship.HP = 0;
                            }
                        }

                        // Check for Collisions of projectiles and ships
                        foreach (Projectile projectile in world.GetProjectiles()) {
                            if (Math.Abs(shipX - projectile.Loc.GetX()) <= shipSize || Math.Abs(shipY - projectile.Loc.GetY()) <= shipSize) {
                                ship.HP = 0;
                                projectile.Alive = false;
                                world.AddPoint(projectile.Owner);
                            }
                        }
                    }
                }
            }

            // Check for Collisions of projectiles and stars
            foreach (Star star in world.GetStars()) {
                double starX = star.Loc.GetX();
                double starY = star.Loc.GetY();

                foreach (Projectile projectile in world.GetProjectiles()) {
                    if (Math.Abs(starX - projectile.Loc.GetX()) <= starSize || Math.Abs(starY - projectile.Loc.GetY()) <= starSize) {
                        projectile.Alive = false;
                    }
                }
            }
        }

        private static bool IsConnected(Ship ship) {
            return clients[ship.id].Socket.Connected;
        }
    }
}
