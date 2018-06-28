using Sowalabs.Bison.ProfitSim.Entity;
using System;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim.CustomerGenerating
{
    /// <summary>
    /// Generator for generating simulated customers.
    /// </summary>
    internal class CustomerGenerator
    {
        /// <summary>
        /// Generates a list of simulated customer descriptions according to given generator configuration.
        /// </summary>
        /// <param name="config">Generator configuration to generate simulated customers by.</param>
        /// <returns>A list of simulated customer descriptions.</returns>
        public List<Customer> CreateCustomers(CustomerGeneratorConfig config)
        {
            var customers = new List<Customer>();
            var random = new Random();

            // Customers arrive with even distribution between T0 and end of simulated time.
            var userDelay = config.NumCustomers != 1 ? config.SimulatedTime / (config.NumCustomers - 1) : 0; 

            for (var i = 0; i < config.NumCustomers; i++)
            {
                // If only one order is given, then all customers trade the same volume.
                var orderSize = config.OrderSizes.Count > 1 ? config.OrderSizes[i] : config.OrderSizes[0];
                var customer = new Customer(config.CreateBuysOrSells, orderSize, 0)
                {
                    RequestDelay = userDelay * i,
                    AcceptRejectDelay = random.Next(config.ReservationPeriod)
                };

                customers.Add(customer);
            }

            var acceptCount = (int) Math.Round(config.NumCustomers * config.OfferAcceptanceRate / 100, 0);
            customers.GetRandomSubset(acceptCount).ForEach(user => user.AcceptsOffer = true);

            return customers;
        }

    }
}
