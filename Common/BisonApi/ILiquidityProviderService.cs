using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sowalabs.Bison.Common.BisonApi
{
    public interface ILiquidityProviderService
    {
        void RegisterAcceptedOffer(Common.Trading.Offer offer);
    }
}
