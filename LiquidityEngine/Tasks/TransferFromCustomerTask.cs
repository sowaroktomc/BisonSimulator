using Sowalabs.Bison.LiquidityEngine.Dependencies;
using Sowalabs.Bison.LiquidityEngine.Model;

namespace Sowalabs.Bison.LiquidityEngine.Tasks
{
    class TransferFromCustomerTask : BaseTask
    {

        private readonly MoneyTransferData _data;

        public TransferFromCustomerTask(IDependencyFactory dependencyFactory, MoneyTransferData data) : base(dependencyFactory)
        {
            _data = data;
        }

        protected override bool ExecuteTask()
        {
            var solaris = this.DependencyFactory.GetBankApi(_data.FromSwift);
            solaris.TransferMoney(_data.FromIban, _data.ToIban, _data.Amount);

            return true;
        }
    }
}
