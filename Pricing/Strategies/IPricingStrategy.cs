namespace Sowalabs.Bison.Pricing.Strategies
{
    public interface IPricingStrategy
    {
        decimal GetBuyPrice(decimal volume);
        decimal GetSellPrice(decimal volume);
    }
}
