using System;
using Sowalabs.Bison.Common.Environment;

namespace Sowalabs.Bison.Common.Trading
{
    public class Offer
    {
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public DateTime OpenTimeStamp { get; }
        public BuySell BuySell { get; set; }
        public Currency Currency { get; set; }

        public Guid Id { get; }

        public Offer()
        {
            this.Id = Guid.NewGuid();
            this.OpenTimeStamp = DateTime.Now;
        }

    }
}
