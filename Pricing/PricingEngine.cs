using System;
using System.Collections.Generic;
using System.Linq;
using Sowalabs.Bison.Pricing.Data;
using Sowalabs.Bison.Pricing.Dependencies;
using Sowalabs.Bison.Pricing.Strategies;

namespace Sowalabs.Bison.Pricing
{
    public class PricingEngine
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
                    return this._offers.Values.Cast<Bison.Common.Trading.Offer>().ToArray();
                }
            }
        }

        public Common.Trading.OrderBook OrderBook => this._dependencyFactory.GetMarketApi().GetOrderBook();

        public PricingEngine() : this(new PricingDependencyFactory())
        {
        }

        public PricingEngine(IDependencyFactory dependencyFactory)
        {
            this._dependencyFactory = dependencyFactory;
            this.PricingStrategy = new SimpleSpreadStrategy(this);
        }

        public Bison.Common.Trading.Offer GetBuyOffer(decimal volume)
        {
            return this.CreateOffer(volume, Bison.Common.Trading.BuySell.Buy, () => this.PricingStrategy.GetBuyPrice(volume));
        }
        public Bison.Common.Trading.Offer GetSellOffer(decimal volume)
        {
            return this.CreateOffer(volume, Bison.Common.Trading.BuySell.Sell, () => this.PricingStrategy.GetSellPrice(volume));
        }

        private Offer CreateOffer(decimal volume, Bison.Common.Trading.BuySell buySell, Func<decimal> priceGetter)
        {
            lock (this._offers)
            {
                var offer = new Offer
                {
                    Amount = volume,
                    Price = priceGetter(),
                    BuySell = buySell
                };

                this._offers.Add(offer.Id, offer);

                return offer;
            }
        }

        public void AcceptOffer(Guid offerId)
        {
            lock (this._offers)
            {
                this._offers[offerId].Accepted = true;
            }
        }

        public void RejectOffer(Guid offerId)
        {
            lock (this._offers)
            {
                this._offers.Remove(offerId);
            }
        }

    }
}
