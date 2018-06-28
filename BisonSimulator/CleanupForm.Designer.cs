namespace Sowalabs.Bison.ProfitSim
{
    partial class CleanupForm
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
            this.executeButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.convertToCsvButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // executeButton
            // 
            this.executeButton.Location = new System.Drawing.Point(12, 12);
            this.executeButton.Name = "executeButton";
            this.executeButton.Size = new System.Drawing.Size(75, 23);
            this.executeButton.TabIndex = 0;
            this.executeButton.Text = "Clean Up";
            this.executeButton.UseVisualStyleBackColor = true;
            this.executeButton.Click += new System.EventHandler(this.ExecuteButton_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog.ShowNewFolderButton = false;
            // 
            // convertToCsvButton
            // 
            this.convertToCsvButton.Location = new System.Drawing.Point(93, 12);
            this.convertToCsvButton.Name = "convertToCsvButton";
            this.convertToCsvButton.Size = new System.Drawing.Size(75, 23);
            this.convertToCsvButton.TabIndex = 1;
            this.convertToCsvButton.Text = "json to csv";
            this.convertToCsvButton.UseVisualStyleBackColor = true;
            this.convertToCsvButton.Click += new System.EventHandler(this.ConvertToCsvButton_Click);
            // 
            // CleanupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(183, 43);
            this.Controls.Add(this.convertToCsvButton);
            this.Controls.Add(this.executeButton);
            this.Name = "CleanupForm";
            this.Text = "CleanupForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button convertToCsvButton;
    }
}