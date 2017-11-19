using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Controller;
using Model;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace View {
    /// <summary>
    /// Form used to display the SpaceWars game
    /// </summary>
    public partial class SpaceWarsForm : Form {

        // REMOVE?
        SocketState state;

        // World is a simple container for Players and Powerups
        private World theWorld;

        // DrawingPanel where all objects are drawn
        private DrawingPanel drawingPanel;

        // Used to collect keystrokes and send them to the server
        private StringBuilder keystrokes;

        // Used to check if we are connected to the server
        private bool connected;

        /// <summary>
        /// Form initializer
        /// </summary>
        public SpaceWarsForm() {
            InitializeComponent();
            this.AcceptButton = connectButton;
            keystrokes = new StringBuilder();
            connected = false;
        }

        private void connectButton_Click(object sender, EventArgs e) {
            if (serverText.Text == "") {
                MessageBox.Show("Please enter a server address");
                connectButton.Enabled = true;
                serverText.Enabled = true;
                nameText.Enabled = true;

            }
            else {
                try {
                    // Disable the controls and try to connect
                    connectButton.Enabled = false;
                    serverText.Enabled = false;
                    nameText.Enabled = false;

                    // REMOVE
                    System.Diagnostics.Debug.WriteLine("Connect button clicked");

                    Socket server = Network.ConnectToServer(HandleFirstContact, serverText.Text);
                }
                catch {
                    MessageBox.Show("Please Enter a valid server address");
                    connectButton.Enabled = true;
                    serverText.Enabled = true;
                    nameText.Enabled = true;
                }
            }

        }

        private void HandleFirstContact(SocketState state) {
            this.state = state;
            state.CallMe = ReceiveStartup;
            Network.Send(state.Socket, nameText.Text + "\n");
            Network.GetData(state);
        }

        private void ReceiveStartup(SocketState state) {
            String[] response = Regex.Split(state.Builder.ToString(), @"(?<=[\n])");
            theWorld = new World(int.Parse(response[1]));
            // Change the client size based on the received worldSize
            this.Invoke(new MethodInvoker(() => ClientSize = new Size(theWorld.WorldSize, theWorld.WorldSize)));
            drawingPanel = new DrawingPanel(theWorld);
            drawingPanel.Location = new Point(0, 50);
            drawingPanel.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
            drawingPanel.BackColor = Color.Black;

            // Add the drawingPanel and invalidate the client to have it redrawn
            this.Invoke(new MethodInvoker(() => this.Controls.Add(drawingPanel)));
            this.Invoke(new MethodInvoker(() => this.Invalidate()));

            this.Invoke(new MethodInvoker(() => this.timer.Enabled = true));
            this.Invoke(new MethodInvoker(() => this.timer.Start()));

            connected = true;
            Thread keyCapture = new Thread(keyCapturer);
            keyCapture.SetApartmentState(ApartmentState.STA);
            keyCapture.Start();

            state.Builder.Clear();
            state.CallMe = ReceiveWorld;
            Network.GetData(state);
        }

        private void ReceiveWorld(SocketState state) {
            Processor.ProcessData(theWorld, state);
            drawingPanel.Invalidate();
            Network.GetData(state);
        }

        private void timer_Tick(object sender, EventArgs e) {
            drawingPanel.Invalidate();
            if (connected) {
                lock (keystrokes) {
                    string dataToSend = keystrokes.ToString() + "\n";
                    keystrokes.Clear();
                    Network.Send(state.Socket, dataToSend);
                }
            }
        }

        // TODO this slows it down quite a bit and doesn't increase speed of key capture, but does capture multiple keys
        //private void keyCapturer() {
        //    StringBuilder tmp = new StringBuilder();
        //    while (true) {
        //        if (Keyboard.IsKeyDown(Key.Up)) {
        //            tmp.Append("(T)");
        //        }
        //        if (Keyboard.IsKeyDown(Key.Right)) {
        //            tmp.Append("(R)");
        //        }
        //        if (Keyboard.IsKeyDown(Key.Left)) {
        //            tmp.Append("(L)");
        //        }
        //        if (Keyboard.IsKeyDown(Key.Space)) {
        //            tmp.Append("(F)");
        //        }

        //        this.Invoke(new MethodInvoker(() => this.keystrokes.Append(tmp.ToString())));
        //        tmp.Clear();
        //    }
        //}

        // TODO not fast, and if you press another key, previous keys stop
        // protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
        //    switch (keyData) {
        //        case Keys.Up:
        //            keystrokes.Append("(T)");
        //            break;
        //        case Keys.Left:
        //            keystrokes.Append("(L)");
        //            break;
        //        case Keys.Right:
        //            keystrokes.Append("(R)");
        //            break;
        //        case Keys.Space:
        //            keystrokes.Append("(F)");
        //            break;
        //        default:
        //            return base.ProcessCmdKey(ref msg, keyData);
        //    }
        //    return true;
        //}
    }
}
