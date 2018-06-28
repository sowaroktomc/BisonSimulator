using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Sowalabs.Bison.ProfitSim
{
    /// <summary>
    /// TraceListener that outputs messages to a TextBox.
    /// </summary>
    internal class TextBoxTraceListener : TraceListener
    {
        private readonly TextBox _textbox;

        /// <summary>
        /// TraceListener that outputs messages to a TextBox.
        /// </summary>
        /// <param name="textbox">Textbox to write messasges to.</param>
        public TextBoxTraceListener(TextBox textbox)
        {
            _textbox = textbox;
        }

        /// <summary>Writes the specified message to the textbox. On UI thread.</summary>
        /// <param name="message">A message to write. </param>
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

        /// <summary>Writes the specified message, followed by a line terminator, to the textbox. On UI thread.</summary>
        /// <param name="message">A message to write. </param>
        public override void WriteLine(string message)
        {
            Write($"{message}{Environment.NewLine}");
        }

        /// <summary>
        /// Writes given message to textbox.
        /// </summary>
        /// <param name="message">A message to write. </param>
        private void OutputMessage(string message)
        {
            _textbox.AppendText($"[{DateTime.Now}] {message}");
        }
    }
}
