namespace Sowalabs.Bison.ProfitSim
{
    partial class ProfitResultForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel = new System.Windows.Forms.Panel();
            this.extremeLabel = new System.Windows.Forms.Label();
            this._extremeBox = new System.Windows.Forms.CheckedListBox();
            this.trendLabel = new System.Windows.Forms.Label();
            this._trendBox = new System.Windows.Forms.CheckedListBox();
            this.volatilityLabel = new System.Windows.Forms.Label();
            this._volatilityBox = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.IsStartedFromZero = false;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.MaximumAutoSize = 90F;
            chartArea1.AxisX.ScaleView.Zoomable = false;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.IsValueShownAsLabel = true;
            series1.Name = "Series1";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(655, 450);
            this.chart1.TabIndex = 15;
            this.chart1.Text = "chart";
            // 
            // panel
            // 
            this.panel.Controls.Add(this.extremeLabel);
            this.panel.Controls.Add(this._extremeBox);
            this.panel.Controls.Add(this.trendLabel);
            this.panel.Controls.Add(this._trendBox);
            this.panel.Controls.Add(this.volatilityLabel);
            this.panel.Controls.Add(this._volatilityBox);
            this.panel.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel.Location = new System.Drawing.Point(655, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(145, 450);
            this.panel.TabIndex = 16;
            // 
            // extremeLabel
            // 
            this.extremeLabel.AutoSize = true;
            this.extremeLabel.Location = new System.Drawing.Point(6, 267);
            this.extremeLabel.Name = "extremeLabel";
            this.extremeLabel.Size = new System.Drawing.Size(45, 13);
            this.extremeLabel.TabIndex = 4;
            this.extremeLabel.Text = "Extreme";
            // 
            // _extremeBox
            // 
            this._extremeBox.CheckOnClick = true;
            this._extremeBox.FormattingEnabled = true;
            this._extremeBox.Location = new System.Drawing.Point(6, 283);
            this._extremeBox.Name = "_extremeBox";
            this._extremeBox.Size = new System.Drawing.Size(120, 94);
            this._extremeBox.TabIndex = 5;
            this._extremeBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ExtremeBox_ItemCheck);
            // 
            // trendLabel
            // 
            this.trendLabel.AutoSize = true;
            this.trendLabel.Location = new System.Drawing.Point(6, 151);
            this.trendLabel.Name = "trendLabel";
            this.trendLabel.Size = new System.Drawing.Size(35, 13);
            this.trendLabel.TabIndex = 2;
            this.trendLabel.Text = "Trend";
            // 
            // _trendBox
            // 
            this._trendBox.CheckOnClick = true;
            this._trendBox.FormattingEnabled = true;
            this._trendBox.Location = new System.Drawing.Point(6, 167);
            this._trendBox.Name = "_trendBox";
            this._trendBox.Size = new System.Drawing.Size(120, 94);
            this._trendBox.TabIndex = 3;
            this._trendBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.TrendBox_ItemCheck);
            // 
            // volatilityLabel
            // 
            this.volatilityLabel.AutoSize = true;
            this.volatilityLabel.Location = new System.Drawing.Point(6, 33);
            this.volatilityLabel.Name = "volatilityLabel";
            this.volatilityLabel.Size = new System.Drawing.Size(45, 13);
            this.volatilityLabel.TabIndex = 0;
            this.volatilityLabel.Text = "Volatility";
            // 
            // _volatilityBox
            // 
            this._volatilityBox.CheckOnClick = true;
            this._volatilityBox.FormattingEnabled = true;
            this._volatilityBox.Location = new System.Drawing.Point(6, 49);
            this._volatilityBox.Name = "_volatilityBox";
            this._volatilityBox.Size = new System.Drawing.Size(120, 94);
            this._volatilityBox.TabIndex = 1;
            this._volatilityBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.VolatilityBox_ItemCheck);
            // 
            // ProfitResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.panel);
            this.Name = "ProfitResultForm";
            this.Text = "ProfitResultForm";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label extremeLabel;
        private System.Windows.Forms.CheckedListBox _extremeBox;
        private System.Windows.Forms.Label trendLabel;
        private System.Windows.Forms.CheckedListBox _trendBox;
        private System.Windows.Forms.Label volatilityLabel;
        private System.Windows.Forms.CheckedListBox _volatilityBox;
    }
}