using System.Collections.Generic;
using Controller;
using Model;

namespace Server {
    class Server {
        private static List<SocketState> clients;
        private static World world;

        static void Main(string[] args) {
            // TODO read world settings and create new world
            world = new World(750);
            clients = new List<SocketState>();
            Network.ServerAwaitingClientLoop(HandleNewClient);
        }

        private static void HandleNewClient(SocketState ss) {
            ss.CallMe = ReceiveName;
            lock(clients) {
                clients[ss.ID] = ss;
            }
            Network.GetData(ss);
        }

        private static void ReceiveName(SocketState ss) {
            Network.Send(ss.Socket, ss.ID + "\n" + world.WorldSize + "\n");
            // TODO create new ship here with name received and start to send data
        }
    }
}
