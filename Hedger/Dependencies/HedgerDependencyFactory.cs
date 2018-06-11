using System.Collections.Generic;
using Sowalabs.Bison.Common.Timer;

namespace Sowalabs.Bison.Hedger.Dependencies
{
    internal class HedgerDependencyFactory : IDependencyFactory
    {
        private readonly List<IMarketApi> _marketApis;
        private readonly BasicTimerFactory _timerFactory;

        public HedgerDependencyFactory()
        {
            this._marketApis = new List<IMarketApi>(new[] {new BitstampMarketApi()});
            this._timerFactory = new BasicTimerFactory();
        }

        public IMarketApi GetMarketApi()
        {
            return this._marketApis[0];
        }

        public ITimerFactory TimerFactory => this._timerFactory;
    }
}
