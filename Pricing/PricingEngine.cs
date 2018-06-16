using System;
using System.Collections.Generic;
using System.Linq;
using Sowalabs.Bison.Common.BisonApi;
using Sowalabs.Bison.Pricing.Data;
using Sowalabs.Bison.Pricing.Dependencies;
using Sowalabs.Bison.Pricing.Strategies;

namespace Sowalabs.Bison.Pricing
{
    public class PricingEngine : IPricingService
    {

        private readonly Dictionary<Guid, Offer> _offers = new Dictionary<Guid, Offer>();
        public SimpleSpreadStrategy PricingStrategy { get; set; }
        private readonly IDependencyFactory _dependencyFactory;

        public Bison.Common.Trading.Offer[] OpenOffers
        {
            get
            {
                lock (this._offers)
                {
                    return this._offers.Values.Cast<Common.Trading.Offer>().ToArray();
                }
            }
        }

        public Common.Trading.OrderBook OrderBook => this._dependencyFactory.GetMarketApi().GetOrderBook();

        public PricingEngine() : this(new PricingDependencyFactory())
        {
        }

        public PricingEngine(IDependencyFactory dependencyFactory)
        {
            _dependencyFactory = dependencyFactory;
            PricingStrategy = new SimpleSpreadStrategy(this);
        }

        public Common.Trading.Offer GetBuyOffer(decimal? amount, decimal? value)
        {
            return CreateOffer(amount, value, Common.Trading.BuySell.Buy, () => this.PricingStrategy.GetBuyPrice(amount, value));
        }
        public Common.Trading.Offer GetSellOffer(decimal? amount, decimal? value)
        {
            return CreateOffer(amount, value, Common.Trading.BuySell.Sell, () => this.PricingStrategy.GetSellPrice(amount, value));
        }

        private Offer CreateOffer(decimal? volume, decimal? value, Common.Trading.BuySell buySell, Func<decimal> priceGetter)
        {
            lock (_offers)
            {
                var price = priceGetter();
                var offer = new Offer
                {
                    Amount = volume ?? (value ?? 0) / price,
                    Price = priceGetter(),
                    BuySell = buySell
                };

                _offers.Add(offer.Id, offer);

                return offer;
            }
        }

        public void AcceptOffer(Guid offerId)
        {
            lock (_offers)
            {
                _offers[offerId].Accepted = true;
            }
        }

        public void RejectOffer(Guid offerId)
        {
            lock (_offers)
            {
                _offers.Remove(offerId);
            }
        }

        public void MarkOfferExecuted(Guid offerId)
        {
            lock (_offers)
            {
                _offers.Remove(offerId);
            }
        }

    }
}
