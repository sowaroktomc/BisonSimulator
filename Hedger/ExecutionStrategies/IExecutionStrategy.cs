using System.Collections.Generic;

namespace Sowalabs.Bison.Hedger.ExecutionStrategies
{
    public interface IExecutionStrategy
    {
        void ExecuteOffers(List<Common.Trading.Offer> offers);
    }
}
