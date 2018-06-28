using System;
using System.Globalization;
using System.IO;
using Sowalabs.Bison.ProfitSim.Entity;

namespace Sowalabs.Bison.ProfitSim.IO.Output
{
    /// <summary>
    /// Writes profit simulation results to CSV file.
    /// </summary>
    internal class CsvProfitSimulationWriter : IProfitSimulationWriter
    {
        private StreamWriter _outputFileWriter;
        private readonly NumberFormatInfo _numberFormat;
        private readonly string _filename;

        /// <summary>
        /// Writes profit simulation results to CSV file.
        /// </summary>
        /// <param name="filename">Filename of file to write results to.</param>
        public CsvProfitSimulationWriter(string filename)
        {
            _filename = filename;
            
            _numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            _numberFormat.NumberDecimalSeparator = ".";
            _numberFormat.NumberGroupSeparator = "";
        }

        /// <summary>
        /// Initializes writer.
        /// </summary>
        public void Initialize()
        {
            _outputFileWriter = new StreamWriter(new FileStream(_filename, FileMode.Create));
            _outputFileWriter.Write("DateTime, PL, Price");
        }


        /// <summary>
        /// Writes given entry to CSV file.
        /// </summary>
        /// <param name="entry">Entry to write.</param>
        public void Write(ProfitSimulationResult.PlEntry entry)
        {
            _outputFileWriter.WriteLine(string.Format(_numberFormat, "{0:dd.MM.yyyy HH:mm:ss}, {1}, {2}", entry.DateTime, entry.PL, entry.Price));
        }


        /// <summary>
        /// Closes writer.
        /// </summary>
        public void Close()
        {
            _outputFileWriter.Write(']');
            _outputFileWriter.Close();
            _outputFileWriter = null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _outputFileWriter?.Dispose();
            }
        }
    }
}
