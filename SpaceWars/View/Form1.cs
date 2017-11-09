using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        // TODO change this to apply to our GUI
        private void connectButton_Click(object sender, EventArgs e) {
            // TODO: This needs better error handling. Left as an exercise.
            // If the server box is empty, it gives a message, but doesn't allow us to try to reconnect.
            // It also doesn't handle unreachable addresses.
            if (serverAddress.Text == "") {
                MessageBox.Show("Please enter a server address");
                return;
            }


            // Disable the controls and try to connect
            connectButton.Enabled = false;
            serverAddress.Enabled = false;


            ConnectToServer(serverAddress.Text);
        }
    }
}
