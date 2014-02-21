using System;
using ECommon.Utilities;
using ENode.Eventing;

namespace BankTransferSample.DomainEvents
{
    /// <summary>交易转入已确认
    /// </summary>
    [Serializable]
    public class CreditConfirmedEvent : DomainEvent<string>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public CreditConfirmedEvent(string transactionId, TransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>交易预转入已确认
    /// </summary>
    [Serializable]
    public class CreditPreparationConfirmedEvent : DomainEvent<string>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public CreditPreparationConfirmedEvent(string transactionId, TransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>交易转出已确认
    /// </summary>
    [Serializable]
    public class DebitConfirmedEvent : DomainEvent<string>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public DebitConfirmedEvent(string transactionId, TransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>交易预转出已确认
    /// </summary>
    [Serializable]
    public class DebitPreparationConfirmedEvent : DomainEvent<string>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public DebitPreparationConfirmedEvent(string transactionId, TransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>交易已终止
    /// </summary>
    [Serializable]
    public class TransactionAbortedEvent : DomainEvent<string>, IProcessCompletedEvent
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime AbortedTime { get; private set; }

        public TransactionAbortedEvent(string transactionId, TransactionInfo transactionInfo, DateTime abortedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            AbortedTime = abortedTime;
        }

        public string ProcessId
        {
            get { return TransactionInfo.TransactionId.ToString(); }
        }
    }
    /// <summary>交易已提交
    /// </summary>
    [Serializable]
    public class TransactionCommittedEvent : DomainEvent<string>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime CommittedTime { get; private set; }

        public TransactionCommittedEvent(string transactionId, TransactionInfo transactionInfo, DateTime committedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            CommittedTime = committedTime;
        }
    }
    /// <summary>交易已完成
    /// </summary>
    [Serializable]
    public class TransactionCompletedEvent : DomainEvent<string>, IProcessCompletedEvent
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime CompletedTime { get; private set; }

        public TransactionCompletedEvent(string transactionId, TransactionInfo transactionInfo, DateTime completedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            CompletedTime = completedTime;
        }

        string IProcessCompletedEvent.ProcessId
        {
            get { return TransactionInfo.TransactionId.ToString(); }
        }
    }
    /// <summary>交易已开始
    /// </summary>
    [Serializable]
    public class TransactionStartedEvent : DomainEvent<string>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime StartedTime { get; private set; }

        public TransactionStartedEvent(TransactionInfo transactionInfo, DateTime startedTime)
            : base(transactionInfo.TransactionId)
        {
            TransactionInfo = transactionInfo;
            StartedTime = startedTime;
        }
    }
}
