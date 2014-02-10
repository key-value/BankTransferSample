using System;
using ECommon.Utilities;
using ENode.Eventing;

namespace BankTransferSample.DomainEvents
{
    /// <summary>交易转入已确认
    /// </summary>
    [Serializable]
    public class CreditConfirmedEvent : SourcingEvent<ObjectId>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public CreditConfirmedEvent(ObjectId transactionId, TransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>交易预转入已确认
    /// </summary>
    [Serializable]
    public class CreditPreparationConfirmedEvent : SourcingEvent<ObjectId>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public CreditPreparationConfirmedEvent(ObjectId transactionId, TransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>交易转出已确认
    /// </summary>
    [Serializable]
    public class DebitConfirmedEvent : SourcingEvent<ObjectId>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public DebitConfirmedEvent(ObjectId transactionId, TransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>交易预转出已确认
    /// </summary>
    [Serializable]
    public class DebitPreparationConfirmedEvent : SourcingEvent<ObjectId>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public DebitPreparationConfirmedEvent(ObjectId transactionId, TransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>交易已终止
    /// </summary>
    [Serializable]
    public class TransactionAbortedEvent : SourcingEvent<ObjectId>, IProcessCompletedEvent
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime AbortedTime { get; private set; }

        public TransactionAbortedEvent(ObjectId transactionId, TransactionInfo transactionInfo, DateTime abortedTime)
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
    public class TransactionCommittedEvent : SourcingEvent<ObjectId>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime CommittedTime { get; private set; }

        public TransactionCommittedEvent(ObjectId transactionId, TransactionInfo transactionInfo, DateTime committedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            CommittedTime = committedTime;
        }
    }
    /// <summary>交易已完成
    /// </summary>
    [Serializable]
    public class TransactionCompletedEvent : SourcingEvent<ObjectId>, IProcessCompletedEvent
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime CompletedTime { get; private set; }

        public TransactionCompletedEvent(ObjectId transactionId, TransactionInfo transactionInfo, DateTime completedTime)
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
    /// <summary>交易已创建
    /// </summary>
    [Serializable]
    public class TransactionCreatedEvent : SourcingEvent<ObjectId>
    {
        public TransactionInfo TransactionInfo { get; private set; }

        public TransactionCreatedEvent(TransactionInfo transactionInfo)
            : base(transactionInfo.TransactionId)
        {
            TransactionInfo = transactionInfo;
        }
    }
    /// <summary>交易已开始
    /// </summary>
    [Serializable]
    public class TransactionStartedEvent : SourcingEvent<ObjectId>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime StartedTime { get; private set; }

        public TransactionStartedEvent(ObjectId transactionId, TransactionInfo transactionInfo, DateTime startedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            StartedTime = startedTime;
        }
    }





}
