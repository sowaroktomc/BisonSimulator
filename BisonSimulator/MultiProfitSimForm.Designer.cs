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
            this.simulateButton = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toDateLabel = new System.Windows.Forms.Label();
            this.fromDateLabel = new System.Windows.Forms.Label();
            this.toDatePicker = new System.Windows.Forms.DateTimePicker();
            this.fromTimePicker = new System.Windows.Forms.DateTimePicker();
            this.loadConfigButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.logBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // simulateButton
            // 
            this.simulateButton.Location = new System.Drawing.Point(97, 47);
            this.simulateButton.Margin = new System.Windows.Forms.Padding(2);
            this.simulateButton.Name = "simulateButton";
            this.simulateButton.Size = new System.Drawing.Size(66, 33);
            this.simulateButton.TabIndex = 6;
            this.simulateButton.Text = "Simulate!";
            this.simulateButton.UseVisualStyleBackColor = true;
            this.simulateButton.Click += new System.EventHandler(this.SimulateButton_Click);
            // 
            // textBox
            // 
            this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox.Location = new System.Drawing.Point(0, 0);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(1046, 391);
            this.textBox.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cancelButton);
            this.panel1.Controls.Add(this.toDateLabel);
            this.panel1.Controls.Add(this.fromDateLabel);
            this.panel1.Controls.Add(this.toDatePicker);
            this.panel1.Controls.Add(this.fromTimePicker);
            this.panel1.Controls.Add(this.loadConfigButton);
            this.panel1.Controls.Add(this.simulateButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 391);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1046, 82);
            this.panel1.TabIndex = 8;
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
            this.fromDateLabel.Size = new System.Drawing.Size(86, 13);
            this.fromDateLabel.TabIndex = 10;
            this.fromDateLabel.Text = "OrderBooks from";
            // 
            // toDatePicker
            // 
            this.toDatePicker.CustomFormat = "dd.MM.yyyy HH";
            this.toDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.toDatePicker.Location = new System.Drawing.Point(266, 6);
            this.toDatePicker.MinDate = new System.DateTime(2018, 4, 3, 0, 0, 0, 0);
            this.toDatePicker.Name = "toDatePicker";
            this.toDatePicker.Size = new System.Drawing.Size(141, 20);
            this.toDatePicker.TabIndex = 9;
            // 
            // fromTimePicker
            // 
            this.fromTimePicker.CustomFormat = "dd.MM.yyyy HH";
            this.fromTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.fromTimePicker.Location = new System.Drawing.Point(97, 6);
            this.fromTimePicker.MinDate = new System.DateTime(2018, 4, 3, 0, 0, 0, 0);
            this.fromTimePicker.Name = "fromTimePicker";
            this.fromTimePicker.Size = new System.Drawing.Size(141, 20);
            this.fromTimePicker.TabIndex = 8;
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
            this.loadConfigButton.Click += new System.EventHandler(this.loadConfigButton_Click);
            // 
            // logBox
            // 
            this.logBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.logBox.Location = new System.Drawing.Point(0, 473);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.Size = new System.Drawing.Size(1046, 145);
            this.logBox.TabIndex = 9;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(167, 47);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(66, 33);
            this.cancelButton.TabIndex = 12;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // MultiProfitSimForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 618);
            this.Controls.Add(this.textBox);
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

        private System.Windows.Forms.Button simulateButton;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button loadConfigButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.DateTimePicker fromTimePicker;
        private System.Windows.Forms.Label toDateLabel;
        private System.Windows.Forms.Label fromDateLabel;
        private System.Windows.Forms.DateTimePicker toDatePicker;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Button cancelButton;
    }
}

