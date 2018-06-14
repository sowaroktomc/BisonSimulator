using System.Collections.Generic;

namespace Sowalabs.Bison.Common.Environment
{
    public class BankData
    {
        private readonly Dictionary<Currency, AccountData> _currencyAccounts;

        public string SwiftCode { get; }

        public BankData(string swiftCode)
        {
            _currencyAccounts = new Dictionary<Currency, AccountData>();
            this.SwiftCode = swiftCode;
        }

        public void AddAccount(AccountData account)
        {
            _currencyAccounts.Add(account.Currency, account);
        }

        public AccountData GetAccount(Currency currency)
        {
            return this._currencyAccounts[currency];
        }
    }
}
