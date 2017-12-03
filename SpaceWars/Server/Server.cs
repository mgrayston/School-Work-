using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Controller;
using Model;
using SpaceWars;

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

        static void Main(string[] args) {
            // TODO read world settings and create new world and add stars etc.
            world = new World(750);
            clients = new List<SocketState>();

            // TODO these are defaults; could be changed in settings
            updateDelay = 10;
            engineStrength = 0.08;
            projectileSpeed = 15;

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
            // TODO
        }
    }
}
