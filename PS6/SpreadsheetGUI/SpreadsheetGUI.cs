using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;

namespace SpreadsheetGUI {
    public partial class SpreadsheetGUI : Form {

        private Spreadsheet ss; // Backing Spreadsheet for this GUI

        // TODO make constructor for use with 'open' button
        public SpreadsheetGUI() {
            ss = new Spreadsheet(s => true, s => s.ToUpper(), "ps6");
            InitializeComponent();
            AcceptButton = EnterButton;
            text_box_name.Text = "A1";
            // TODO autopopulate other text boxes and cell values from spreadsheet -> goes in constructor for SpreadsheetGUI with given filename
        }

        private void EnterButton_Click(object sender, EventArgs e) {
            // TODO check if changed cells > 5-10ish; if so, call backgroundworker
            // TODO set cell to value of cell
            // UPDATE textboxes and cells through loop of returned cells
        }

        private void panel_SelectionChanged(SpreadsheetPanel sender) {
            // TODO set contentsBox to contents of cell, valueBox to value of cell
            int col, row;
            panel.GetSelection(out col, out row);
            text_box_name.Text = getCellName(col, row);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            // TODO either check if changed values > 5-10ish OR create backgroundworker pool to handle ALL events
        }

        private string getCellName(int col, int row) {
            return (char)(col + 65) + "" + (row + 1);
        }
    }
}
