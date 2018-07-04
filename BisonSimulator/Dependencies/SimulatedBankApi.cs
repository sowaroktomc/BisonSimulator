using System;
using System.Collections.Generic;
using System.Linq;
using Sowalabs.Bison.LiquidityEngine.Dependencies;
using Sowalabs.Bison.ProfitSim.Events;

namespace Sowalabs.Bison.ProfitSim.Dependencies
{
    class SimulatedBankApi : IBankApi
    {

        public enum SimulatedTransactionStatus
        {
            Pending, Complete, Rejected, InsufficientFunds
        }
        private class SimulatedTransaction
        {
            public string TransactionId { get; set; }
            public string FromAccount { get; set; }
            public string ToAccount { get; set; }
            public decimal Amount { get; set; }
            public SimulatedTransactionStatus Status { get; set; }
        }

        public bool InfiniteBalance { get; set; }
        public int TransferDelay { get; set; }
        public decimal Balance { get; private set; }
        private readonly List<SimulatedTransaction> _transactions = new List<SimulatedTransaction>();
        private readonly SimulationEngine _simulationEngine;
        private readonly Dictionary<string, decimal> _accountBalances = new Dictionary<string, decimal>();

        public SimulatedBankApi(SimulationEngine simulationEngine)
        {
            _simulationEngine = simulationEngine;
        }

        public void SetAccountBalance(string accountNumber, decimal balance)
        {
            _accountBalances[accountNumber] = balance;
        }

        public decimal GetAccountBalance(string accountNumber)
        {
            if (InfiniteBalance)
            {
                return Decimal.MaxValue / 2;
            }
            return _accountBalances.TryGetValue(accountNumber, out var balance) ? balance : 0;
        }

        public string TransferMoney(string fromAccount, string toAccount, decimal amount, string fromReference, string toReference)
        {
            if (toAccount.ToUpper().Contains("EUWAX"))
            {
                Balance += amount;
            }
            if (fromAccount.ToUpper().Contains("EUWAX"))
            {
                Balance -= amount;
            }

            var transfer = new SimulatedTransaction
            {
                TransactionId = Guid.NewGuid().ToString(),
                FromAccount = fromAccount,
                ToAccount = toAccount,
                Amount = amount,
                Status = SimulatedTransactionStatus.Pending
            };
            _transactions.Add(transfer);

            if (TransferDelay == 0)
            {
                ExecuteTransaction(transfer.TransactionId);
            }
            else
            {
                _simulationEngine.AddEvent(new IntraBankTransferStatusEvent(_simulationEngine, this, transfer.TransactionId, TransferDelay));
            }


            return transfer.TransactionId;
        }

        public void ExecuteTransaction(string transactionId)
        {
            var trans = _transactions.First(transaction => transaction.TransactionId == transactionId);
            var fromBalance = GetAccountBalance(trans.FromAccount);
            var toBalance = GetAccountBalance(trans.ToAccount);

            if (fromBalance >= trans.Amount || InfiniteBalance)
            {
                _accountBalances[trans.FromAccount] = fromBalance - trans.Amount;
                _accountBalances[trans.ToAccount] = toBalance + trans.Amount;
                trans.Status = SimulatedTransactionStatus.Complete;
            }
            else
            {
                trans.Status = SimulatedTransactionStatus.InsufficientFunds;
            }
        }

        public void RejectTransaction(string transactionId)
        {
            var trans = _transactions.First(transaction => transaction.TransactionId == transactionId);
            trans.Status = SimulatedTransactionStatus.Rejected;
        }

        public BankTransactionStatusResponse GetTransactionStatus(string bankTransactionId)
        {
            var transaction = _transactions.First(trans => trans.TransactionId == bankTransactionId);
            switch (transaction.Status)
            {
                case SimulatedTransactionStatus.Pending:
                    return new BankTransactionStatusResponse(BankTransactionStatusResponse.BankTransactionStatus.Pending, string.Empty);
                case SimulatedTransactionStatus.Complete:
                    return new BankTransactionStatusResponse(BankTransactionStatusResponse.BankTransactionStatus.Executed, string.Empty);
                case SimulatedTransactionStatus.Rejected:
                    return new BankTransactionStatusResponse(BankTransactionStatusResponse.BankTransactionStatus.Rejected, string.Empty);
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public void Reset()
        {
            Balance = 0;
            _transactions.Clear();
            _accountBalances.Clear();
        }
    }
}
