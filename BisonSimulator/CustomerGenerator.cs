using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.ProfitSim.Model;
using System;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim
{
    internal class CustomerGenerator
    {
        public class Config
        {
            /// <summary>
            /// Number of simulater users.
            /// </summary>
            public int NumCustomers { get; set; }

            /// <summary>
            /// List of sizes (values) of simulater user orders.
            /// </summary>
            public List<decimal> OrderSizes { get; }

            /// <summary>
            /// Number of seconds the user has to accept or reject a price offer.
            /// </summary>
            public int ReservationPeriod { get; set; }

            /// <summary>
            /// Rate of simulated offer acceptance (in %).
            /// </summary>
            public double OfferAcceptanceRate { get; set; }

            /// <summary>
            /// Length of simulated time slot (in seconds).
            /// </summary>
            public int SimulatedTime { get; set; }

            public BuySell CreateBidOrAsks { get; set; }

            public List<Customer> Users { get; set; }

            public Config()
            {
                OrderSizes = new List<decimal>();
                Users = new List<Customer>();

                ReservationPeriod = 10;
                OfferAcceptanceRate = 100;
                SimulatedTime = 600;
                NumCustomers = 1;
            }
        }

        public List<Customer> CreateCustomers(Config config)
        {
            var customers = new List<Customer>();

            var random = new Random();
            var userDelay = config.NumCustomers != 1 ? config.SimulatedTime / (config.NumCustomers - 1) : 0;

            for (int i = 0; i < config.NumCustomers; i++)
            {
                var orderSize = config.OrderSizes.Count > 1 ? config.OrderSizes[i] : config.OrderSizes[0];
                var customer = new Customer(config.CreateBidOrAsks, orderSize, 0)
                {
                    RequestDelay = userDelay * i,
                    AcceptRejectDelay = random.Next(config.ReservationPeriod)
                };

                customers.Add(customer);
            }

            var acceptCount = (int)Math.Round(config.NumCustomers * config.OfferAcceptanceRate / 100, 0);
            customers.GetRandomSubset(acceptCount).ForEach(user => user.AcceptsOffer = true);

            return customers;
        }

    }
}
