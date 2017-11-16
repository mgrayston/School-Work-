using System;
using Model;
using System.Text.RegularExpressions;

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

                state.Builder.Remove(0, part.Length);
            }
        }
    }
}
