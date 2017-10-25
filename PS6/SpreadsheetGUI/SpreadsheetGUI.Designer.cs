namespace SpreadsheetGUI {
    partial class SpreadsheetGUI {
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
            this.panel = new SS.SpreadsheetPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newButton = new System.Windows.Forms.ToolStripMenuItem();
            this.saveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.openButton = new System.Windows.Forms.ToolStripMenuItem();
            this.closeButton = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.text_box_name = new System.Windows.Forms.TextBox();
            this.text_box_value = new System.Windows.Forms.TextBox();
            this.text_box_contents = new System.Windows.Forms.TextBox();
            this.label_name = new System.Windows.Forms.Label();
            this.label_value = new System.Windows.Forms.Label();
            this.label_contents = new System.Windows.Forms.Label();
            this.EnterButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.AutoSize = true;
            this.panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 24);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(681, 361);
            this.panel.TabIndex = 0;
            this.panel.SelectionChanged += new SS.SelectionChangedHandler(this.panel_SelectionChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(681, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newButton,
            this.saveButton,
            this.openButton,
            this.closeButton});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newButton
            // 
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(152, 22);
            this.newButton.Text = "New";
            this.newButton.Click += new System.EventHandler(this.newButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(152, 22);
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // openButton
            // 
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(152, 22);
            this.openButton.Text = "Open";
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(152, 22);
            this.closeButton.Text = "Close";
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // text_box_name
            // 
            this.text_box_name.Location = new System.Drawing.Point(342, 4);
            this.text_box_name.MaxLength = 3;
            this.text_box_name.Name = "text_box_name";
            this.text_box_name.ReadOnly = true;
            this.text_box_name.Size = new System.Drawing.Size(35, 20);
            this.text_box_name.TabIndex = 1;
            // 
            // text_box_value
            // 
            this.text_box_value.Location = new System.Drawing.Point(454, 4);
            this.text_box_value.Name = "text_box_value";
            this.text_box_value.ReadOnly = true;
            this.text_box_value.Size = new System.Drawing.Size(88, 20);
            this.text_box_value.TabIndex = 2;
            // 
            // text_box_contents
            // 
            this.text_box_contents.Location = new System.Drawing.Point(648, 4);
            this.text_box_contents.Name = "text_box_contents";
            this.text_box_contents.Size = new System.Drawing.Size(301, 20);
            this.text_box_contents.TabIndex = 3;
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label_name.Location = new System.Drawing.Point(300, 6);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(38, 13);
            this.label_name.TabIndex = 0;
            this.label_name.Text = "Name:";
            this.label_name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_value
            // 
            this.label_value.AutoSize = true;
            this.label_value.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label_value.Location = new System.Drawing.Point(411, 6);
            this.label_value.Name = "label_value";
            this.label_value.Size = new System.Drawing.Size(37, 13);
            this.label_value.TabIndex = 0;
            this.label_value.Text = "Value:";
            this.label_value.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_contents
            // 
            this.label_contents.AutoSize = true;
            this.label_contents.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label_contents.Location = new System.Drawing.Point(590, 6);
            this.label_contents.Name = "label_contents";
            this.label_contents.Size = new System.Drawing.Size(52, 13);
            this.label_contents.TabIndex = 0;
            this.label_contents.Text = "Contents:";
            this.label_contents.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EnterButton
            // 
            this.EnterButton.Location = new System.Drawing.Point(951, 0);
            this.EnterButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.EnterButton.Name = "EnterButton";
            this.EnterButton.Size = new System.Drawing.Size(42, 23);
            this.EnterButton.TabIndex = 4;
            this.EnterButton.Text = "Enter";
            this.EnterButton.UseVisualStyleBackColor = true;
            this.EnterButton.Click += new System.EventHandler(this.EnterButton_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // SpreadsheetGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 385);
            this.Controls.Add(this.EnterButton);
            this.Controls.Add(this.label_contents);
            this.Controls.Add(this.label_value);
            this.Controls.Add(this.label_name);
            this.Controls.Add(this.text_box_contents);
            this.Controls.Add(this.text_box_value);
            this.Controls.Add(this.text_box_name);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SpreadsheetGUI";
            this.Text = "Spreadsheet";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel panel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newButton;
        private System.Windows.Forms.ToolStripMenuItem saveButton;
        private System.Windows.Forms.ToolStripMenuItem openButton;
        private System.Windows.Forms.ToolStripMenuItem closeButton;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.TextBox text_box_name;
        private System.Windows.Forms.TextBox text_box_value;
        private System.Windows.Forms.TextBox text_box_contents;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label_value;
        private System.Windows.Forms.Label label_contents;
        private System.Windows.Forms.Button EnterButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

