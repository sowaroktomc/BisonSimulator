using Sowalabs.Bison.Common.DateTimeProvider;
using System;

namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    internal class SimulatedDateTimeProvider : IDateTimeProvider
    {
        private readonly SimulationEngine _engine;

        public SimulatedDateTimeProvider(SimulationEngine engine)
        {
            _engine = engine;
        }

        public DateTime Now => _engine.CurrentTime;
    }
}
