using Sowalabs.Bison.Common.Environment;
using Sowalabs.Bison.Common.Trading;
using System;
using System.Linq;
using System.Windows.Forms;
using Sowalabs.Bison.ProfitSim.Config;
using Sowalabs.Bison.ProfitSim.CustomerGenerating;

namespace Sowalabs.Bison.ProfitSim
{
    public partial class ProfitSimForm : Form
    {
        private readonly SimulationScenario _simulationScenario = new SimulationScenario();
        private readonly CustomerGeneratorConfig _customerConfig = new CustomerGeneratorConfig();
        private readonly CustomerGenerator _customerGenerator = new CustomerGenerator();

        public ProfitSimForm()
        {
            InitializeComponent();

            simConfigSource.DataSource = _simulationScenario;
            customerConfigSource.DataSource = _customerConfig;

            buys.Checked = true;
            orderSizes.Text = "1";
            _simulationScenario.Crypto = Crypto.Btc;
        }

        private void SimulateButton_Click(object sender, EventArgs e)
        {
            _customerConfig.OrderSizes.Clear();
            _customerConfig.OrderSizes.AddRange(orderSizes.Text.Split(new [] {";"}, StringSplitOptions.RemoveEmptyEntries).Select(Convert.ToDecimal));
            _customerConfig.CreateBuysOrSells = buys.Checked ? BuySell.Buy : BuySell.Sell;

            _simulationScenario.Customers.Clear();
            _simulationScenario.Customers.AddRange(_customerGenerator.CreateCustomers(_customerConfig));

            new ProfitSimulation(new[] {_simulationScenario}).ExecuteSimulation(null);
        }
    }
}
