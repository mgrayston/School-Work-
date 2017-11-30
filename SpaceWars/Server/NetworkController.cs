using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// Used for the connecting host to provide 
    /// a callback function.
    /// </summary>
    /// <param name="state"></param>
    public delegate void NetworkAction(SocketState state);

    public class SocketState
    {
        private Socket socket;
        private int ID;
        private NetworkAction callMe;

        public SocketState(Socket s, int id, NetworkAction d)
        {
            socket = s;
            ID = id;
            callMe = d;
        }

        public Socket Socket { get => socket; }
        public NetworkAction CallMe { get => callMe; set => callMe = value; }
    }
    /// <summary>
    /// Stores any socket state
    /// </summary>
    class NetworkController
    {

    }
}
