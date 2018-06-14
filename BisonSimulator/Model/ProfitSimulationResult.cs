using System;
using System.Collections.Generic;
using System.Linq;

namespace Sowalabs.Bison.ProfitSim.Model
{
    internal class ProfitSimulationResult
    {
 
        public class Profit {
            public Profit(decimal value)
            {
                Value = value;
            }

            public decimal Value { get; set; }
            public MarketVolatility Volatility { get; set; }
            public MarketTrend Trend { get; set; }
            public MarketExtreme Extreme { get; set; }
        }

        public class DistributionEntry<TOrdinal>
        {
            public TOrdinal Value { get; set; }
            public int Count { get; set; }

            public override string ToString()
            {
                return $"{this.Value:N2}  {this.Count}";
            }
        }

        private const int BUCKETS = 10;

        public List<Profit> Profits { get; }


        public ProfitSimulationResult()
        {
            this.Profits = new List<Profit>();
        }

        public List<DistributionEntry<decimal>> GetProfitDistribution(MarketVolatility? volatility = null, MarketTrend? trend = null, MarketExtreme? extreme = null) 
        {
            var profits = Profits.Where(p =>
                (volatility == null || (p.Volatility & volatility) == p.Volatility) && (trend == null || (p.Trend & trend) == p.Trend) && (extreme == null || (p.Extreme & extreme) == p.Extreme)).ToList();

            if (profits.Count == 0)
            {
                return new List<DistributionEntry<decimal>>();
            }


            //TODO Better way for bucketing?!
            var min = profits.Min(profit => profit.Value);
            var max = profits.Max(profit => profit.Value);
            var bucketSize = Math.Abs(max - min) / BUCKETS;

            var result = profits.GroupBy(val =>
                {
                    // Do not want the limit value to fall into next (11th) bucket so it gets special treatment!
                    if (val.Value == max)
                    {
                        return BUCKETS - 1;
                    }

                    return (int) ((val.Value - min) / bucketSize);
                }).Select(grouping =>
                    new DistributionEntry<decimal>
                    {
                        Value = (grouping.Key + 0.5m) * bucketSize + min,
                        Count = grouping.Count()
                    })
                .OrderBy(val => val.Value)
                .ToList();

            return result;
        }
    }
}
