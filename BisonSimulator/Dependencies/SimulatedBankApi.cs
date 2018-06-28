namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    class SimulatedBankApi : LiquidityEngine.Dependencies.IBankApi
    {
        public decimal Balance { get; private set; }


        public void TransferMoney(string fromAccount, string toAccount, decimal amount)
        {
            if (toAccount.ToUpper().Contains("EUWAX"))
            {
                this.Balance += amount;
            }
            if (fromAccount.ToUpper().Contains("EUWAX"))
            {
                this.Balance -= amount;
            }
        }

        public void Reset()
        {
            this.Balance = 0;
        }
    }
}
