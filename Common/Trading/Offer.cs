using System;
using Sowalabs.Bison.Common.Environment;

namespace Sowalabs.Bison.Common.Trading
{
    public class Offer
    {
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public DateTime OpenTimeStamp { get; set; }
        public BuySell BuySell { get; set; }
        public Currency Currency { get; set; }
        public string Reference { get; set; }
        public int UserId { get; set; }
        public string UserAccount { get; set; }

        public Guid Id { get; }

        public Offer()
        {
            Id = Guid.NewGuid();
        }

    }
}
