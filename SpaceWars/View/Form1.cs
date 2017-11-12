using System;
using System.Net.Sockets;
using System.Windows.Forms;
using NetworkController;

namespace View {
    public partial class spaceWarsForm : Form {
        public spaceWarsForm() {
            InitializeComponent();
        }

        // TODO change this to apply to our GUI
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

            Socket server = Network.ConnectToServer(HandleFirstContact, serverText.Text);
        }
        void HandleFirstContact(SocketState state) {
            state.CallMe = ReceiveStartup;
        }

        void ReceiveStartup(SocketState state) {
            // TODO get data from state
            state.CallMe = ReceiveWorld;
            Network.GetData(state);
        }

        void ReceiveWorld(SocketState state) {
            // TODO
        }
    }
}
