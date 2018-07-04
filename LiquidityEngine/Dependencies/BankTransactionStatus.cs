namespace Sowalabs.Bison.LiquidityEngine.Dependencies
{
    public class BankTransactionStatusResponse
    {
        public enum BankTransactionStatus
        {
            Pending,
            Executed,
            InsufficentFunds,
            Rejected
        }

        public BankTransactionStatusResponse(BankTransactionStatus status, string description)
        {
            Status = status;
            Description = description;
        }

        public BankTransactionStatus Status { get; }
        public string Description { get; }
    }
}
