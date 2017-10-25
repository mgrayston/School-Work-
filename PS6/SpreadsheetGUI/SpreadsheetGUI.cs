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
using System.IO;

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

        /// <summary>
        /// Closes the GUI from the menu bar close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Open button from file menu bar opens a file explorer, so a file can be opened. 
        /// .sprd files are only displayed by default, but all file types can also be shown 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open File";
            ofd.Filter = "Spreadsheet (*.sprd)|*.sprd | All Files (*.*)|*.*"; //displays only .sprd by default, else all files
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader readFile = new StreamReader(File.OpenRead(ofd.FileName));

                //TODO - Take read file and place into spreadsheet

                //TODO - Check for invalid file type?

                //TODO - Open in new window could be an additional feature
            }

        }

        /// <summary>
        /// Save button from file menu bar opens a file explorer, so that a file can be save
        /// .sprd is default save type. All file types and .txt are also available
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save File";
            sfd.Filter = "Spreadsheet (*.sprd)|*.sprd | All Files (*.*)|*.* |Text File (*.txt)|*.txt" ; 
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writeFile = new StreamWriter(File.Create(sfd.FileName));

                //TODO - write spreadsheet into xml

               
            }
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            //TODO
        }
    }
}
