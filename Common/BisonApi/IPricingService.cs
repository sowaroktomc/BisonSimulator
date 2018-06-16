using Sowalabs.Bison.Common.Trading;
using System;

namespace Sowalabs.Bison.Common.BisonApi
{
    public interface IPricingService
    {
        Offer GetBuyOffer(decimal? amount, decimal? value);
        Offer GetSellOffer(decimal? amount, decimal? value);
        void AcceptOffer(Guid offerId);
        void RejectOffer(Guid offerId);
        void MarkOfferExecuted(Guid offerId);
    }
}
