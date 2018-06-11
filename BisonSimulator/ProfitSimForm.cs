using Sowalabs.Bison.Common.Trading;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Sowalabs.Bison.ProfitSim
{
    public partial class ProfitSimForm : Form
    {

        private readonly ProfitSimulator _simulator = new ProfitSimulator();

        public ProfitSimForm()
        {
            InitializeComponent();

            this.bindingSource.DataSource = _simulator;
            this.buys.Checked = true;
            this.orderSizes.Text = "1";
        }

        private void SimulateButton_Click(object sender, EventArgs e)
        {

            this._simulator.OrderSizes.Clear();
            this._simulator.OrderSizes.AddRange(this.orderSizes.Text.Split(new [] {";"}, StringSplitOptions.RemoveEmptyEntries).Select(Convert.ToDecimal));
            this._simulator.CreateBidOrAsks = this.buys.Checked ? BuySell.Buy : BuySell.Sell;

            var result = this._simulator.ExecuteProfitSimulation();
            var frm = new ProfitResultForm(result);
            frm.Closed += (o, args) => frm.Dispose();
            frm.Show();
        }
    }
}
