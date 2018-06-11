using System;

namespace Sowalabs.Bison.Common.Timer
{
    public interface ITimer : IDisposable
    {
        double Interval { get; set; }
        Action Action { get; set; }
        void Start();
        void Stop();
    }
}
