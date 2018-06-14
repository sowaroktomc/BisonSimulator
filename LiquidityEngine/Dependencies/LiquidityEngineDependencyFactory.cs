using System;

namespace Sowalabs.Bison.LiquidityEngine.Dependencies
{
    internal class LiquidityEngineDependencyFactory : IDependencyFactory
    {
        public IBankApi GetBankApi(string bankSwiftCode)
        {
            throw new NotImplementedException();
        }
    }
}
