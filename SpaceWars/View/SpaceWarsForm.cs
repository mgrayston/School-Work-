using System;
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
        // Used to access the socket for sending movement commands
        SocketState state;

        // World is a simple container for Players and Powerups
        private World theWorld;

        // DrawingPanel where all objects are drawn
        private DrawingPanel drawingPanel;

        // DrawingPanel where scoreboard is drawn
        private ScorePanel scorePanel;

        // Used to collect keystrokes and send them to the server
        private StringBuilder keystrokes;

        // Used to capture keys
        private Thread keyCapture;

        /// <summary>
        /// Form initializer
        /// </summary>
        public SpaceWarsForm() {
            InitializeComponent();
            this.AcceptButton = connectButton;
            keystrokes = new StringBuilder();
        }

        /// <summary>
        /// Button to allow connecting to the specified server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectButton_Click(object sender, EventArgs e) {
            // Disable inputs and ConnectButton
            ToggleInputEnabled();

            // Catch empty server addresses
            if (serverText.Text == "") {
                MessageBox.Show("Please enter a server address");
                // Reenable inputs and ConnectButton
                ToggleInputEnabled();
            }
            else {
                Network.ConnectToServer(HandleFirstContact, serverText.Text);
            }
        }

        /// <summary>
        /// Called after attempting to connect to the server.
        /// If connection was successful, send name to complete handshake.
        /// Otherwise, prompts user to try connecting again.
        /// </summary>
        /// <param name="state"></param>
        private void HandleFirstContact(SocketState state) {
            if (!state.Socket.Connected) {
                MessageBox.Show("Connection failed, please try again.");
                this.Invoke(new MethodInvoker(ToggleInputEnabled));
            }
            else {
                this.state = state;
                this.state.CallMe = ReceiveStartup;
                Network.Send(this.state.Socket, nameText.Text + "\n");
                Network.GetData(this.state);
            }
        }

        /// <summary>
        /// Receives information about the world and adds the DrawingPanel 
        /// so that the world can begin being drawn.
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveStartup(SocketState state) {
            String[] response = Regex.Split(state.Builder.ToString(), @"(?<=[\n])");
            theWorld = new World(int.Parse(response[1]));
            // Change the client size based on the received worldSize; constant number is so the DrawingPanel 
            // is not drawn where the buttons are, and to provide space for the scoreboard.
            this.Invoke(new MethodInvoker(() => ClientSize = new Size(theWorld.WorldSize + 250, theWorld.WorldSize + 30)));
            this.Invoke(new MethodInvoker(() => this.FormBorderStyle = FormBorderStyle.FixedSingle));
            drawingPanel = new DrawingPanel(theWorld);
            drawingPanel.Location = new Point(0, 30);
            drawingPanel.Size = new Size(theWorld.WorldSize, theWorld.WorldSize);
            drawingPanel.BackColor = Color.Black;

            // Add scoreboard
            scorePanel = new ScorePanel(theWorld);
            scorePanel.Location = new Point(theWorld.WorldSize, 30);
            scorePanel.Size = new Size(ClientSize.Width - theWorld.WorldSize, theWorld.WorldSize);

            // Add the drawingPanel and invalidate the client to have it redrawn
            this.Invoke(new MethodInvoker(() => this.Controls.Add(drawingPanel)));
            this.Invoke(new MethodInvoker(() => this.Controls.Add(scorePanel)));
            this.Invoke(new MethodInvoker(() => this.Invalidate()));
            this.Invoke(new MethodInvoker(() => this.redrawTimer.Enabled = true));
            this.Invoke(new MethodInvoker(() => this.redrawTimer.Start()));

            keyCapture = new Thread(KeyCapturer);
            keyCapture.SetApartmentState(ApartmentState.STA);
            keyCapture.Start();

            state.Builder.Clear();
            state.CallMe = ReceiveWorld;
            Network.GetData(state);
        }

        /// <summary>
        /// Recieves World information from the server and sends 
        /// it to be processed. Begins receiving more information 
        /// afterward.
        /// </summary>
        /// <param name="state"></param>
        private void ReceiveWorld(SocketState state) {
            Processor.ProcessData(theWorld, state);
            drawingPanel.Invalidate();
            Network.GetData(state);
        }

        /// <summary>
        /// Toggles the input boxes and ConnectButton.
        /// </summary>
        private void ToggleInputEnabled() {
            if (this.connectButton.Enabled) {
                this.connectButton.Enabled = false;
                this.serverText.Enabled = false;
                this.nameText.Enabled = false;
            }
            else {
                this.connectButton.Enabled = true;
                this.serverText.Enabled = true;
                this.nameText.Enabled = true;
                this.Focus();
            }
        }

        /// <summary>
        /// Called when the Form Timer ticks. Used 
        /// to regulate client framerate. Currently 
        /// set to 7 milliseconds (~140 FPS).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e) {
            drawingPanel.Invalidate();
            scorePanel.Invalidate();
        }

        /// <summary>
        /// Captures all input from user and sends it 
        /// to the server.
        /// </summary>
        private void KeyCapturer() {
            // StringBuilder to store all current keystrokes
            StringBuilder tmp = new StringBuilder();

            while (state.Socket.Connected) {
                if (Keyboard.IsKeyDown(Key.Up)) {
                    tmp.Append("(T)");
                }
                if (Keyboard.IsKeyDown(Key.Right)) {
                    tmp.Append("(R)");
                }
                if (Keyboard.IsKeyDown(Key.Left)) {
                    tmp.Append("(L)");
                }
                if (Keyboard.IsKeyDown(Key.Space)) {
                    tmp.Append("(F)");
                }

                try {
                    this.Invoke(new MethodInvoker(() => Network.Send(state.Socket, tmp.ToString() + "\n")));
                }
                catch (Exception e) {
                    if (e is ObjectDisposedException) {
                        break;
                    }
                    else {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                }
                tmp.Clear();
            }
        }

        /// <summary>
        /// Opens an info dialog when the helpButton is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpButton_Click(object sender, EventArgs e) {
            ShowReadMe();
        }

        /// <summary>
        /// Message box displays README.txt file to explain functionality to User.
        /// </summary>
        /// <returns></returns>
        private DialogResult ShowReadMe() {
            return MessageBox.Show(Properties.Resources.README);
        }

        /// <summary>
        /// Finishes closing and disposing of the Socket.
        /// </summary>
        /// <param name="ar"></param>
        private void DisposeSocket(IAsyncResult ar) {
            state.Socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            state.Socket.Close();
        }

        /// <summary>
        /// Used to ensure the connection is closed when the form is exited.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpaceWarsForm_FormClosing(object sender, FormClosingEventArgs e) {
            this.BeginInvoke(new MethodInvoker(() => this.redrawTimer.Stop()));
            this.BeginInvoke(new MethodInvoker(() => this.redrawTimer.Enabled = false));
            this.BeginInvoke(new MethodInvoker(() => this.redrawTimer.Dispose()));
            if (state != null && state.Socket.Connected) {
                state.Socket.BeginDisconnect(false, DisposeSocket, state);
            }
            Application.Exit();
        }
    }
}
