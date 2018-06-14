namespace Sowalabs.Bison.LiquidityEngine.Model
{
    class MoneyTransferData
    {
        public string Currency { get; set; }
        public string FromSwift { get; set; }
        public string FromIban { get; set; }
        public string ToSwift { get; set; }
        public string ToIban { get; set; }
        public decimal Amount { get; set; }
    }
}
