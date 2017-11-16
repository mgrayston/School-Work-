using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Controller;
using Model;

namespace View {
    public partial class spaceWarsForm : Form {
        private World world;

        public spaceWarsForm() {
            InitializeComponent();
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
            world = new World(int.Parse(response[0]), int.Parse(response[1]));

            // REMOVE
            System.Diagnostics.Debug.WriteLine("Received connection info!\nID: " + world.Id + "\nWorld Size: " + world.WorldSize);

            state.Builder.Clear();
            state.CallMe = ReceiveWorld;
            Network.GetData(state);
        }


        private void ReceiveWorld(SocketState state) {
            Processor.ProcessData(world, state);
            Network.GetData(state);
        }

        /// <summary>
        /// Helper method for DrawObjectWithTransform
        /// </summary>
        /// <param name="size">The world (and image) size</param>
        /// <param name="w">The worldspace coordinate</param>
        /// <returns></returns>
        private static int WorldSpaceToImageSpace(int size, double w)
        {
            return (int)w + size / 2;
        }

        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);

        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldSize">The size of one edge of the world (assuming the world is square)</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, int worldSize, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // Perform the transformation
            int x = WorldSpaceToImageSpace(worldSize, worldX);
            int y = WorldSpaceToImageSpace(worldSize, worldY);
            e.Graphics.TranslateTransform(x, y);
            e.Graphics.RotateTransform((float)angle);
            // Draw the object 
            drawer(o, e);
            // Then undo the transformation
            e.Graphics.ResetTransform();
        }

        //todo - copied from canvas
        //private void ShipDrawer(object o, PaintEventArgs e)
        //{
        //    int shipWidth = 35;
        //    Ship s = o as Ship;
        //    if (s.GetID() == ...)
        //        color = ...;
        //    else if (...)
        //        color = ...;
        //    Rectangle r = new Rectangle(-(shipWidth / 2), -(shipWidth / 2), shipWidth, shipWidth);
        //    e.Graphics.FillRectangle(someBrush, r);
        //}
    }
}
