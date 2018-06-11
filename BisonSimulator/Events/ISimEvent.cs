using System;

namespace Sowalabs.Bison.ProfitSim.Events
{
    internal interface ISimEvent
    {
        Guid Id { get; }
        DateTime SimTime { get; }
        void Simulate();

    }
}
