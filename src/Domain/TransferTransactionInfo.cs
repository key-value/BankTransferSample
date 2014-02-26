using System;

namespace BankTransferSample.Domain
{
    /// <summary>值对象，包含了一次转账交易的基本信息
    /// </summary>
    [Serializable]
    public class TransferTransactionInfo
    {
        /// <summary>转账交易ID
        /// </summary>
        public string TransactionId { get; private set; }
        /// <summary>源账户
        /// </summary>
        public string SourceAccountId { get; private set; }
        /// <summary>目标账户
        /// </summary>
        public string TargetAccountId { get; private set; }
        /// <summary>转账金额
        /// </summary>
        public double Amount { get; private set; }

        public TransferTransactionInfo(string transactionId, string sourceAccountId, string targetAccountId, double amount)
        {
            TransactionId = transactionId;
            SourceAccountId = sourceAccountId;
            TargetAccountId = targetAccountId;
            Amount = amount;
        }
    }
}
