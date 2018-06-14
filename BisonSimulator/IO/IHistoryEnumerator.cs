using Sowalabs.Bison.Common.Trading;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim.IO
{
    interface IHistoryEnumerator
    {
        void AppendHistory(List<OrderBook> history);
    }
}
