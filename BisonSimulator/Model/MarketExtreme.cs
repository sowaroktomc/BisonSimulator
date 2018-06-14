using System;

namespace Sowalabs.Bison.ProfitSim.Model
{
    [Flags]
    public enum MarketExtreme
    {
        None = 1,
        HighRise = 2,
        HighFall = 4
    }
}
