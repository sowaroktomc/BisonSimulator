using Newtonsoft.Json;
using Sowalabs.Bison.Common.Environment;
using Sowalabs.Bison.ProfitSim.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Exception = System.Exception;

namespace Sowalabs.Bison.ProfitSim
{
    public partial class MultiProfitSimForm : Form
    {
        private CancellationTokenSource _cancellationSource;


        public MultiProfitSimForm()
        {
            InitializeComponent();

            _fromDateTimePicker.Value = new DateTime(2018, 4, 4);
            _toDateTimePicker.Value = DateTime.Today.AddHours(-1);
            
            Trace.Listeners.Add(new TextBoxTraceListener(logBox));
        }

        private void SimulateButton_Click(object sender, EventArgs e)
        {
            _cancellationSource?.Dispose();
            _cancellationSource = new CancellationTokenSource();

            _simulateButton.Enabled = false;
            _cancelButton.Enabled = true;

            Task.Run(() => ExecuteSimulation()).ContinueWith(task =>
            {
                _simulateButton.Invoke(new Action(() => _simulateButton.Enabled = true));
                _simulateButton.Invoke(new Action(() => _cancelButton.Enabled = false));
            });
        }

        private void ExecuteSimulation()
        {
            try
            {
                SimulationConfig config;

                try
                {
                    config = JsonConvert.DeserializeObject<SimulationConfig>(_textBox.Text);
                }
                catch (Exception exception)
                {
                    Trace.WriteLine($"{exception.Message} {exception.StackTrace}");
                    return;
                }

                var outputFolder = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
                Directory.CreateDirectory(outputFolder);
                using (var metaWriter = File.CreateText(Path.Combine(outputFolder, "Config.json")))
                {
                    metaWriter.WriteLine(_textBox.Text);
                }

                config.Scenarios.ForEach(scenario => scenario.OutputFilename = $"{Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(scenario.OutputFilename) ?? string.Empty)}.csv");
                config.Scenarios.ForEach(scenario => scenario.FromDateTime = config.FromDateTime ?? _fromDateTimePicker.Value);
                config.Scenarios.ForEach(scenario => scenario.ToDateTime = config.ToDateTime ?? _toDateTimePicker.Value);

                if (config.Crypto.HasValue)
                {
                    config.Scenarios.ForEach(scenario => scenario.Crypto = config.Crypto.Value);
                }
                else
                {
                    config.Scenarios = config.Scenarios.SelectMany(scenario => Enum.GetValues(typeof(Crypto)).Cast<Crypto>().Select(crypto =>
                    {
                        var clone = scenario.Clone();
                        clone.Crypto = crypto;
                        clone.OutputFilename = $"{Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(clone.OutputFilename) ?? string.Empty)}_{crypto}.csv";
                        return clone;
                    })).ToList();
                }

                new ProfitSimulation(config.Scenarios).ExecuteSimulation(_cancellationSource.Token);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void LoadConfigButton_Click(object sender, EventArgs e)
        {

            if (_openFileDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            using (var reader = new StreamReader(_openFileDialog.FileName))
            {
                _textBox.Text = reader.ReadToEnd();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            _cancellationSource?.Cancel();
        }
    }
}
