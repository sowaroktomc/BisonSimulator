using Sowalabs.Bison.Common.Environment;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    internal class OrderBookHistoryLoaderFactory
    {
        private static readonly Lazy<OrderBookHistoryLoaderFactory> Lazy = new Lazy<OrderBookHistoryLoaderFactory>(() => new OrderBookHistoryLoaderFactory());

        public static OrderBookHistoryLoaderFactory Instance => Lazy.Value;

        private readonly Dictionary<Crypto, BitstampHistoryLoader> _loaders;

        private OrderBookHistoryLoaderFactory()
        {
            _loaders = Enum.GetValues(typeof(Crypto)).Cast<Crypto>().Select(crypto => new BitstampHistoryLoader(crypto)).ToDictionary(loader => loader.Crypto);
        }

        public BitstampHistoryLoader GetLoader(Crypto crypto)
        {
            return _loaders[crypto];
        }

        public IReadOnlyList<BitstampHistoryLoader> GetAllLoaders()
        {
            return _loaders.Values.ToList();
        }
    }
}
