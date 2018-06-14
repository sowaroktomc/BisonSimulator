using System;
using System.Collections.Generic;
using Sowalabs.Bison.ProfitSim.Model;
using System.Windows.Forms;
using System.Linq;

namespace Sowalabs.Bison.ProfitSim
{
    internal partial class ProfitResultForm : Form
    {

        private class CheckableEnum<T> where T : struct
        {
            public class Item
            {
                public Item(T value)
                {
                    Value = value;
                    Name = Enum.GetName(typeof(T), value);
                    Checked = false;
                }

                // ReSharper fails to notice the property is used to databind to.
                // ReSharper disable once MemberCanBePrivate.Local
                // ReSharper disable once UnusedAutoPropertyAccessor.Local
                public string Name { get; }
                public bool Checked { get; set; }
                public T Value { get; }
            }

            public List<Item> Items { get; }

            public CheckableEnum()
            {
                Items = new List<Item>(Enum.GetValues(typeof(T)).Cast<T>().Select(value => new Item(value)));
            }
        }

        private readonly ProfitSimulationResult _data;
        private CheckableEnum<MarketVolatility> _volatilities;
        private CheckableEnum<MarketTrend> _trends;
        private CheckableEnum<MarketExtreme> _extremes;

        public ProfitResultForm()
        {
            Init();
        }

        public ProfitResultForm(ProfitSimulationResult data)
        {
            Init();
            _data = data;
            ShowData();
        }

        private void ShowData()
        {
            if (_data == null)
            {
                return;
            }

            var volatility = _volatilities.Items.Where(item => item.Checked).Select(item => item.Value).Cast<MarketVolatility?>().DefaultIfEmpty(null).Aggregate((a, b) => a | b);
            var trend = _trends.Items.Where(item => item.Checked).Select(item => item.Value).Cast<MarketTrend?>().DefaultIfEmpty(null).Aggregate((a, b) => a | b);
            var extreme = _extremes.Items.Where(item => item.Checked).Select(item => item.Value).Cast<MarketExtreme?>().DefaultIfEmpty(null).Aggregate((a, b) => a | b);


            this.chart1.Series[0].Points.Clear();
            _data.GetProfitDistribution(volatility, trend, extreme).ForEach(profit => this.chart1.Series[0].Points.AddXY(profit.Value.ToString("N2") + "%", profit.Count));
        }

        private void Init()
        {
            InitializeComponent();
            this.Closed += (o, args) => this.Dispose();

            _volatilities = new CheckableEnum<MarketVolatility>();
            _trends = new CheckableEnum<MarketTrend>();
            _extremes = new CheckableEnum<MarketExtreme>();

            _volatilityBox.DataSource = _volatilities.Items;
            _volatilityBox.DisplayMember = "Name";

            _trendBox.DataSource = _trends.Items;
            _trendBox.DisplayMember = "Name";

            _extremeBox.DataSource = _extremes.Items;
            _extremeBox.DisplayMember = "Name";
        }

        private void VolatilityBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ((CheckableEnum<MarketVolatility>.Item) _volatilityBox.Items[e.Index]).Checked = e.NewValue == CheckState.Checked;
            ShowData();
        }
        private void TrendBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ((CheckableEnum<MarketTrend>.Item) _trendBox.Items[e.Index]).Checked = e.NewValue == CheckState.Checked;
            ShowData();
        }
        private void ExtremeBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ((CheckableEnum<MarketExtreme>.Item) _extremeBox.Items[e.Index]).Checked = e.NewValue == CheckState.Checked;
            ShowData();
        }
    }
}
