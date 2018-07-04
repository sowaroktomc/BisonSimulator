using Sowalabs.Bison.ProfitSim.Dependencies;
using System;

namespace Sowalabs.Bison.ProfitSim.Events
{
    internal class IntraBankTransferStatusEvent : ISimEvent
    {
        private readonly SimulatedBankApi _bankApi;
        private readonly string _bankTransactionId;

        public IntraBankTransferStatusEvent(SimulationEngine engine, SimulatedBankApi bankApi, string bankTransactionId, int delay)
        {
            _bankApi = bankApi;
            _bankTransactionId = bankTransactionId;
            SimTime = engine.CurrentTime.AddSeconds(delay);
        }

        public DateTime SimTime { get; }

        public void Simulate()
        {
            _bankApi.ExecuteTransaction(_bankTransactionId);
        }
    }
}
