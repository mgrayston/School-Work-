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
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
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
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.AutoSize = true;
            this.spreadsheetPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.spreadsheetPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 24);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(1239, 575);
            this.spreadsheetPanel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1239, 24);
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
            // 
            // saveButton
            // 
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(152, 22);
            this.saveButton.Text = "Save";
            // 
            // openButton
            // 
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(152, 22);
            this.openButton.Text = "Open";
            // 
            // closeButton
            // 
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(152, 22);
            this.closeButton.Text = "Close";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // text_box_name
            // 
            this.text_box_name.Location = new System.Drawing.Point(189, 1);
            this.text_box_name.MaxLength = 3;
            this.text_box_name.Name = "text_box_name";
            this.text_box_name.ReadOnly = true;
            this.text_box_name.Size = new System.Drawing.Size(35, 20);
            this.text_box_name.TabIndex = 1;
            this.text_box_name.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // text_box_value
            // 
            this.text_box_value.Location = new System.Drawing.Point(297, 1);
            this.text_box_value.Name = "text_box_value";
            this.text_box_value.ReadOnly = true;
            this.text_box_value.Size = new System.Drawing.Size(88, 20);
            this.text_box_value.TabIndex = 2;
            // 
            // text_box_contents
            // 
            this.text_box_contents.Location = new System.Drawing.Point(466, 1);
            this.text_box_contents.Name = "text_box_contents";
            this.text_box_contents.Size = new System.Drawing.Size(301, 20);
            this.text_box_contents.TabIndex = 3;
            this.text_box_contents.TextChanged += new System.EventHandler(this.text_box_contents_TextChanged);
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label_name.Location = new System.Drawing.Point(147, 4);
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
            this.label_value.Location = new System.Drawing.Point(255, 4);
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
            this.label_contents.Location = new System.Drawing.Point(409, 4);
            this.label_contents.Name = "label_contents";
            this.label_contents.Size = new System.Drawing.Size(52, 13);
            this.label_contents.TabIndex = 0;
            this.label_contents.Text = "Contents:";
            this.label_contents.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SpreadsheetGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1239, 599);
            this.Controls.Add(this.label_contents);
            this.Controls.Add(this.label_value);
            this.Controls.Add(this.label_name);
            this.Controls.Add(this.text_box_contents);
            this.Controls.Add(this.text_box_value);
            this.Controls.Add(this.text_box_name);
            this.Controls.Add(this.spreadsheetPanel1);
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

        private SS.SpreadsheetPanel spreadsheetPanel1;
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
    }
}

