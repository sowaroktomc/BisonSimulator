﻿using Sowalabs.Bison.Common.Timer;

namespace Sowalabs.Bison.Hedger.Dependencies
{
    public interface IDependencyFactory
    {
        IMarketApi GetMarketApi();

        ITimerFactory TimerFactory { get; }
    }
}
