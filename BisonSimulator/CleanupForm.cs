using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sowalabs.Bison.ProfitSim
{
    public partial class CleanupForm : Form
    {
        public CleanupForm()
        {
            InitializeComponent();
        }

        private void executeButton_Click(object sender, EventArgs e)
        {
            var buffer = new char[1000 * 1024];
            var copyBuffer = new char[1000 * 1024];


            foreach (var fromFilename in Directory.EnumerateFiles(@"C:\Sowalabs\results").Where(filename => !filename.Contains("Config.json")))
            {
                var toFilename = $"{Path.GetDirectoryName(fromFilename)}\\clean\\{Path.GetFileName(fromFilename)}";
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
    }
}
