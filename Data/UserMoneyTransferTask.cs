using System.Collections.Generic;

namespace Sowalabs.Bison.Data
{
    public class UserMoneyTransferTask
    {
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string UserAccount { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Reference { get; set; }
        public string BankTransactionId { get; set; }
        public List<UserMoneyTransfer> Transfers { get; set; }

        public UserMoneyTransferTask()
        {
            Transfers = new List<UserMoneyTransfer>();
        }
    }
}
