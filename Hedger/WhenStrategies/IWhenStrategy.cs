using System;

namespace Sowalabs.Bison.Hedger.WhenStrategies
{
    public interface IWhenStrategy : IDisposable
    {
        void RegisterAcceptedOffer(Common.Trading.Offer offer);
    }
}
