using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Controller;
using Model;
using SpaceWars;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace Server {
    class Server {
        // This will function as the default milliseconds between frames
        private static List<SocketState> clients;
        private static World world;
        private static Stopwatch updateStopwatch;

        // Update values
        private static int updateDelay;
        private static double engineStrength;
        private static int projectileSpeed;
        private static int universeSize;
        private static int starSize;
        private static int shipSize;

        static void Main(string[] args) {
            // Read world settings and create new world and add stars etc.
            try
            {
                XDocument settings = XDocument.Load("..\\..\\SETTINGS.XML");
                universeSize = Convert.ToInt32(settings.Descendants("UniverseSize").First().Value);
                updateDelay = Convert.ToInt32(settings.Descendants("MSPerFrame").First().Value); //10;
                engineStrength = Convert.ToDouble(settings.Descendants("EngineStrength").First().Value); //0.08;
                projectileSpeed = Convert.ToInt32(settings.Descendants("ProjectileSpeed").First().Value); //15;
                starSize = Convert.ToInt32(settings.Descendants("StarSize").First().Value);
                shipSize = Convert.ToInt32(settings.Descendants("ShipSize").First().Value);
            }
            catch
            {
                Console.WriteLine("settings could not be read.");
            }

            world = new World(universeSize);
            clients = new List<SocketState>();

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

            Network.Send(ss.Socket, ss.ID + "\n" + world.WorldSize + "\n");
            clients[ss.ID] = ss;
            world.AddShip(new Ship(ss.ID, name));
        }

        private static void UpdateWorld() {
            // Spin until delay is reached
            while (updateStopwatch.ElapsedMilliseconds < updateDelay) { }

            updateStopwatch.Restart();

            // Move ships
            foreach (Ship ship in world.GetShips()) {
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

            // Move projectiles
            foreach (Projectile projectile in world.GetProjectiles())
            {
                projectile.Loc = projectile.Loc + (projectile.Dir * projectileSpeed);
            }

            //Check for collisions
            CollisionChecks();


        }

        public static void CollisionChecks()
        {
            // Check for Collisions of Stars and ships
            foreach (Star star in world.GetStars())
            {
                double starX = star.Loc.GetX();
                double starY = star.Loc.GetY();
                double starRad = starSize / 2;
                foreach (Ship ship in world.GetShips())
                {
                    if (Math.Abs(starX - ship.Loc.GetX()) >= starRad || Math.Abs(starY - ship.Loc.GetY()) >= starRad)
                    {
                        //TODO - send dead representation of ship
                    }
                }
            }

            // Check for Collisions of projectiles and ships
            foreach (Ship ship in world.GetShips())
            {
                double shipX = ship.Loc.GetX();
                double shipY = ship.Loc.GetY();
                double shipRad = shipSize / 2;
                foreach (Projectile proj in world.GetProjectiles())
                {
                    if (Math.Abs(shipX - proj.Loc.GetX()) >= shipRad || Math.Abs(shipY - proj.Loc.GetY()) >= shipRad)
                    {
                        //TODO - send dead representation of proj and ship
                    }
                }
            }

            // Check for Collisions of projectiles and stars
            foreach (Star star in world.GetStars())
            {
                double starX = star.Loc.GetX();
                double starY = star.Loc.GetY();
                double starRad = starSize / 2;
                foreach (Projectile proj in world.GetProjectiles())
                {
                    if (Math.Abs(starX - proj.Loc.GetX()) >= starRad || Math.Abs(starY - proj.Loc.GetY()) >= starRad)
                    {
                        //TODO - send dead representation of proj
                    }
                }
            }
        }
    }
}
