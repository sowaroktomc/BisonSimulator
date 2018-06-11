using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.Dependencies;
using Sowalabs.Bison.ProfitSim.Events;
using Sowalabs.Bison.ProfitSim.IO.Bitstamp;
using Sowalabs.Bison.ProfitSim.Model;
using System;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim
{
    internal class ProfitSimulator
    {

        /// <summary>
        /// Number of simulater users.
        /// </summary>
        public int NumUsers { get; set; }

        /// <summary>
        /// List of sizes (values) of simulater user orders.
        /// </summary>
        public List<decimal> OrderSizes { get; }

        /// <summary>
        /// Number of seconds the user has to accept or reject a price offer.
        /// </summary>
        public int ReservationPeriod { get; set; }

        /// <summary>
        /// Number of seconds between price offer acceptance and hedging execution on market.
        /// </summary>
        public int HedgingDelay
        {
            get => (int)this._dependencyFactory.HedgingEngine.WhenStrategy.Delay;
            set => this._dependencyFactory.HedgingEngine.WhenStrategy.Delay = value;
        }

        /// <summary>
        /// Rate of simulated offer acceptance (in %).
        /// </summary>
        public double OfferAcceptanceRate { get; set; }

        /// <summary>
        /// Length of simulated time slot (in seconds).
        /// </summary>
        public int SimulatedTime { get; set; }

        /// <summary>
        /// Spread between market best ask and Bison offered price on buys (in %).
        /// </summary>
        public decimal BuySpread
        {
            get => this._dependencyFactory.PricingEngine.PricingStrategy.BuySpread * 100;
            set => this._dependencyFactory.PricingEngine.PricingStrategy.BuySpread = value / 100;
        }

        /// <summary>
        /// Spread between market best bid and Bison offered price on sells (in %).
        /// </summary>
        public decimal SellSpread
        {
            get => this._dependencyFactory.PricingEngine.PricingStrategy.SellSpread * 100;
            set => this._dependencyFactory.PricingEngine.PricingStrategy.SellSpread = value / 100;
        }

        public BuySell CreateBidOrAsks { get; set; }

        private readonly SimulationDependencyFactory _dependencyFactory;

        public ProfitSimulator()
        {
            this.OrderSizes = new List<decimal>();
            this._dependencyFactory = new SimulationDependencyFactory();

            this.ReservationPeriod = 10;
            this.HedgingDelay = 0;
            this.OfferAcceptanceRate = 100;
            this.SimulatedTime = 600;
            this.NumUsers = 1000;
            this.BuySpread = 5;
            this.SellSpread = 5;
        }

        public ProfitSimulationResult ExecuteProfitSimulation()
        {
            var result = new ProfitSimulationResult();

            var historyLoader = new BitstampHistoryLoader();
            var users = this.CreateUsers();

            var executeSimulation = true;

            SimulationEngine.Instance.AfterEventSimulation += (sender, args) =>
            {
                var lastEvent = SimulationEngine.Instance.PeekLastEvent();
                while (lastEvent != null && !(lastEvent is OrderBookEvent))
                {
                    var nextOrderBook = historyLoader.GetNext();
                    if (nextOrderBook == null)
                    {
                        executeSimulation = false;
                        break;
                    }
                    else
                    {
                        SimulationEngine.Instance.AddEvent(new OrderBookEvent(nextOrderBook, this._dependencyFactory.BitcoinMarketApi));
                    }

                    lastEvent = SimulationEngine.Instance.PeekLastEvent();
                }
            };


            while (executeSimulation)
            {
                var firstOrderBook = historyLoader.GetNext();
                SimulationEngine.Instance.AddEvent(new OrderBookEvent(firstOrderBook, this._dependencyFactory.BitcoinMarketApi));
                users.ForEach(user => user.CreateEvents(this._dependencyFactory, firstOrderBook.AcqTime));

                SimulationEngine.Instance.Execute();
                result.Profits.Add(this._dependencyFactory.BitcoinMarketApi.MoneyBalance + this._dependencyFactory.SolarisBank.Balance);
                
                this._dependencyFactory.BitcoinMarketApi.Reset();
                this._dependencyFactory.SolarisBank.Reset();
                executeSimulation &= historyLoader.Restart();
            }

            return result;
        }



        private List<User> CreateUsers()
        {
            var users = new List<User>();

            var random = new Random();
            var userDelay = this.NumUsers != 1 ? this.SimulatedTime / (this.NumUsers - 1) : 0;

            for (int i = 0; i < this.NumUsers; i++)
            {
                var orderSize = this.OrderSizes.Count > 1 ? this.OrderSizes[i] : this.OrderSizes[0];
                var user = new User(this.CreateBidOrAsks, orderSize)
                {
                    RequestDelay = (int) (userDelay * i),
                    AcceptRejectDelay = random.Next(this.ReservationPeriod)
                };

                users.Add(user);
            }

            var acceptCount = (int)Math.Round(this.NumUsers * this.OfferAcceptanceRate / 100, 0);
            users.GetRandomSubset(acceptCount).ForEach(user => user.AcceptsOffer = true);

            return users;
        }
    }
}
