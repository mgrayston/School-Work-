using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class ConnectionState
    {
        private NetworkAction callMe;
        private List<SocketState> clients;
        private TcpListener listener;


        public ConnectionState(NetworkAction callMe)
        {
            callMe = this.callMe;
            clients = new List<SocketState>();
            listener = null;
        }
        public List<SocketState> Buffer { get => clients; }
        public NetworkAction CallMe { get => callMe; set => callMe = value; }
        public TcpListener Listener { get => listener; set => listener = value; }
    }
    public class NetworkController
    {
            public const int DEFAULT_PORT = 11000;
            private TcpListener listener;
            private ConnectionState state;

        public void ServerAwaitingClientLoop(NetworkAction callMe)
            {
                Console.WriteLine("Server waiting for client");
                listener = new TcpListener(IPAddress.Any, NetworkController.DEFAULT_PORT);
                state = new ConnectionState(callMe);
                listener.Start();
                state.Listener = listener;
                state.CallMe = callMe;
                listener.BeginAcceptSocket(AcceptNewClient, state);
            }

        private void AcceptNewClient(IAsyncResult ar)
        {
            state = (ConnectionState)ar.AsyncState;
            Socket socket = state.Listener.EndAcceptSocket(ar);
            SocketState ss = new SocketState(socket, -1, null);
            ss.CallMe = state.CallMe;
            ss.CallMe(ss);
            state.Listener.BeginAcceptSocket(AcceptNewClient, state); //TODO - should this take in AcceptNewClient or HandleNewClient?
        }
    }
}
