using System;

namespace Sowalabs.Bison.LiquidityEngine.WhenStrategies
{
    public interface IWhenStrategy : IDisposable
    {
        void RegisterNewEntry();
    }
}
