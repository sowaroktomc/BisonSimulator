using Sowalabs.Bison.Common.Environment;
using Sowalabs.Bison.ProfitSim.Dependencies;
using Sowalabs.Bison.ProfitSim.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sowalabs.Bison.ProfitSim
{
    internal class MarketAnalyzerFactory
    {
        private static readonly Lazy<MarketAnalyzerFactory> Lazy = new Lazy<MarketAnalyzerFactory>(() => new MarketAnalyzerFactory());

        public static MarketAnalyzerFactory Instance => Lazy.Value;

        private readonly Dictionary<Crypto, MarketAnalyzer> _analyzers;

        private MarketAnalyzerFactory()
        {
            _analyzers = new Dictionary<Crypto, MarketAnalyzer>();
            foreach (Crypto crypto in Enum.GetValues(typeof(Crypto)))
            {
                _analyzers.Add(crypto, new MarketAnalyzer(OrderBookHistoryLoaderFactory.Instance.GetLoader(crypto)));
            }
        }

        public MarketAnalyzer GetAnalyzer(Crypto crypto)
        {
            return _analyzers[crypto];
        }

        public IReadOnlyCollection<MarketAnalyzer> GetAllAnalyzers()
        {
            return _analyzers.Values.ToList();
        }
    }
}
