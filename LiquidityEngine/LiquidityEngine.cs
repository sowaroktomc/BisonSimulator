using System;
using Sowalabs.Bison.Common.BisonApi;
using Sowalabs.Bison.Common.Environment;
using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.LiquidityEngine.Dependencies;
using Sowalabs.Bison.LiquidityEngine.Model;
using Sowalabs.Bison.LiquidityEngine.Tasks;

namespace Sowalabs.Bison.LiquidityEngine
{
    public class LiquidityEngine : ILiquidityProviderService
    {

        private readonly IDependencyFactory _dependencyFactory;

        public LiquidityEngine() : this(new LiquidityEngineDependencyFactory())
        {
        }

        public LiquidityEngine(IDependencyFactory dependencyFactory)
        {
            this._dependencyFactory = dependencyFactory;
        }

        public void RegisterAcceptedOffer(Common.Trading.Offer offer)
        {
            if (offer.BuySell == BuySell.Buy)
            {
                var solaris = EuwaxData.Solaris;
                var data = new MoneyTransferData
                {
                    FromSwift = solaris.SwiftCode,
                    FromIban = "CustomerIBAN",
                    ToSwift = solaris.SwiftCode,
                    ToIban = solaris.GetAccount(offer.Currency).Iban,
                    Amount = Math.Round(offer.Amount * offer.Price, 2)
                };
                Queue.Instance.Enqueue(new TransferFromCustomerTask(this._dependencyFactory, data));
            }
            else
            {
                var solaris = EuwaxData.Solaris;
                var data = new MoneyTransferData
                {
                    FromSwift = solaris.SwiftCode,
                    FromIban = solaris.GetAccount(offer.Currency).Iban,
                    ToSwift = solaris.SwiftCode,
                    ToIban = "CustomerIBAN",
                    Amount = Math.Round(offer.Amount * offer.Price, 2)
                };
                Queue.Instance.Enqueue(new TransferToCustomerTask(this._dependencyFactory, data));
            }
        }

    }
}
