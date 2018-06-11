using Sowalabs.Bison.ProfitSim.Model;
using System.Windows.Forms;

namespace Sowalabs.Bison.ProfitSim
{
    internal partial class ProfitResultForm : Form
    {
        public ProfitResultForm()
        {
            InitializeComponent();
        }

        public ProfitResultForm(ProfitSimulationResult data)
        {
            InitializeComponent();

            data.ProfitDistribution.ForEach(profit => this.chart1.Series[0].Points.AddXY(profit.Value.ToString("N2"), profit.Count));
        }

    }
}
