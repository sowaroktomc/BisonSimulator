namespace Sowalabs.Bison.LiquidityEngine.Dependencies
{
    public interface IBankApi
    {

        void TransferMoney(string fromAccount, string toAccount, decimal amount);

    }
}
