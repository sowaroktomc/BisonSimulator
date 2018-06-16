using Sowalabs.Bison.Common;
using Sowalabs.Bison.ProfitSim.Dependencies;
using Sowalabs.Bison.ProfitSim.Events;
using Sowalabs.Bison.ProfitSim.IO;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp;
using Sowalabs.Bison.ProfitSim.Model;
using System;
using System.IO;
using Newtonsoft.Json;
using Sowalabs.Bison.Common.Trading;

namespace Sowalabs.Bison.ProfitSim
{
    internal class ProfitSimulator
    {
        private readonly SimulationDependencyFactory _dependencyFactory;
        private readonly SimulationEngine _engine;
        private ProfitSimulationConfig _config;
        private bool _isSimulationDone;
        private StreamWriter _outputFileWriter;
        private readonly JsonSerializer _serializer;

        public ProfitSimulationResult Result { get; set; }

        private BitstampHistoryLoader _historyLoader;
        private HistoryEnumerator _historyEnumerator;
        private MarketAnalyzer _marketAnalyzer;


        public ProfitSimulator()
        {
            _engine = new SimulationEngine();
            _engine.AfterEventSimulation += LoadOrderBookHistoryEventsIntoEngine;

            _dependencyFactory = new SimulationDependencyFactory(_engine);
            _serializer = new JsonSerializer();
            SimulationLiquidityEngineQueue.EnsureInstanceCreated();
        }

        public void InitializeSimulation(ProfitSimulationConfig config)
        {
            _config = config;
            _dependencyFactory.HedgingEngine.WhenStrategy.Delay = _config.HedgingDelay * 1000;
            _dependencyFactory.PricingEngine.PricingStrategy.SellSpread = _config.SellSpread / 100;
            _dependencyFactory.PricingEngine.PricingStrategy.BuySpread = _config.BuySpread / 100;


            // TODO Create JSON serializer class
            if (!string.IsNullOrEmpty(_config.OutputFilename))
            {
                _outputFileWriter = new StreamWriter(new FileStream(_config.OutputFilename, FileMode.Create));
                _outputFileWriter.Write('[');
            }
            else
            {
                _outputFileWriter = null;
            }

            _historyLoader = OrderBookHistoryLoaderFactory.Instance.GetLoader(config.Crypto);
            _historyEnumerator = new HistoryEnumerator(_historyLoader);
            _marketAnalyzer = MarketAnalyzerFactory.Instance.GetAnalyzer(config.Crypto);
            Result = new ProfitSimulationResult();
            _isSimulationDone = false;
        }

        private void LoadOrderBookHistoryEventsIntoEngine(object sender, EventArgs<ISimEvent> args)
        {
            var lastEvent = _engine.PeekLastEvent();
            while (lastEvent != null && !(lastEvent is OrderBookEvent))
            {
                var nextOrderBook = _historyEnumerator.GetNext();
                if (nextOrderBook == null)
                {
                    _isSimulationDone = true;
                    break;
                }

                _engine.AddEvent(new OrderBookEvent(nextOrderBook, _dependencyFactory.BitcoinMarketApi));

                lastEvent = _engine.PeekLastEvent();
            }
        }

        public bool ExecuteSimulationStep()
        {
            if (_isSimulationDone)
            {
                return false;
            }

            var firstOrderBook = _historyEnumerator.GetNext();

            if (firstOrderBook == null)
            {
                return false;
            }

            _engine.AddEvent(new OrderBookEvent(firstOrderBook, _dependencyFactory.BitcoinMarketApi));
            _config.Customers.ForEach(customer => customer.CreateEvents(_dependencyFactory, firstOrderBook.AcqTime).ForEach(simEvent => _engine.AddEvent(simEvent)));

            _engine.Execute();

            var profit = (_dependencyFactory.BitcoinMarketApi.MoneyBalance + _dependencyFactory.SolarisBank.Balance) / Math.Abs(_dependencyFactory.BitcoinMarketApi.MoneyBalance) * 100;
            var marketStats = _config.Customers[0].BuySell == BuySell.Buy ? _marketAnalyzer.AskStat : _marketAnalyzer.BidStat;
            var profitEntry = new ProfitSimulationResult.PLEntry(firstOrderBook.AcqTime, profit)
            {
                Volatility = marketStats.Volatility.Value,
                Trend = marketStats.Trend.Value,
                Extreme = marketStats.Extreme.Value,
                O = marketStats.OpenPrice.Value,
                H = marketStats.HighPrice.Value,
                L = marketStats.LowPrice.Value,
                C = marketStats.ClosePrice.Value,
                V = marketStats.VolatilityValue.Value
            };


            Result.Entries.Add(profitEntry);
            if (_outputFileWriter != null)
            {
                if (Result.Entries.Count > 1)
                {
                    _outputFileWriter.Write(',');
                }

                _serializer.Serialize(_outputFileWriter, profitEntry);
            }

            _dependencyFactory.BitcoinMarketApi.Reset();
            _dependencyFactory.SolarisBank.Reset();
            _isSimulationDone |= !_historyEnumerator.Restart();

            return !_isSimulationDone;
        }

        public void FinalizeSimulation()
        {
            if (_outputFileWriter != null)
            {
                _outputFileWriter.Write(']');
                _outputFileWriter.Close();
                _outputFileWriter = null;
            }
        }
    }
}
