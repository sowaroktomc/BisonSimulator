using System;

namespace Sowalabs.Bison.LiquidityEngine.Dependencies
{
    public interface IResetEvent : IDisposable
    {
        void Set();
        bool Wait(int millisecondsTimeout);
    }
}
