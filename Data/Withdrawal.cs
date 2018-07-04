using System;

namespace Sowalabs.Bison.Data
{
    public class Withdrawal
    {
        public int WithdrawalId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public string ToName { get; set; }
        public string ToIban { get; set; }
        public string ToBic { get; set; }
        public string ToAddress { get; set; }
        public string ToPostalCode { get; set; }
        public string ToCity { get; set; }
        public string ToCountryCode { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime OpenTimestamp { get; set; }
        public string InReference { get; set; }
        public string OutReference { get; set; }
    }
}