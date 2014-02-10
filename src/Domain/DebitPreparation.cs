using System;
using ECommon.Utilities;

namespace BankTransferSample.Domain
{
    /// <summary>实体，表示一笔转账交易中的预转出信息
    /// </summary>
    [Serializable]
    public class DebitPreparation
    {
        public ObjectId TransactionId { get; private set; }
        public double Amount { get; private set; }

        public DebitPreparation(ObjectId transactionId, double amount)
        {
            TransactionId = transactionId;
            Amount = amount;
        }
    }
}
