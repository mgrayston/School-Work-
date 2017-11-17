using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Controller;
using Model;
using System.Drawing;
using System.Collections.Generic;
using SpaceWars;

namespace View {
    public partial class spaceWarsForm : Form {

        // World is a simple container for Players and Powerups
        private World theWorld;

        DrawingPanel drawingPanel;

        //private FakeServer server;

        const int worldSize = 500;

        public spaceWarsForm() {
            InitializeComponent();
            theWorld = new World(worldSize);

            // Set up the windows Form.
            // This stuff is usually handled by the drag and drop designer,
            // but it's simple enough for this lab.
            ClientSize = new Size(worldSize, worldSize);
            drawingPanel = new DrawingPanel(theWorld);
            drawingPanel.Location = new Point(0, 50);
            drawingPanel.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
            drawingPanel.BackColor = Color.Black;
            this.Controls.Add(drawingPanel);

            this.AcceptButton = connectButton;
        }

        private void connectButton_Click(object sender, EventArgs e) {
            // TODO: This needs better error handling. Left as an exercise.
            // If the server box is empty, it gives a message, but doesn't allow us to try to reconnect.
            // It also doesn't handle unreachable addresses.
            if (serverText.Text == "") {
                MessageBox.Show("Please enter a server address");
                return;
            }

            // Disable the controls and try to connect
            connectButton.Enabled = false;
            serverText.Enabled = false;
            nameText.Enabled = false;

            // REMOVE
            System.Diagnostics.Debug.WriteLine("Connect button clicked");

            Socket server = Network.ConnectToServer(HandleFirstContact, serverText.Text);
        }

        private void HandleFirstContact(SocketState state) {
            state.CallMe = ReceiveStartup;
            Network.Send(state.Socket, nameText.Text + "\n");
            Network.GetData(state);
        }

        private void ReceiveStartup(SocketState state) {
            String[] response = Regex.Split(state.Builder.ToString(), @"(?<=[\n])");
            theWorld = new World(int.Parse(response[0]), int.Parse(response[1]));

            // REMOVE
            System.Diagnostics.Debug.WriteLine("Received connection info!\nID: " + theWorld.Id + "\nWorld Size: " + theWorld.WorldSize);

            state.Builder.Clear();
            state.CallMe = ReceiveWorld;
            Network.GetData(state);
        }


        private void ReceiveWorld(SocketState state) {
            Processor.ProcessData(theWorld, state);
            Network.GetData(state);
        }

        
    }
}
