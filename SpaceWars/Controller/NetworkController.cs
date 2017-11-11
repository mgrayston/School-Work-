using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkController {

    public delegate void NetworkAction(SocketState state);

    public class SocketState {
        private Socket socket;
        private int ID;
        private byte[] buffer;
        private StringBuilder builder;
        private NetworkAction callMe;

        public SocketState(Socket s, int id, NetworkAction d) {
            socket = s;
            ID = id;
            callMe = d;
            buffer = new byte[1024];
            builder = new StringBuilder();
        }

        public Socket Socket { get => socket; }
        public StringBuilder Builder { get => builder; }
        public byte[] Buffer { get => buffer; set => buffer = value; }
        public NetworkAction CallMe { get => callMe; }
    }

    public class Network { //changed from static class
        Socket ConnectToServer(NetworkAction callMe, string hostName) {
            Socket socket;
            IPAddress ipAddress;
            MakeSocket(hostName, out socket, out ipAddress);

            SocketState state = new SocketState(socket, -1, callMe);

            socket.BeginConnect(ipAddress, DEFAULT_PORT, ConnectedCallback, state);
           
            return socket;
        }

        void ConnectedCallback(IAsyncResult stateObject) {
            SocketState state = (SocketState)stateObject;
            
            try
            {
                // Complete the connection.
                state.Socket.EndConnect(stateObject);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return;
            }

            // Don't start an event loop to receive data from the server. The client might not want to do that.
            //state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, ReceiveCallback, state);

            // Instead, just invoke the client's delegate so it can take whatever action it desires.
            state.CallMe(state);

        }

        /// <summary>
        /// GetData is  a wrapper for BeginReceive.
        /// This is the public entry point for asking for data.
        /// Necessary so that we can separate networking concerns from client concerns.
        /// </summary>
        /// <param name="state"></param>
        void GetData(SocketState state) {
            state.Socket.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, ReceiveCallback, state);
        }

        private void ReceiveCallback(IAsyncResult stateObject) {
            SocketState state = (SocketState)stateObject.AsyncState;

            int bytesRead = state.Socket.EndReceive(stateObject);

            // If the socket is still open
            if (bytesRead > 0) {
                string theMessage = Encoding.UTF8.GetString(state.Buffer, 0, bytesRead);
                // Append the received data to the growable buffer.
                // It may be an incomplete message, so we need to start building it up piece by piece
                state.Builder.Append(theMessage);

                //ProcessMessage(state); // this should be moved to the client.  Not networking code

                // TODO - Instead, just invoke the client's delegate, so it can take whatever action it desires.
                state.CallMe(state);
    
            }

            // Continue the "event loop" that was started on line 96.
            // Start listening for more parts of a message, or more new messages
            state.Socket.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, ReceiveCallback, state);

        }

        public const int DEFAULT_PORT = 11000;

        private static void MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress) {
            ipAddress = IPAddress.None;
            socket = null;

            try {
                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo;

                try {
                    ipHostInfo = Dns.GetHostEntry(hostName);
                    bool foundIPV4 = false;
                    foreach (IPAddress addr in ipHostInfo.AddressList)
                        if (addr.AddressFamily != AddressFamily.InterNetworkV6) {
                            foundIPV4 = true;
                            ipAddress = addr;
                            break;
                        }
                    // Didn't find any IPV4 addresses
                    if (!foundIPV4) {
                        System.Diagnostics.Debug.WriteLine("Invalid addres: " + hostName);
                        throw new ArgumentException("Invalid address");
                    }
                }
                catch (Exception) {
                    // see if host name is actually an ipaddress, i.e., 155.99.123.456
                    System.Diagnostics.Debug.WriteLine("using IP");
                    ipAddress = IPAddress.Parse(hostName);
                }

                // Create a TCP/IP socket.
                socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                // Disable Nagle's algorithm - can speed things up for tiny messages, 
                // such as for a game
                socket.NoDelay = true;
            }
            catch (Exception e) {
                System.Diagnostics.Debug.WriteLine("Unable to create socket. Error occured: " + e);
                throw new ArgumentException("Invalid address");
            }
        }
    }
}
