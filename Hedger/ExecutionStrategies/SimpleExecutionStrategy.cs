using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.Hedger.Dependencies;
using System.Collections.Generic;

namespace Sowalabs.Bison.Hedger.ExecutionStrategies
{
    public class SimpleExecutionStrategy : IExecutionStrategy
    {
        private readonly IDependencyFactory _dependencyFactory;

        public SimpleExecutionStrategy(IDependencyFactory dependencyFactory)
        {
            this._dependencyFactory = dependencyFactory;
        }

        public void ExecuteOffers(List<Offer> offers)
        {
            foreach (var offer in offers)
            {
                var market = this._dependencyFactory.GetMarketApi();

                switch (offer.BuySell)
                {
                    case BuySell.Buy:
                        market.ExecuteBuy(offer.Amount, offer.Price);
                        break;
                    case BuySell.Sell:
                        market.ExecuteSell(offer.Amount, offer.Price);
                        break;
                }

                this._dependencyFactory.PricingService.MarkOfferExecuted(offer.Id);

            }
        }
    }
}
