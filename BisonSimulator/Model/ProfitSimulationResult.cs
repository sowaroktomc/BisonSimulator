using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Sowalabs.Bison.ProfitSim.Model
{
    internal class ProfitSimulationResult
    {
 
        public class PLEntry {
            public PLEntry(DateTime time, decimal pl)
            {
                DateTime = time;
                PL = pl;
            }

            public DateTime DateTime { get; set; }
            public decimal PL { get; set; }
            public decimal O { get; set; }
            public decimal H { get; set; }
            public decimal L { get; set; }
            public decimal C { get; set; }
            public decimal V { get; set; }

            [JsonIgnore]
            public MarketVolatility Volatility { get; set; }
            [JsonIgnore]
            public MarketTrend Trend { get; set; }
            [JsonIgnore]
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

        public List<PLEntry> Entries { get; }


        public ProfitSimulationResult()
        {
            this.Entries = new List<PLEntry>();
        }

        public List<DistributionEntry<decimal>> GetProfitDistribution(MarketVolatility? volatility = null, MarketTrend? trend = null, MarketExtreme? extreme = null) 
        {
            var profits = Entries.Where(p =>
                (volatility == null || (p.Volatility & volatility) == p.Volatility) && (trend == null || (p.Trend & trend) == p.Trend) && (extreme == null || (p.Extreme & extreme) == p.Extreme)).ToList();

            if (profits.Count == 0)
            {
                return new List<DistributionEntry<decimal>>();
            }


            //TODO Better way for bucketing?!
            var min = profits.Min(profit => profit.PL);
            var max = profits.Max(profit => profit.PL);
            var bucketSize = Math.Abs(max - min) / BUCKETS;

            var result = profits.GroupBy(val =>
                {
                    // Do not want the limit value to fall into next (11th) bucket so it gets special treatment!
                    if (val.PL == max)
                    {
                        return BUCKETS - 1;
                    }

                    return (int) ((val.PL - min) / bucketSize);
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
