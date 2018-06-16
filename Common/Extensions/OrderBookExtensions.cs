using Sowalabs.Bison.Common.Trading;
using System;
using System.Collections.Generic;

namespace Sowalabs.Bison.Common.Extensions
{
    public static class OrderBookExtensions
    {
        public static decimal GetTopEntriesValue(this ICollection<OrderBookEntry> orderBookEntries, decimal amount)
        {
            decimal value = 0;
            foreach (var entry in orderBookEntries)
            {
                var entryAmount = Math.Min(amount, entry.Amount);
                value += entryAmount * entry.Price;
                amount -= entryAmount;

                if (amount <= 0)
                {
                    break;
                }
            }

            return value;
        }
        public static decimal GetTopEntriesAmount(this ICollection<OrderBookEntry> orderBookEntries, decimal value)
        {
            decimal amount = 0;
            foreach (var entry in orderBookEntries)
            {
                var entryValue = Math.Min(value, entry.Amount * entry.Price);
                amount += entryValue / entry.Price;
                value -= entryValue;

                if (value <= 0)
                {
                    break;
                }
            }

            return amount;
        }

    }
}
