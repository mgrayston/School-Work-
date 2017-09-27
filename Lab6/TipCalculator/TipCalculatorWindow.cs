using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TipCalculator {
    public partial class tipCalculatorWindow : Form {

        private double billTotal;
        private double tipPercent;

        public tipCalculatorWindow() {
            InitializeComponent();
        }

        private void calculateButton_Click(object sender, EventArgs e) {
            double tipAmt = (tipPercent / 100) * billTotal;
            double totalAmt = billTotal + tipAmt;
            tipAmtLabel.Text = "Tip Amount: " + tipAmt.ToString();
            totalLabel.Text = "Total Amount: " + totalAmt;
        }

        private void billBox_TextChanged(object sender, EventArgs e) {
            if (Regex.IsMatch(billBox.Text, "-") || !double.TryParse(billBox.Text, out billTotal)) {
                calculateButton.Enabled = false;
            } else {
                calculateButton.Enabled = true;
            }
        }

        private void tipBox_TextChanged(object sender, EventArgs e) {
            if (Regex.IsMatch(tipBox.Text, "-") || !double.TryParse(tipBox.Text, out tipPercent)) {
                calculateButton.Enabled = false;
            } else {
                calculateButton.Enabled = true;
            }
        }
    }
}
