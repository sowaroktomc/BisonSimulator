using Newtonsoft.Json;
using Sowalabs.Bison.ProfitSim.Dependencies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sowalabs.Bison.Common.Environment;
using Exception = System.Exception;

namespace Sowalabs.Bison.ProfitSim
{
    public partial class MultiProfitSimForm : Form
    {

        private class TextBoxTraceListener : TraceListener
        {
            private readonly TextBox _textbox;

            public TextBoxTraceListener(TextBox textbox)
            {
                _textbox = textbox;
            }

            public override void Write(string message)
            {
                if (string.IsNullOrEmpty(message))
                {
                    return;
                }

                if (_textbox.InvokeRequired)
                {
                    _textbox.Invoke(new Action(() => OutputMessage(message)));
                }
                else
                {
                    OutputMessage(message);
                }
            }

            private void OutputMessage(string message)
            {
                _textbox.AppendText($"[{DateTime.Now}] {message}");
            }

            public override void WriteLine(string message)
            {
                Write($"{message}{Environment.NewLine}");
            }
        }

        public MultiProfitSimForm()
        {
            InitializeComponent();

            fromTimePicker.Value = new DateTime(2018, 4, 4);
            toDatePicker.Value = DateTime.Today.AddHours(-1);


            Trace.Listeners.Add(new TextBoxTraceListener(logBox));
        }

        private void SimulateButton_Click(object sender, EventArgs e)
        {
            _cancellationSource?.Dispose();
            _cancellationSource = new CancellationTokenSource();
        

            Task.Run(() =>
            {
                simulateButton.Invoke(new Action(() => simulateButton.Enabled = false));
                simulateButton.Invoke(new Action(() => cancelButton.Enabled = true));
                Trace.WriteLine("Simulation started!");
                try
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

                    configs.ForEach(config => config.OutputFilename = $"{Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(config.OutputFilename) ?? string.Empty)}.json");

                    configs = configs.SelectMany(config => Enum.GetValues(typeof(Crypto)).Cast<Crypto>().Select(crypto =>
                    {
                        var clone = config.Clone();
                        clone.Crypto = crypto;
                        clone.OutputFilename = $"{Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(clone.OutputFilename) ?? string.Empty)}_{crypto}.json";
                        return clone;
                    })).ToList();


                    var analyzers = configs.Select(config => config.Crypto).Distinct().Select(crypto => MarketAnalyzerFactory.Instance.GetAnalyzer(crypto)).ToList();
                    var simulations = configs.Select(config => new {Simulator = new ProfitSimulator(), Config = config}).ToList();

                    OrderBookHistoryLoaderFactory.Instance.GetAllLoaders().ToList().ForEach(loader => loader.Restart());
                    simulations.ForEach(simulation => simulation.Simulator.InitializeSimulation(simulation.Config));
                    analyzers.ForEach(analyzer => analyzer.Init());
                    var threader = new ThreadedExecutor(simulations.Select(simulation => new Func<bool>(simulation.Simulator.ExecuteSimulationStep)).ToList());


                    while (threader.ExecuteStep())
                    {
                        analyzers.ForEach(analyzer => analyzer.Next());
                        _cancellationSource.Token.ThrowIfCancellationRequested();
                    }


                    //var isSimRunning = true;
                    //while (isSimRunning)
                    //{
                    //    isSimRunning = false;
                    //    simulations.ForEach(simulation => isSimRunning |= simulation.Simulator.ExecuteSimulationStep());

                    ////    analyzers.ForEach(analyzer => analyzer.Next());
                    //}

                    //simulations.ForEach(simulation => {var frm = new ProfitResultForm(simulation.Simulator.Result); frm.Show();});
                    simulations.ForEach(simulation => simulation.Simulator.FinalizeSimulation());
                    Trace.WriteLine("Simulation done.");
                }
                catch (OperationCanceledException)
                {
                    Trace.WriteLine("Simulation canceled!");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }).ContinueWith(task =>
            {
                simulateButton.Invoke(new Action(() => simulateButton.Enabled = true));
                simulateButton.Invoke(new Action(() => cancelButton.Enabled = false));
            });
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

        private CancellationTokenSource _cancellationSource;

        private void cancelButton_Click(object sender, EventArgs e)
        {
            _cancellationSource.Cancel();
        }
    }
}
