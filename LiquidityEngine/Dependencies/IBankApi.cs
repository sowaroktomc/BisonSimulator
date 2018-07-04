namespace Sowalabs.Bison.LiquidityEngine.Dependencies
{
    public interface IBankApi
    {
        decimal GetAccountBalance(string accountNumber);

        string TransferMoney(string fromAccount, string toAccount, decimal amount, string fromReference, string toReference);

        BankTransactionStatusResponse GetTransactionStatus(string bankTransactionId);
    }
}
