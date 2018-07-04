using Sowalabs.Bison.Common.DateTimeProvider;
using Sowalabs.Bison.Common.Timer;
using System;

namespace Sowalabs.Bison.LiquidityEngine.Dependencies
{
    internal class LiquidityEngineDependencyFactory : IDependencyFactory
    {
        
        public ITimerFactory TimerFactory { get; }

        public IDateTimeProvider DateTimeProvider { get; }

        public IResetEvent CreateResetEvent()
        {
            return new ResetEvent();
        }

        public LiquidityEngineDependencyFactory()
        {
            TimerFactory = new BasicTimerFactory();
            DateTimeProvider = new DateTimeProvider();
        }

        public IBankApi GetBankApi(string bankSwiftCode)
        {
            throw new NotImplementedException();
        }

    }
}
