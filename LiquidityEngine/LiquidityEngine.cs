using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.LiquidityEngine.Dependencies;

namespace Sowalabs.Bison.LiquidityEngine
{
    public class LiquidityEngine
    {

        private readonly IDependencyFactory _dependencyFactory;

        public LiquidityEngine() : this(new LiquidityEngineDependencyFactory())
        {
        }

        public LiquidityEngine(IDependencyFactory dependencyFactory)
        {
            this._dependencyFactory = dependencyFactory;
        }

        public void RegisterAcceptedOffer(Common.Trading.Offer offer)
        {
            if (offer.BuySell == BuySell.Buy)
            {
                this._dependencyFactory.GetSolarisApi().TransferMoney("Customer", "EUWAX", offer.Amount * offer.Price);
            }
            else
            {
                this._dependencyFactory.GetSolarisApi().TransferMoney("EUWAX", "Customer", offer.Amount * offer.Price);
            }
        }

    }
}
