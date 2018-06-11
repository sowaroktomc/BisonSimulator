namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    class SimulatedBankApi : Sowalabs.Bison.LiquidityEngine.Dependencies.IBankApi
    {
        public decimal Balance { get; private set; }


        public void TransferMoney(string fromAccount, string toAccount, decimal amount)
        {
            if (toAccount == "EUWAX")
            {
                this.Balance += amount;
            }
            if (fromAccount == "EUWAX")
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
