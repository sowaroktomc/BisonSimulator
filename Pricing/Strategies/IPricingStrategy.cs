namespace Sowalabs.Bison.Pricing.Strategies
{
    public interface IPricingStrategy
    {
        decimal GetBuyPrice(decimal? amount, decimal? value);
        decimal GetSellPrice(decimal? amount, decimal? value);
    }
}
