using Sowalabs.Bison.ProfitSim.Config;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Sowalabs.Bison.ProfitSim
{
    /// <summary>
    /// Simulates a given scenarios and calculates profit / losses that would be made if scenarios happened on each second in history of captured market order books.
    /// </summary>
    internal class ProfitSimulation
    {

        private readonly List<SimulationScenario> _scenarios;
        private readonly List<BitstampHistoryLoader> _historyLoaders;

        /// <summary>
        /// Simulates a given scenarios and calculates profit / losses that would be made if scenarios happened on each second in history of captured market order books.
        /// Profit/losses are output to a file given in scenario description.
        /// </summary>
        public ProfitSimulation(IEnumerable<SimulationScenario> scenarios)
        {
            _scenarios = scenarios.ToList();
            _historyLoaders = _scenarios.GroupBy(scenario => scenario.Crypto)
                .Select(group => new BitstampHistoryLoader(group.Key, group.Min(scenario => scenario.FromDateTime), group.Max(scenario => scenario.ToDateTime))).ToList();
        }

        /// <summary>
        /// Executes simulation. Simulation can be canceled using given cancelation token.
        /// </summary>
        /// <param name="cancelationToken">Cancelation token with which the simulation can be canceled.</param>
        public void ExecuteSimulation(CancellationToken? cancelationToken)
        {
            Trace.WriteLine("Simulation started!");
            try
            {
                var simulators = _scenarios.Select(scenario => new ProfitSimulator(scenario, _historyLoaders.Find(loader => loader.Crypto == scenario.Crypto))).ToList();

                var threader = new ThreadedExecutor(simulators.Select(simulator => new Func<bool>(simulator.ExecuteSimulationStep)).ToList());


                try
                {
                    while (threader.ExecuteSteps())
                    {
                        cancelationToken?.ThrowIfCancellationRequested();
                    }
                }
                catch (OperationCanceledException)
                {
                    Trace.WriteLine("Simulation canceled!");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }
                finally
                {
                    simulators.ForEach(simulator => simulator.FinalizeSimulation());
                }

                Trace.WriteLine("Simulation done.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

    }
}
