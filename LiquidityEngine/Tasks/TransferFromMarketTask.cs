using Sowalabs.Bison.LiquidityEngine.Dependencies;
using Sowalabs.Bison.LiquidityEngine.Model;

namespace Sowalabs.Bison.LiquidityEngine.Tasks
{
    class TransferFromMarketTask : BaseTask
    {

        private readonly MoneyTransferData _data;

        public TransferFromMarketTask(IDependencyFactory dependencyFactory, MoneyTransferData data) : base(dependencyFactory)
        {
            _data = data;
        }

        protected override ExecutionStatus ExecuteTask()
        {
            var bank = DependencyFactory.GetBankApi(_data.FromSwift);
            bank.TransferMoney(_data.FromIban, _data.ToIban, _data.Amount, string.Empty, string.Empty);

            return ExecutionStatus.Done;
        }
    }
}
