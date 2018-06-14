using System.Collections.Generic;

namespace Sowalabs.Bison.Common.Environment
{
    public class EuwaxData
    {

        private static readonly Dictionary<string, BankData> BankAccounts;
        public static BankData Solaris { get; }

        static EuwaxData()
        {
            BankAccounts = new Dictionary<string, BankData>();

            // TODO Load from settings!
            Solaris = new BankData("SolarisSwiftCode");
            Solaris.AddAccount(new AccountData {Currency = Currency.Eur, Iban = "EuwaxEur@Solaris"});
            Solaris.AddAccount(new AccountData {Currency = Currency.Usd, Iban = "EuwaxUsd@Solaris"});

            BankAccounts.Add(Solaris.SwiftCode, Solaris);

            var bitstampsBank = new BankData("BitstampBankSwiftCode");
            bitstampsBank.AddAccount(new AccountData { Currency = Currency.Eur, Iban = "Eur@Bitstamp" });
            bitstampsBank.AddAccount(new AccountData { Currency = Currency.Usd, Iban = "Usd@Bitstamp" });
            BankAccounts.Add(bitstampsBank.SwiftCode, bitstampsBank);
        }

        public static BankData GetBankData(string bankSwiftCode)
        {
            return BankAccounts[bankSwiftCode];
        }

    }
}
