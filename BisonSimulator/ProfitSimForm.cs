using Sowalabs.Bison.Common.Environment;
using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.Dependencies;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Sowalabs.Bison.ProfitSim
{
    public partial class ProfitSimForm : Form
    {

        private readonly ProfitSimulationConfig _simulationConfig = new ProfitSimulationConfig();
        private readonly CustomerGenerator.Config _customerConfig = new CustomerGenerator.Config();
        private readonly CustomerGenerator _customerGenerator = new CustomerGenerator();

        public ProfitSimForm()
        {
            InitializeComponent();

            simConfigSource.DataSource = _simulationConfig;
            customerConfigSource.DataSource = _customerConfig;

            buys.Checked = true;
            orderSizes.Text = "1";
            _simulationConfig.Crypto = Crypto.Btc;
        }

        private void SimulateButton_Click(object sender, EventArgs e)
        {
            _customerConfig.OrderSizes.Clear();
            _customerConfig.OrderSizes.AddRange(orderSizes.Text.Split(new [] {";"}, StringSplitOptions.RemoveEmptyEntries).Select(Convert.ToDecimal));
            _customerConfig.CreateBidOrAsks = buys.Checked ? BuySell.Buy : BuySell.Sell;

            _simulationConfig.Customers.Clear();
            _simulationConfig.Customers.AddRange(_customerGenerator.CreateCustomers(_customerConfig));

            var analyzer = MarketAnalyzerFactory.Instance.GetAnalyzer(_simulationConfig.Crypto);
            var simulator = new ProfitSimulator();

            OrderBookHistoryLoaderFactory.Instance.GetAllLoaders().ToList().ForEach(loader => loader.Restart());
            simulator.InitializeSimulation(_simulationConfig);
            analyzer.Init();



            var isSimRunning = true;
            while (isSimRunning)
            {
                isSimRunning = false;
                isSimRunning |= simulator.ExecuteSimulationStep();

                analyzer.Next();
            }

            var frm = new ProfitResultForm(simulator.Result);
            frm.Show();
            simulator.FinalizeSimulation();
        }
    }
}
