using System;
using Sowalabs.Bison.LiquidityEngine.Tasks;

namespace Sowalabs.Bison.LiquidityEngine
{
    public interface IQueue : IDisposable
    {
        void Enqueue(BaseTask task);
    }
}
