using Sowalabs.Bison.Common.Environment;
using Sowalabs.Bison.LiquidityEngine.Dependencies;
using Sowalabs.Bison.LiquidityEngine.Model;

namespace Sowalabs.Bison.LiquidityEngine.Tasks
{
    class TransferToCustomerTask : BaseTask
    {

        private readonly MoneyTransferData _data;

        public TransferToCustomerTask(IDependencyFactory dependencyFactory, LiquidityEngine engine, MoneyTransferData data) : base(dependencyFactory, engine)
        {
            _data = data;
        }

        protected override ExecutionStatus ExecuteTask()
        {
            var solaris = DependencyFactory.GetBankApi(_data.FromSwift);
            solaris.TransferMoney(_data.FromIban, _data.ToIban, _data.Amount, string.Empty, string.Empty);

            return ExecutionStatus.Done;
        }
    }
}
