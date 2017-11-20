using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Controller {

    /// <summary>
    /// Used for the connecting host to provide 
    /// a callback function.
    /// </summary>
    /// <param name="state"></param>
    public delegate void NetworkAction(SocketState state);

    /// <summary>
    /// Class representing the state of a Socket, such as 
    /// data it has been sent, the Socket, and a callback.
    /// </summary>
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
        public NetworkAction CallMe { get => callMe; set => callMe = value; }
    }

    /// <summary>
    /// Acts as a wrapper class for a Network. 
    /// Allows connecting, receiving, and sending
    /// data.
    /// </summary>
    public static class Network {
        public const int DEFAULT_PORT = 11000;

        /// <summary>
        /// Entrance point for connecting to a 
        /// specified server.
        /// </summary>
        /// <param name="callMe"></param>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static Socket ConnectToServer(NetworkAction callMe, string hostName) {
            Socket socket;
            IPAddress ipAddress;
            MakeSocket(hostName, out socket, out ipAddress);

            SocketState state = new SocketState(socket, -1, callMe);

            state.CallMe = callMe;

            state.Socket.BeginConnect(ipAddress, DEFAULT_PORT, ConnectedCallback, state);

            return socket;
        }

        /// <summary>
        /// Called after sending a request to connect.
        /// Attempts to complete the connection, and writes 
        /// to debug if it fails. Always calls the provided
        /// callback CallMe.
        /// </summary>
        /// <param name="stateObject"></param>
        private static void ConnectedCallback(IAsyncResult stateObject) {
            SocketState state = (SocketState)stateObject.AsyncState;

            try {
                // Complete the connection.
                state.Socket.EndConnect(stateObject);
            }
            catch (Exception e) {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
            }
            finally {
                state.CallMe(state);
            }
        }

        /// <summary>
        /// GetData is  a wrapper for BeginReceive.
        /// This is the public entry point for asking for data.
        /// Necessary so that we can separate networking concerns from client concerns.
        /// </summary>
        /// <param name="state"></param>
        public static void GetData(SocketState state) {
            state.Socket.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, ReceiveCallback, state);
        }

        /// <summary>
        /// Called after requesting data from the connection.
        /// Appends received data to the SocketState's StringBuilder
        /// and calls the CallMe function.
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar) {
            SocketState state = (SocketState)ar.AsyncState;
            try {
                int bytesRead = state.Socket.EndReceive(ar);

                // If the socket is still open
                if (bytesRead > 0) {
                    string theMessage = Encoding.UTF8.GetString(state.Buffer, 0, bytesRead);
                    // Append the received data to the growable buffer.
                    // It may be an incomplete message, so we need to start building it up piece by piece
                    state.Builder.Append(theMessage);
                    state.CallMe(state);
                }
            }
            catch (Exception e) {
                if (e is ObjectDisposedException) {
                    return;
                }
                else {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Wrapper for sending data to the connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        public static void Send(Socket socket, String data) {
            Byte[] toSend = Encoding.UTF8.GetBytes(data);
            try {
                socket.BeginSend(toSend, 0, toSend.Length, SocketFlags.None, SendCallback, socket);
            }
            catch (Exception e) {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Finishes used to finish sending data.
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar) {
            Socket socket = (Socket)ar.AsyncState;
            try {
                socket.EndSend(ar);
            }
            catch (Exception e) {
                if (e is ObjectDisposedException) {
                    return;
                }
                else {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Wrapper for making a socket. Attempts to parse and 
        /// connect to the provided hostName.
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="socket"></param>
        /// <param name="ipAddress"></param>
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
