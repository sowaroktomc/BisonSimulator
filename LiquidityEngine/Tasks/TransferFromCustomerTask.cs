using Sowalabs.Bison.LiquidityEngine.Dependencies;
using Sowalabs.Bison.LiquidityEngine.Model;

namespace Sowalabs.Bison.LiquidityEngine.Tasks
{
    class TransferFromCustomerTask : BaseTask
    {

        private readonly MoneyTransferData _data;

        public TransferFromCustomerTask(IDependencyFactory dependencyFactory, LiquidityEngine engine, MoneyTransferData data) : base(dependencyFactory, engine)
        {
            _data = data;
        }

        protected override ExecutionStatus ExecuteTask()
        {
            var solaris = this.DependencyFactory.GetBankApi(_data.FromSwift);
            solaris.TransferMoney(_data.FromIban, _data.ToIban, _data.Amount, string.Empty, string.Empty);

            return ExecutionStatus.Done;
        }
    }
}
