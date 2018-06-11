using System.Collections.Generic;
using OrderBook = Sowalabs.Bison.ProfitSim.IO.Bitstamp.Model.OrderBook;

namespace Sowalabs.Bison.ProfitSim.Model
{
    internal class SimulationCase
    {

        private readonly List<User> _users = new List<User>();
        private List<OrderBook> _orderBookHistory;

        public SimulationCase(List<OrderBook> orderBookHistory)
        {
            this._orderBookHistory = new List<OrderBook>(orderBookHistory);
        }


    }
}
