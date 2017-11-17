using System;
using Model;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

                Console.WriteLine("Received: " + part);

                JObject jsonObj = JObject.Parse(part);
                var token = jsonObj.First;

                if (token.Path == "ship")
                {
                    Ship theShip = JsonConvert.DeserializeObject<Ship>(part);
                    //world.Ships.Add(theShip.ID, theShip;
                    world.Ships[theShip.ID] = theShip;
                }
                else if (token.Path == "proj")
                {
                    Projectile theProj = JsonConvert.DeserializeObject<Projectile>(part);
                    //world.Projectiles.Add(theProj.ID, theProj);
                    world.Projectiles[theProj.ID] = theProj;
                }
                else if (token.Path == "star")
                {
                    Star theStar = JsonConvert.DeserializeObject<Star>(part);
                    //world.Stars.Add(theStar.ID, theStar);
                    world.Stars[theStar.ID] = theStar;
                }
                state.Builder.Remove(0, part.Length);
            }
        }
    }
}
