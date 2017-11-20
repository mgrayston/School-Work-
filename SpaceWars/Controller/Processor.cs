using Model;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Timers;
using System.Collections.Generic;
using System;

namespace Controller {
    /// <summary>
    /// Static class to parse data at \n and return an array of Strings.
    /// </summary>
    public static class Processor {
        public static void ProcessData(World world, SocketState state) {
            string total = state.Builder.ToString();
            string[] parts = Regex.Split(total, @"(?<=[\n])");

            foreach (string part in parts) {
                if (part.Length == 0) {
                    continue;
                }
                if (part[part.Length - 1] != '\n') {
                    break;
                }

                JObject jsonObj = JObject.Parse(part);
                var token = jsonObj.First;

                if (token.Path == "ship") {
                    Ship theShip = JsonConvert.DeserializeObject<Ship>(part);
                    world.Ships[theShip.id] = theShip;
                    if (!world.Timers.ContainsKey(theShip.id)) {
                        world.Timers[theShip.id] = new Timer(2000);
                        world.Timers[theShip.id].Elapsed += (sender, e) => DisposeShip(sender, e, world, theShip.id);
                    }
                    world.Timers[theShip.id].Stop();
                    world.Timers[theShip.id].Start();
                }
                else if (token.Path == "proj") {
                    Projectile theProj = JsonConvert.DeserializeObject<Projectile>(part);
                    world.Projectiles[theProj.id] = theProj;
                }
                else if (token.Path == "star") {
                    Star theStar = JsonConvert.DeserializeObject<Star>(part);
                    world.Stars[theStar.id] = theStar;
                }
                state.Builder.Remove(0, part.Length);
            }
        }
        private static void DisposeShip(object sender, ElapsedEventArgs e, World world, int ID) {
            Ship removed;
            world.Ships.TryRemove(ID, out removed);
            world.Timers[ID].Dispose();
        }
    }
}
