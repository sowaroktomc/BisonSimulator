using System;

namespace Sowalabs.Bison.Data
{
    public class MarketMoneyTransfer
    {
        public int TransferId { get; set; }
        public string Market { get; set; }
        public string UserAccount { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime OpenTimestamp { get; set; }
        public string Reference { get; set; }
    }
}