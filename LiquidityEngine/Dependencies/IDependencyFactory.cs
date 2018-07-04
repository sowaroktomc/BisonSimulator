using Sowalabs.Bison.Common.DateTimeProvider;
using Sowalabs.Bison.Common.Timer;

namespace Sowalabs.Bison.LiquidityEngine.Dependencies
{
    public interface IDependencyFactory
    {
        IBankApi GetBankApi(string bankSwiftCode);
        ITimerFactory TimerFactory { get; }
        IDateTimeProvider DateTimeProvider { get; }

        IResetEvent CreateResetEvent();
    }
}
