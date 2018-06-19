using System;

namespace Sowalabs.Bison.ProfitSim.Events
{
    internal interface ISimEvent
    {
        DateTime SimTime { get; }
        void Simulate();

    }
}
