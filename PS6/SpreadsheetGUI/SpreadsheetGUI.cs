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
using System.Threading;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;

namespace SpreadsheetGUI {
    public partial class SpreadsheetGUI : Form {

        private Spreadsheet ss; // Backing Spreadsheet for this GUI
        private string validVariable = "^[A-Z]{1}[1-99]$";

        public SpreadsheetGUI() {
            ss = new Spreadsheet(s => Regex.IsMatch(s, validVariable), s => s.ToUpper(), "ps6");
            InitializeComponent();
            refreshCell("A1");
            contentsBox.Focus();    // TODO not working..?
        }

        private void EnterButton_Click(object sender, EventArgs e) {
            int col, row;
            panel.GetSelection(out col, out row);
            if (backgroundWorker1.IsBusy) {
                backgroundWorker2.RunWorkerAsync(getCellName(col, row));
            }
            else {
                backgroundWorker1.RunWorkerAsync(getCellName(col, row));
            }
        }

        private void panel_SelectionChanged(SpreadsheetPanel sender) {
            contentsBox.Focus();
            int col, row;
            panel.GetSelection(out col, out row);
            refreshCell(getCellName(col, row));
        }

        private void refreshCell(string cell) {
            if (InvokeRequired) {
                this.Invoke(new Action<string>(refreshCell), new object[] { cell });
            }

            int col = getCellCoord(cell);
            int row = int.Parse(cell.Substring(1)) - 1;
            cellBox.Text = cell;
            object cellValue = ss.GetCellValue(cell);
            if (cellValue is FormulaError) {
                valueBox.Text = ((FormulaError)cellValue).Reason;
            }
            else {
                valueBox.Text = cellValue.ToString();
            }
            object cellContents = ss.GetCellContents(cell);
            if (cellContents is Formula) {
                contentsBox.Text = "=" + ((Formula)cellContents).ToString();
            }
            else {
                contentsBox.Text = cellContents.ToString();
            }
        }

        private string getCellName(int col, int row) {
            return (char)(col + 65) + "" + (row + 1);
        }

        private int getCellCoord(string cell) {
            return ((char)cell[0]) - 65;
        }

        private void updateCells(ISet<string> cells) {
            foreach (string cell in cells) {
                object cellValue = ss.GetCellValue(cell);
                if (cellValue is FormulaError) {
                    panel.SetValue(getCellCoord(cell), int.Parse(cell.Substring(1)) - 1, ((FormulaError)cellValue).Reason);
                }
                else {
                    panel.SetValue(getCellCoord(cell), int.Parse(cell.Substring(1)) - 1, cellValue.ToString());
                }
            }
        }

        /// <summary>
        /// Closes the GUI from the menu bar close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        /// <summary>
        /// Open button from file menu bar opens a file explorer, so a file can be opened. 
        /// .sprd files are only displayed by default, but all file types can also be shown 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openButton_Click(object sender, EventArgs e) {
            if (ss.Changed) {
                DialogResult dr = promptSave();
                switch (dr) {
                    case DialogResult.Yes:
                        saveButton_Click(sender, e);
                        break;

                    case DialogResult.No:
                        break;

                    case DialogResult.Cancel:
                        return;
                }
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open File";
            ofd.Filter = "Spreadsheet (*.sprd)|*.sprd|All Files (*.*)|*.*"; // displays only .sprd by default, else all files
            if (ofd.ShowDialog() == DialogResult.OK) {

                // check for unsaved work before saving and prompt
                ss = new Spreadsheet(ofd.FileName, s => Regex.IsMatch(s, validVariable), s => s.ToUpper(), "ps6");
                panel.Clear();
                updateCells(new HashSet<string>(ss.GetNamesOfAllNonemptyCells()));

                //change window name to name of file
                string name = ofd.FileName;
                this.Text = string.Format("{0} - MyEditor", System.IO.Path.GetFileName(name));

                int col, row;
                panel.GetSelection(out col, out row);
                refreshCell(getCellName(col, row));
                ss.Save(ofd.FileName); //resets "Changed" property to false

            }
        }

        /// <summary>
        /// Save button from file menu bar opens a file explorer, so that a file can be save
        /// .sprd is default save type. All file types and .txt are also available
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, EventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save File";
            sfd.Filter = "Spreadsheet (*.sprd)|*.sprd|All Files (*.*)|*.* |Text File (*.txt)|*.txt";
            sfd.FileName = this.Text;
            if (sfd.ShowDialog() == DialogResult.OK) {
     
                ss.Save(sfd.FileName);
                //change window name to name of file
                string name = sfd.FileName;
                this.Text = string.Format("{0} - MyEditor", System.IO.Path.GetFileName(name));
            }
        }

        /// <summary>
        /// From the menu bar, "open" button opens a new GUI window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newButton_Click(object sender, EventArgs e) {
            SpreadsheetGUI newWindow = new SpreadsheetGUI();
            int count = SSApplicationContext.getAppContext().RunWindow(newWindow);
            newWindow.Text = "Spreadsheet " + count; // change spreadsheet title
        }

        private DialogResult promptSave() {
            return MessageBox.Show("Unsaved changes detected. Would you like to save?","Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// Message box displays README.txt file to explain functionality to User.
        /// </summary>
        /// <returns></returns>
        private DialogResult showReadMe()
        {

            return MessageBox.Show(File.ReadAllText(@"..\..\..\Resources\README.txt"), "README", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Handles the DoWork event of the backgroundWorker1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            try {
                updateCells(ss.SetContentsOfCell((string)e.Argument, contentsBox.Text));
                refreshCell((string)e.Argument);
            }
            catch (Exception exc) {
                MessageBox.Show("Error occured!\n" + exc.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the DoWork event of the backgroundWorker2 control.
        /// Backup in case backgroundWorker1 is busy.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e) {
            try {
                updateCells(ss.SetContentsOfCell((string)e.Argument, contentsBox.Text));
                refreshCell((string)e.Argument);
            }
            catch (Exception exc) {
                MessageBox.Show("Error occured!\n" + exc.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SpreadsheetGUI_FormClosing(object sender, FormClosingEventArgs e) {
            if (ss.Changed) {
                DialogResult dr = promptSave();
                switch (dr) {
                    case DialogResult.Yes:
                        saveButton_Click(sender, e);
                        break;

                    case DialogResult.No:
                        e.Cancel = false;
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showReadMe();
        }
    }
}
