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
        public DateTime StartedTime { get; private set; }

        public DepositTransactionStartedEvent(string transactionId, string accountId, double amount, DateTime startedTime)
            : base(transactionId)
        {
            AccountId = accountId;
            Amount = amount;
            StartedTime = startedTime;
        }
    }
    /// <summary>存款交易预存款已确认
    /// </summary>
    [Serializable]
    public class DepositPreparationConfirmedEvent : DomainEvent<string>
    {
        public string AccountId { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public DepositPreparationConfirmedEvent(string transactionId, string accountId, DateTime confirmedTime)
            : base(transactionId)
        {
            AccountId = accountId;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>存款交易已完成
    /// </summary>
    [Serializable]
    public class DepositTransactionCompletedEvent : DomainEvent<string>, IProcessCompletedEvent
    {
        public DateTime CompletedTime { get; private set; }

        public DepositTransactionCompletedEvent(string transactionId, DateTime completedTime)
            : base(transactionId)
        {
            CompletedTime = completedTime;
        }

        string IProcessCompletedEvent.ProcessId
        {
            get { return AggregateRootId; }
        }
    }
}
