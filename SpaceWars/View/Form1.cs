using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Controller;
using Model;
using System.Drawing;
using System.Collections.Generic;
using SpaceWars;
using System.Timers;

namespace View {
    public partial class spaceWarsForm : Form {

        // World is a simple container for Players and Powerups
        private World theWorld;

        DrawingPanel drawingPanel;

        //for sending purposes only
        private SocketState theServer;

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

            // Start a new timer that will redraw the game every 15 milliseconds 
            // This should correspond to about 67 frames per second.
            System.Timers.Timer frameTimer = new System.Timers.Timer();
            frameTimer.Interval = 15;
            frameTimer.Elapsed += Redraw;
            frameTimer.Start();
        }

        /// <summary>
        /// Redraw the game.This method is invoked every time the "frameTimer"above ticks.
        /// </summary>
        private void Redraw(object sender, ElapsedEventArgs e)
        {
            // Invalidate this form and all its children (true)
            // This will cause the form to redraw as soon as it can
            MethodInvoker invalidator = new MethodInvoker(() => this.Invalidate(true));

            //try catch for keeps this from crashing when closing the form
            try { this.Invoke(invalidator); }
            catch { return; }
        }

        private void connectButton_Click(object sender, EventArgs e) {
            if (serverText.Text == "")
            {
                MessageBox.Show("Please enter a server address");
                connectButton.Enabled = true;
                serverText.Enabled = true;
                nameText.Enabled = true;

            }
            else
            {
                try
                {
                    // Disable the controls and try to connect
                    connectButton.Enabled = false;
                    serverText.Enabled = false;
                    nameText.Enabled = false;

                    // REMOVE
                    System.Diagnostics.Debug.WriteLine("Connect button clicked");

                    Socket server = Network.ConnectToServer(HandleFirstContact, serverText.Text);
                }
                catch
                {
                    MessageBox.Show("Please Enter a valid server address");
                    connectButton.Enabled = true;
                    serverText.Enabled = true;
                    nameText.Enabled = true;
                }
            }
            
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
