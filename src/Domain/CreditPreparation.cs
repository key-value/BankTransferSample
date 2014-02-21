using System;
using ECommon.Utilities;

namespace BankTransferSample.Domain
{
    /// <summary>实体，表示一笔转账交易中的预转入信息
    /// </summary>
    [Serializable]
    public class CreditPreparation
    {
        public string TransactionId { get; private set; }
        public double Amount { get; private set; }

        public CreditPreparation(string transactionId, double amount)
        {
            TransactionId = transactionId;
            Amount = amount;
        }
    }
}
