using System;

namespace Sowalabs.Bison.LiquidityEngine.WhenStrategies
{
    public interface IWhenStrategyConsumer
    {
        event EventHandler OnExecute;
        void Execute();
    }
}
