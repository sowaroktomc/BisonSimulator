using System;
using System.Collections.Generic;
using System.Linq;

namespace Sowalabs.Bison.ProfitSim.Model
{
    internal class ProfitSimulationResult
    {
        private const int BUCKETS = 10;

        public List<decimal> Profits { get; }

        public class DistributionEntry<TOrdinal>
        {
            public TOrdinal Value { get; set; }
            public int Count { get; set; }

            public override string ToString()
            {
                return $"{this.Value:N2}  {this.Count}";
            }
        }

        public List<DistributionEntry<decimal>> ProfitDistribution
        {
            get
            {
                //TODO Better way for bucketing?!
                var min = this.Profits.Min();
                var max = this.Profits.Max();
                var bucketSize = Math.Abs(max - min) / BUCKETS;

                var result = this.Profits.GroupBy(val =>
                    {
                        // Do not want the limit value to fall into next (11th) bucket so it gets special treatment!
                        if (val == max)
                        {
                            return BUCKETS - 1;
                        }
                        return (int) ((val - min) / bucketSize);
                    }).Select(grouping =>
                        new DistributionEntry<decimal>()
                        {
                            Value = (grouping.Key + 0.5m) * bucketSize + min,
                            Count = grouping.Count()
                        })
                    .OrderBy(val => val.Value)
                    .ToList();

                return result;
            }
        }

        public decimal TotalProfit => this.Profits.Count != 0 ? this.Profits.Sum() / this.Profits.Count : 0;

        public ProfitSimulationResult()
        {
            this.Profits = new List<decimal>();
        }
    }
}
