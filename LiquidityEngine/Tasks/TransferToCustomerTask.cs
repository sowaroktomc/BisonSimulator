using Sowalabs.Bison.Common.Environment;
using Sowalabs.Bison.LiquidityEngine.Dependencies;
using Sowalabs.Bison.LiquidityEngine.Model;

namespace Sowalabs.Bison.LiquidityEngine.Tasks
{
    class TransferToCustomerTask : BaseTask
    {

        private readonly MoneyTransferData _data;

        public TransferToCustomerTask(IDependencyFactory dependencyFactory, MoneyTransferData data) : base(dependencyFactory)
        {
            _data = data;
        }

        protected override bool ExecuteTask()
        {
            var solaris = DependencyFactory.GetBankApi(_data.FromSwift);
            solaris.TransferMoney(_data.FromIban, _data.ToIban, _data.Amount);

            return true;
        }
    }
}
