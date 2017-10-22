﻿using System;
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

        public SpreadsheetGUI() {
            ss = new Spreadsheet(s => true, s => s, "ps6");
            InitializeComponent();
        }
    }
}
