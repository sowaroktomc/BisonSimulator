using Sowalabs.Bison.Common.Trading;
using System;

namespace Sowalabs.Bison.Common.BisonApi
{
    public interface IPricingService
    {
        Offer GetBuyOffer(decimal volume);
        Offer GetSellOffer(decimal volume);
        void AcceptOffer(Guid offerId);
        void RejectOffer(Guid offerId);
        void MarkOfferExecuted(Guid offerId);
    }
}
