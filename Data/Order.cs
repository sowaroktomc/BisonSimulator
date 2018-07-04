using System;

namespace Sowalabs.Bison.Data
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Entity { get; set; }
        public DateTime OpenTimestamp { get; set; }
        public decimal ExecutedAmount { get; set; }
        public decimal SettledAmount { get; set; }
        public string Reference { get; set; }
    }
}