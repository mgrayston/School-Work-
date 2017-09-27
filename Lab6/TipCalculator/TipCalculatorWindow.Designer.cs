namespace TipCalculator {
    partial class tipCalculatorWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.calculateButton = new System.Windows.Forms.Button();
            this.promptLabel = new System.Windows.Forms.Label();
            this.billBox = new System.Windows.Forms.TextBox();
            this.tipBox = new System.Windows.Forms.TextBox();
            this.tipLabel = new System.Windows.Forms.Label();
            this.totalLabel = new System.Windows.Forms.Label();
            this.tipAmtLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // calculateButton
            // 
            this.calculateButton.Location = new System.Drawing.Point(120, 203);
            this.calculateButton.Name = "calculateButton";
            this.calculateButton.Size = new System.Drawing.Size(118, 41);
            this.calculateButton.TabIndex = 0;
            this.calculateButton.Text = "Calculate";
            this.calculateButton.UseVisualStyleBackColor = true;
            this.calculateButton.Click += new System.EventHandler(this.calculateButton_Click);
            // 
            // promptLabel
            // 
            this.promptLabel.AutoSize = true;
            this.promptLabel.Location = new System.Drawing.Point(44, 62);
            this.promptLabel.Name = "promptLabel";
            this.promptLabel.Size = new System.Drawing.Size(194, 25);
            this.promptLabel.TabIndex = 1;
            this.promptLabel.Text = "Enter the Bill Total:";
            // 
            // billBox
            // 
            this.billBox.Location = new System.Drawing.Point(370, 59);
            this.billBox.Name = "billBox";
            this.billBox.Size = new System.Drawing.Size(100, 31);
            this.billBox.TabIndex = 2;
            this.billBox.TextChanged += new System.EventHandler(this.billBox_TextChanged);
            // 
            // tipBox
            // 
            this.tipBox.Location = new System.Drawing.Point(370, 124);
            this.tipBox.Name = "tipBox";
            this.tipBox.Size = new System.Drawing.Size(100, 31);
            this.tipBox.TabIndex = 4;
            this.tipBox.TextChanged += new System.EventHandler(this.tipBox_TextChanged);
            // 
            // tipLabel
            // 
            this.tipLabel.AutoSize = true;
            this.tipLabel.Location = new System.Drawing.Point(17, 127);
            this.tipLabel.Name = "tipLabel";
            this.tipLabel.Size = new System.Drawing.Size(221, 25);
            this.tipLabel.TabIndex = 5;
            this.tipLabel.Text = "Enter Tip Percentage:";
            // 
            // totalLabel
            // 
            this.totalLabel.AutoSize = true;
            this.totalLabel.Location = new System.Drawing.Point(319, 278);
            this.totalLabel.Name = "totalLabel";
            this.totalLabel.Size = new System.Drawing.Size(151, 25);
            this.totalLabel.TabIndex = 6;
            this.totalLabel.Text = "Total Amount: ";
            // 
            // tipAmtLabel
            // 
            this.tipAmtLabel.AutoSize = true;
            this.tipAmtLabel.Location = new System.Drawing.Point(337, 211);
            this.tipAmtLabel.Name = "tipAmtLabel";
            this.tipAmtLabel.Size = new System.Drawing.Size(133, 25);
            this.tipAmtLabel.TabIndex = 7;
            this.tipAmtLabel.Text = "Tip Amount: ";
            // 
            // tipCalculatorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 333);
            this.Controls.Add(this.tipAmtLabel);
            this.Controls.Add(this.totalLabel);
            this.Controls.Add(this.tipLabel);
            this.Controls.Add(this.tipBox);
            this.Controls.Add(this.billBox);
            this.Controls.Add(this.promptLabel);
            this.Controls.Add(this.calculateButton);
            this.Name = "tipCalculatorWindow";
            this.Text = "Tip Calculator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button calculateButton;
        private System.Windows.Forms.Label promptLabel;
        private System.Windows.Forms.TextBox billBox;
        private System.Windows.Forms.TextBox tipBox;
        private System.Windows.Forms.Label tipLabel;
        private System.Windows.Forms.Label totalLabel;
        private System.Windows.Forms.Label tipAmtLabel;
    }
}

