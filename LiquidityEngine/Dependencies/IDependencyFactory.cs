namespace Sowalabs.Bison.LiquidityEngine.Dependencies
{
    public interface IDependencyFactory
    {
        IBankApi GetBankApi(string bankSwiftCode);
    }
}
