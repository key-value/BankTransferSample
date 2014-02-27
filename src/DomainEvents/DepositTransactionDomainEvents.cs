using System;
using BankTransferSample.Domain;
using ENode.Eventing;

namespace BankTransferSample.DomainEvents
{
    /// <summary>转账交易已开始
    /// </summary>
    [Serializable]
    public class DepositTransactionStartedEvent : DomainEvent<string>
    {
        public string AccountId { get; private set; }
        public double Amount { get; private set; }

        public DepositTransactionStartedEvent(string transactionId, string accountId, double amount)
            : base(transactionId)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
    /// <summary>存款交易已提交
    /// </summary>
    [Serializable]
    public class DepositTransactionCommittedEvent : DomainEvent<string>
    {
        public string AccountId { get; private set; }

        public DepositTransactionCommittedEvent(string transactionId, string accountId)
            : base(transactionId)
        {
            AccountId = accountId;
        }
    }
    /// <summary>存款交易已完成
    /// </summary>
    [Serializable]
    public class DepositTransactionCompletedEvent : DomainEvent<string>, IProcessCompletedEvent
    {
        public string AccountId { get; private set; }

        public DepositTransactionCompletedEvent(string transactionId, string accountId)
            : base(transactionId)
        {
            AccountId = accountId;
        }

        string IProcessCompletedEvent.ProcessId
        {
            get { return AggregateRootId; }
        }
    }
}
