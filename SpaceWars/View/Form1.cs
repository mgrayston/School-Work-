using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Controller;
using Model;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Input;
using System.IO;

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

        // DrawingPanel where all objects are drawn
        private ScorePanel scorePanel;

        // Used to collect keystrokes and send them to the server
        private StringBuilder keystrokes;

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
                state.CallMe = ReceiveStartup;
                Network.Send(state.Socket, nameText.Text + "\n");
                Network.GetData(state);
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
            this.Invoke(new MethodInvoker(() => ClientSize = new Size(theWorld.WorldSize + 200, theWorld.WorldSize + 30)));
            this.Invoke(new MethodInvoker(() => this.FormBorderStyle = FormBorderStyle.FixedSingle));
            drawingPanel = new DrawingPanel(theWorld);
            drawingPanel.Location = new Point(0, 30);
            drawingPanel.Size = new Size(theWorld.WorldSize, theWorld.WorldSize);
            drawingPanel.BackColor = Color.Black;

            //add scoreboard
            scorePanel = new ScorePanel(theWorld);
            scorePanel.Location = new Point(theWorld.WorldSize, 30);
            scorePanel.Size = new Size(ClientSize.Width-theWorld.WorldSize, theWorld.WorldSize);
            scorePanel.BackColor = Color.White;

            // Add the drawingPanel and invalidate the client to have it redrawn
            this.Invoke(new MethodInvoker(() => this.Controls.Add(drawingPanel)));
            this.Invoke(new MethodInvoker(() => this.Controls.Add(scorePanel)));
            this.Invoke(new MethodInvoker(() => this.Invalidate()));

            this.Invoke(new MethodInvoker(() => this.timer.Enabled = true));
            this.Invoke(new MethodInvoker(() => this.timer.Start()));

            Thread keyCapture = new Thread(keyCapturer);
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
                this.helpButton.Enabled = false;
            }
            else {
                this.connectButton.Enabled = true;
                this.serverText.Enabled = true;
                this.nameText.Enabled = true;
                this.helpButton.Enabled = true;
            }
        }

        /// <summary>
        /// Called when the Form Timer ticks. Used 
        /// to regulate client framerate. Currently 
        /// set to 7 milliseconds (~140 FPS).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e) {
            drawingPanel.Invalidate();
        }

        /// <summary>
        /// Captures all input from user and sends it 
        /// to the server.
        /// </summary>
        private void keyCapturer() {
            // StringBuilder to store all current keystrokes
            StringBuilder tmp = new StringBuilder();

            while (true) {
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

                Network.Send(state.Socket, tmp.ToString() + "\n");
                tmp.Clear();
            }
        }

        /// <summary>
        /// Used to ensure the connection is closed when the form is exited.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpaceWarsForm_FormClosed(object sender, FormClosedEventArgs e) {
            state.Socket.Close();
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            showReadMe();
        }

        /// <summary>
        /// Message box displays README.txt file to explain functionality to User.
        /// </summary>
        /// <returns></returns>
        private DialogResult showReadMe()
        {
            return MessageBox.Show(File.ReadAllText(@"..\..\..\Resources\README.txt"), "README", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
