using Sowalabs.Bison.Common.Trading;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim.IO
{
    /// <summary>
    /// Interface for classes that provide access to order book history data.
    /// </summary>
    internal interface IHistoryProvider
    {
        void AppendHistory(List<OrderBook> history, int periodSynchronizationToken);
    }
}
