namespace Sowalabs.Bison.ProfitSim
{
    partial class MultiProfitSimForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._simulateButton = new System.Windows.Forms.Button();
            this._textBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this._cancelButton = new System.Windows.Forms.Button();
            this.toDateLabel = new System.Windows.Forms.Label();
            this.fromDateLabel = new System.Windows.Forms.Label();
            this._toDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this._fromDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.loadConfigButton = new System.Windows.Forms.Button();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.logBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _simulateButton
            // 
            this._simulateButton.Location = new System.Drawing.Point(97, 47);
            this._simulateButton.Margin = new System.Windows.Forms.Padding(2);
            this._simulateButton.Name = "_simulateButton";
            this._simulateButton.Size = new System.Drawing.Size(66, 33);
            this._simulateButton.TabIndex = 6;
            this._simulateButton.Text = "Simulate!";
            this._simulateButton.UseVisualStyleBackColor = true;
            this._simulateButton.Click += new System.EventHandler(this.SimulateButton_Click);
            // 
            // _textBox
            // 
            this._textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._textBox.Location = new System.Drawing.Point(0, 0);
            this._textBox.Multiline = true;
            this._textBox.Name = "_textBox";
            this._textBox.Size = new System.Drawing.Size(1046, 387);
            this._textBox.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancelButton);
            this.panel1.Controls.Add(this.toDateLabel);
            this.panel1.Controls.Add(this.fromDateLabel);
            this.panel1.Controls.Add(this._toDateTimePicker);
            this.panel1.Controls.Add(this._fromDateTimePicker);
            this.panel1.Controls.Add(this.loadConfigButton);
            this.panel1.Controls.Add(this._simulateButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 387);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1046, 82);
            this.panel1.TabIndex = 8;
            // 
            // _cancelButton
            // 
            this._cancelButton.Enabled = false;
            this._cancelButton.Location = new System.Drawing.Point(167, 47);
            this._cancelButton.Margin = new System.Windows.Forms.Padding(2);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(66, 33);
            this._cancelButton.TabIndex = 12;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // toDateLabel
            // 
            this.toDateLabel.AutoSize = true;
            this.toDateLabel.Location = new System.Drawing.Point(244, 12);
            this.toDateLabel.Name = "toDateLabel";
            this.toDateLabel.Size = new System.Drawing.Size(16, 13);
            this.toDateLabel.TabIndex = 11;
            this.toDateLabel.Text = "to";
            // 
            // fromDateLabel
            // 
            this.fromDateLabel.AutoSize = true;
            this.fromDateLabel.Location = new System.Drawing.Point(8, 12);
            this.fromDateLabel.Name = "fromDateLabel";
            this.fromDateLabel.Size = new System.Drawing.Size(70, 13);
            this.fromDateLabel.TabIndex = 10;
            this.fromDateLabel.Text = "Simulate from";
            // 
            // _toDateTimePicker
            // 
            this._toDateTimePicker.CustomFormat = "dd.MM.yyyy HH";
            this._toDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this._toDateTimePicker.Location = new System.Drawing.Point(266, 6);
            this._toDateTimePicker.MinDate = new System.DateTime(2018, 4, 3, 0, 0, 0, 0);
            this._toDateTimePicker.Name = "_toDateTimePicker";
            this._toDateTimePicker.Size = new System.Drawing.Size(141, 20);
            this._toDateTimePicker.TabIndex = 9;
            // 
            // _fromDateTimePicker
            // 
            this._fromDateTimePicker.CustomFormat = "dd.MM.yyyy HH";
            this._fromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this._fromDateTimePicker.Location = new System.Drawing.Point(97, 6);
            this._fromDateTimePicker.MinDate = new System.DateTime(2018, 4, 3, 0, 0, 0, 0);
            this._fromDateTimePicker.Name = "_fromDateTimePicker";
            this._fromDateTimePicker.Size = new System.Drawing.Size(141, 20);
            this._fromDateTimePicker.TabIndex = 8;
            // 
            // loadConfigButton
            // 
            this.loadConfigButton.Location = new System.Drawing.Point(11, 47);
            this.loadConfigButton.Margin = new System.Windows.Forms.Padding(2);
            this.loadConfigButton.Name = "loadConfigButton";
            this.loadConfigButton.Size = new System.Drawing.Size(82, 33);
            this.loadConfigButton.TabIndex = 7;
            this.loadConfigButton.Text = "Load config";
            this.loadConfigButton.UseVisualStyleBackColor = true;
            this.loadConfigButton.Click += new System.EventHandler(this.LoadConfigButton_Click);
            // 
            // logBox
            // 
            this.logBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.logBox.Location = new System.Drawing.Point(0, 469);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.Size = new System.Drawing.Size(1046, 149);
            this.logBox.TabIndex = 9;
            // 
            // MultiProfitSimForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 618);
            this.Controls.Add(this._textBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.logBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MultiProfitSimForm";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _simulateButton;
        private System.Windows.Forms.TextBox _textBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button loadConfigButton;
        private System.Windows.Forms.OpenFileDialog _openFileDialog;
        private System.Windows.Forms.DateTimePicker _fromDateTimePicker;
        private System.Windows.Forms.Label toDateLabel;
        private System.Windows.Forms.Label fromDateLabel;
        private System.Windows.Forms.DateTimePicker _toDateTimePicker;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Button _cancelButton;
    }
}

