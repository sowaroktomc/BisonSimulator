using Newtonsoft.Json;
using Sowalabs.Bison.ProfitSim.Dependencies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Sowalabs.Bison.Common.Environment;
using Sowalabs.Bison.Common.Trading;
using Exception = System.Exception;

namespace Sowalabs.Bison.ProfitSim
{
    public partial class MultiProfitSimForm : Form
    {

        public MultiProfitSimForm()
        {
            InitializeComponent();
        }

        private void SimulateButton_Click(object sender, EventArgs e)
        {
            List<ProfitSimulationConfig> configs;

            try
            {
                configs = JsonConvert.DeserializeObject<List<ProfitSimulationConfig>>(textBox.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }

            var outputFolder = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
            Directory.CreateDirectory(outputFolder);
            using (var metaWriter = File.CreateText(Path.Combine(outputFolder, "Config.json")))
            {
                metaWriter.WriteLine(textBox.Text);
            }

            //configs = configs.SelectMany(config => Enum.GetValues(typeof(Crypto)).Cast<Crypto>().Select(crypto =>
            //{
            //    var clone = config.Clone();
            //    clone.Crypto = crypto;
            //    clone.OutputFilename = $"{Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(clone.OutputFilename) ?? string.Empty)}_{crypto}.json";
            //    return clone;
            //})).ToList();


            var analyzers = configs.Select(config => config.Crypto).Distinct().Select(crypto => MarketAnalyzerFactory.Instance.GetAnalyzer(crypto)).ToList();
            var simulations = configs.Select(config => new {Simulator = new ProfitSimulator(), Config = config}).ToList();

            OrderBookHistoryLoaderFactory.Instance.GetAllLoaders().ToList().ForEach(loader => loader.Restart());
            simulations.ForEach(simulation => simulation.Simulator.InitializeSimulation(simulation.Config));
            analyzers.ForEach(analyzer => analyzer.Init());



            var isSimRunning = true;
            while (isSimRunning)
            {
                isSimRunning = false;
                simulations.ForEach(simulation => isSimRunning |= simulation.Simulator.ExecuteSimulationStep());

                analyzers.ForEach(analyzer => analyzer.Next());
            }

            //simulations.ForEach(simulation => {var frm = new ProfitResultForm(simulation.Simulator.Result); frm.Show();});
            simulations.ForEach(simulation => simulation.Simulator.FinalizeSimulation());
        }

        private void loadConfigButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            using (var reader = new StreamReader(openFileDialog.FileName))
            {
                textBox.Text = reader.ReadToEnd();
            }
        }
    }
}
