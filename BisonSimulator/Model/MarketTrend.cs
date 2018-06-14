using System;

namespace Sowalabs.Bison.ProfitSim.Model
{
    [Flags]
    public enum MarketTrend
    {
        NoChange = 1,
        Up = 2,
        Down = 4
    }
}
