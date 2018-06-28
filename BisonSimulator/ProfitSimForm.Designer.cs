using Sowalabs.Bison.ProfitSim.Config;

namespace Sowalabs.Bison.ProfitSim
{
    partial class ProfitSimForm
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
            this.components = new System.ComponentModel.Container();
            this.simulateButton = new System.Windows.Forms.Button();
            this.numUsers = new System.Windows.Forms.TextBox();
            this.simConfigSource = new System.Windows.Forms.BindingSource(this.components);
            this.reservationPeriod = new System.Windows.Forms.TextBox();
            this.hedgingDelay = new System.Windows.Forms.TextBox();
            this.offerAcceptanceRate = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sells = new System.Windows.Forms.RadioButton();
            this.buys = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.orderSizes = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.simulatedTime = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buySpread = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.sellSpread = new System.Windows.Forms.TextBox();
            this.customerConfigSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.simConfigSource)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customerConfigSource)).BeginInit();
            this.SuspendLayout();
            // 
            // simulateButton
            // 
            this.simulateButton.Location = new System.Drawing.Point(511, 552);
            this.simulateButton.Margin = new System.Windows.Forms.Padding(2);
            this.simulateButton.Name = "simulateButton";
            this.simulateButton.Size = new System.Drawing.Size(66, 33);
            this.simulateButton.TabIndex = 6;
            this.simulateButton.Text = "Simulate!";
            this.simulateButton.UseVisualStyleBackColor = true;
            this.simulateButton.Click += new System.EventHandler(this.SimulateButton_Click);
            // 
            // numUsers
            // 
            this.numUsers.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerConfigSource, "NumCustomers", true));
            this.numUsers.Location = new System.Drawing.Point(408, 188);
            this.numUsers.Margin = new System.Windows.Forms.Padding(2);
            this.numUsers.Name = "numUsers";
            this.numUsers.Size = new System.Drawing.Size(52, 20);
            this.numUsers.TabIndex = 1;
            // 
            // simConfigSource
            // 
            this.simConfigSource.DataSource = typeof(SimulationScenario);
            // 
            // reservationPeriod
            // 
            this.reservationPeriod.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerConfigSource, "ReservationPeriod", true));
            this.reservationPeriod.Location = new System.Drawing.Point(408, 328);
            this.reservationPeriod.Margin = new System.Windows.Forms.Padding(2);
            this.reservationPeriod.Name = "reservationPeriod";
            this.reservationPeriod.Size = new System.Drawing.Size(52, 20);
            this.reservationPeriod.TabIndex = 2;
            // 
            // hedgingDelay
            // 
            this.hedgingDelay.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.simConfigSource, "HedgingDelay", true));
            this.hedgingDelay.Location = new System.Drawing.Point(408, 352);
            this.hedgingDelay.Margin = new System.Windows.Forms.Padding(2);
            this.hedgingDelay.Name = "hedgingDelay";
            this.hedgingDelay.Size = new System.Drawing.Size(52, 20);
            this.hedgingDelay.TabIndex = 3;
            // 
            // offerAcceptanceRate
            // 
            this.offerAcceptanceRate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerConfigSource, "OfferAcceptanceRate", true));
            this.offerAcceptanceRate.Location = new System.Drawing.Point(408, 236);
            this.offerAcceptanceRate.Margin = new System.Windows.Forms.Padding(2);
            this.offerAcceptanceRate.Name = "offerAcceptanceRate";
            this.offerAcceptanceRate.Size = new System.Drawing.Size(52, 20);
            this.offerAcceptanceRate.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.sells);
            this.panel1.Controls.Add(this.buys);
            this.panel1.Location = new System.Drawing.Point(297, 286);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(143, 38);
            this.panel1.TabIndex = 0;
            // 
            // sells
            // 
            this.sells.AutoSize = true;
            this.sells.Location = new System.Drawing.Point(79, 11);
            this.sells.Margin = new System.Windows.Forms.Padding(2);
            this.sells.Name = "sells";
            this.sells.Size = new System.Drawing.Size(47, 17);
            this.sells.TabIndex = 1;
            this.sells.TabStop = true;
            this.sells.Text = "Sells";
            this.sells.UseVisualStyleBackColor = true;
            // 
            // buys
            // 
            this.buys.AutoSize = true;
            this.buys.Location = new System.Drawing.Point(8, 11);
            this.buys.Margin = new System.Windows.Forms.Padding(2);
            this.buys.Name = "buys";
            this.buys.Size = new System.Drawing.Size(48, 17);
            this.buys.TabIndex = 0;
            this.buys.TabStop = true;
            this.buys.Text = "Buys";
            this.buys.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(294, 192);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Num. users";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(294, 331);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Reservation period (s)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(294, 355);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Hedging delay (s)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(294, 240);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Acceptance rate (%)";
            // 
            // orderSizes
            // 
            this.orderSizes.Location = new System.Drawing.Point(408, 260);
            this.orderSizes.Margin = new System.Windows.Forms.Padding(2);
            this.orderSizes.Name = "orderSizes";
            this.orderSizes.Size = new System.Drawing.Size(347, 20);
            this.orderSizes.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(294, 264);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Order sizes";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(294, 216);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Simulated time (s)";
            // 
            // simulatedTime
            // 
            this.simulatedTime.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customerConfigSource, "SimulatedTime", true));
            this.simulatedTime.Location = new System.Drawing.Point(408, 212);
            this.simulatedTime.Margin = new System.Windows.Forms.Padding(2);
            this.simulatedTime.Name = "simulatedTime";
            this.simulatedTime.Size = new System.Drawing.Size(52, 20);
            this.simulatedTime.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(294, 394);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Buy spread (%)";
            // 
            // buySpread
            // 
            this.buySpread.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.simConfigSource, "BuySpread", true));
            this.buySpread.Location = new System.Drawing.Point(408, 391);
            this.buySpread.Margin = new System.Windows.Forms.Padding(2);
            this.buySpread.Name = "buySpread";
            this.buySpread.Size = new System.Drawing.Size(52, 20);
            this.buySpread.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(294, 418);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Sell spread (%)";
            // 
            // sellSpread
            // 
            this.sellSpread.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.simConfigSource, "SellSpread", true));
            this.sellSpread.Location = new System.Drawing.Point(408, 415);
            this.sellSpread.Margin = new System.Windows.Forms.Padding(2);
            this.sellSpread.Name = "sellSpread";
            this.sellSpread.Size = new System.Drawing.Size(52, 20);
            this.sellSpread.TabIndex = 18;
            // 
            // customerConfigSource
            // 
            this.customerConfigSource.DataSource = typeof(Sowalabs.Bison.ProfitSim.CustomerGenerating.CustomerGeneratorConfig);
            // 
            // ProfitSimForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 618);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.sellSpread);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.buySpread);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.simulatedTime);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.orderSizes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.offerAcceptanceRate);
            this.Controls.Add(this.hedgingDelay);
            this.Controls.Add(this.reservationPeriod);
            this.Controls.Add(this.numUsers);
            this.Controls.Add(this.simulateButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ProfitSimForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.simConfigSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customerConfigSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button simulateButton;
        private System.Windows.Forms.BindingSource simConfigSource;
        private System.Windows.Forms.TextBox numUsers;
        private System.Windows.Forms.TextBox reservationPeriod;
        private System.Windows.Forms.TextBox hedgingDelay;
        private System.Windows.Forms.TextBox offerAcceptanceRate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton buys;
        private System.Windows.Forms.RadioButton sells;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox orderSizes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox simulatedTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox buySpread;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox sellSpread;
        private System.Windows.Forms.BindingSource customerConfigSource;
    }
}

