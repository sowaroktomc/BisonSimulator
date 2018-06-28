using Sowalabs.Bison.Common;
using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.Config;
using Sowalabs.Bison.ProfitSim.Dependencies;
using Sowalabs.Bison.ProfitSim.Entity;
using Sowalabs.Bison.ProfitSim.Events;
using Sowalabs.Bison.ProfitSim.IO;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp;
using Sowalabs.Bison.ProfitSim.IO.Output;
using Sowalabs.Bison.ProfitSim.SimulationMoqs;
using System;

namespace Sowalabs.Bison.ProfitSim
{
    /// <summary>
    /// Simulates a given scenario and calculates profit / loss that would be made if scenario happened on each second in history of captured market order books.
    /// </summary>
    internal class ProfitSimulator
    {
        private readonly SimulationDependencyFactory _dependencyFactory;
        private readonly SimulationEngine _engine;
        private readonly SimulationScenario _scenario;
        private readonly IProfitSimulationWriter _resultWriter;
        private readonly HistoryEnumerator _historyEnumerator;
        private bool _isSimulationDone;

        /// <summary>
        /// Simulates a given scenario and calculates profit / loss that would be made if scenario happened on each second in history of captured market order books.
        /// Profit/losses are output to a file given in scenario description.
        /// </summary>
        /// <param name="scenario">Scenario to simulate.</param>
        /// <param name="historyLoader">Loader to be used to access history of market order books.</param>
        public ProfitSimulator(SimulationScenario scenario, BitstampHistoryLoader historyLoader)
        {
            _engine = new SimulationEngine();
            _engine.AfterEventSimulation += LoadOrderBookHistoryEventsIntoEngine;

            _dependencyFactory = new SimulationDependencyFactory(_engine);
            LiquidityEngineQueueMoq.EnsureInstanceCreated();

            _scenario = scenario;
            _dependencyFactory.HedgingEngine.WhenStrategy.Delay = _scenario.HedgingDelay * 1000;
            _dependencyFactory.PricingEngine.PricingStrategy.SellSpread = _scenario.SellSpread / 100;
            _dependencyFactory.PricingEngine.PricingStrategy.BuySpread = _scenario.BuySpread / 100;

            _resultWriter = new CsvProfitSimulationWriter(_scenario.OutputFilename);
            _resultWriter.Initialize();

            _historyEnumerator = new HistoryEnumerator(historyLoader);
            _isSimulationDone = false;
        }

        private void LoadOrderBookHistoryEventsIntoEngine(object sender, EventArgs<ISimEvent> args)
        {
            if (_isSimulationDone)
            {
                return;
            }

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

        /// <summary>
        /// Executes a simulation step -> calculates profit/loss value for next one time slot in simulated time.
        /// </summary>
        /// <returns>True if more simlated timeslots are available - more profits are possible to calclate. False when simulation is done (simulated time period is exhausted).</returns>
        public bool ExecuteSimulationStep()
        {
            if (_isSimulationDone)
            {
                return false;
            }

            var firstOrderBook = _historyEnumerator.GetNext();

            if (firstOrderBook == null || firstOrderBook.AcqTime > _scenario.ToDateTime)
            {
                _isSimulationDone = true;
                _historyEnumerator.Close(); // Close enumerator to avoid hogging orderbooks from other simulators.
                return false;
            }

            if (firstOrderBook.AcqTime >= _scenario.FromDateTime)
            {

                _engine.AddEvent(new OrderBookEvent(firstOrderBook, _dependencyFactory.BitcoinMarketApi));
                _scenario.Customers.ForEach(customer => customer.CreateEvents(_dependencyFactory, firstOrderBook.AcqTime).ForEach(simEvent => _engine.AddEvent(simEvent)));

                _engine.Simulate();


                var profit = (_dependencyFactory.BitcoinMarketApi.MoneyBalance + _dependencyFactory.SolarisBank.Balance) / Math.Abs(_dependencyFactory.BitcoinMarketApi.MoneyBalance) * 100;
                var price = _scenario.Customers[0].BuySell == BuySell.Buy ? firstOrderBook.Asks[0].Price : firstOrderBook.Bids[0].Price;
                _resultWriter.Write(new ProfitSimulationResult.PlEntry(firstOrderBook.AcqTime, profit) {Price = price});


                _dependencyFactory.BitcoinMarketApi.Reset();
                _dependencyFactory.SolarisBank.Reset();
            }

            _isSimulationDone |= !_historyEnumerator.Restart();

            return !_isSimulationDone;
        }

        /// <summary>
        /// Finalizes simulation - closes open file handles and such.
        /// </summary>
        public void FinalizeSimulation()
        {
            _resultWriter.Close();
            _historyEnumerator.Close();
        }
    }
}
