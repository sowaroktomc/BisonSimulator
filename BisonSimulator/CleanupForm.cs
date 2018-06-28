using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Sowalabs.Bison.ProfitSim.Entity;

namespace Sowalabs.Bison.ProfitSim
{
    public partial class CleanupForm : Form
    {
        public CleanupForm()
        {
            InitializeComponent();
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            var buffer = new char[1000 * 1024];
            var copyBuffer = new char[1000 * 1024];

            if (folderBrowserDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            foreach (var fromFilename in Directory.EnumerateFiles(folderBrowserDialog.SelectedPath).Where(filename => !filename.Contains("Config.json") && Path.GetExtension(filename) == ".json"))
            {
                var toDirectory = $"{Path.GetDirectoryName(fromFilename)}\\clean";
                var toFilename = $"{toDirectory}\\{Path.GetFileName(fromFilename)}";
                if (!Directory.Exists(toDirectory))
                {
                    Directory.CreateDirectory(toDirectory);
                }
                using (var writer = new StreamWriter(new FileStream(toFilename, FileMode.Create)))
                {
                    using (var reader = new StreamReader(new FileStream(fromFilename, FileMode.Open)))
                    {
                        var index = 0;
                        int read;
                        do
                        {
                            read = reader.Read(buffer, index, buffer.Length - index);
                            int i;
                            for (i = read + index - 1; i >= 0; i--)
                            {
                                if (buffer[i] == '}')
                                {
                                    break;
                                }
                            }

                            if (i == 0 && buffer[i] != '}')
                            {
                                return;
                            }

                            writer.Write(buffer, 0, i + 1);
                            //writer.Write(Environment.NewLine);
                            if (i < read + index - 1)
                            {
                                index = read + index - i - 1;
                                Array.Copy(buffer, i + 1, copyBuffer, 0, index);
                                var temp = copyBuffer;
                                copyBuffer = buffer;
                                buffer = temp;
                            }
                            else
                            {
                                index = 0;
                            }

                        } while (read > 0);

                        writer.Write(']');
                    }
                }
            }

        }

        private void ConvertToCsvButton_Click(object sender, EventArgs e)
        {

            if (folderBrowserDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var formatprovider = (NumberFormatInfo) CultureInfo.CurrentCulture.NumberFormat.Clone();
            formatprovider.NumberGroupSeparator = "";
            formatprovider.NumberDecimalSeparator = ".";
            
            foreach (var fromFilename in Directory.EnumerateFiles(folderBrowserDialog.SelectedPath).Where(filename => !filename.Contains("Config.json") && Path.GetExtension(filename) == ".json"))
            {
                var toDirectory = $"{Path.GetDirectoryName(fromFilename)}\\csv";
                var toFilename = $"{toDirectory}\\{Path.GetFileNameWithoutExtension(fromFilename)}.csv";
                if (!Directory.Exists(toDirectory))
                {
                    Directory.CreateDirectory(toDirectory);
                }
                using (var writer = new StreamWriter(new FileStream(toFilename, FileMode.Create)))
                {
                    writer.WriteLine("DateTime, Price, PL");

                    using (var reader = new JsonTextReader(new StreamReader(fromFilename)))
                    {
                        var serializer = new JsonSerializer();
                        var list = serializer.Deserialize<List<ProfitSimulationResult.PlEntry>>(reader);


                        list.ForEach(entry => writer.WriteLine($"{entry.DateTime:yyyy.MM.dd HH:mm:ss}, {entry.Price.ToString(formatprovider)}, {entry.PL.ToString(formatprovider)}"));
                    }
                }
            }
        }
    }
}
