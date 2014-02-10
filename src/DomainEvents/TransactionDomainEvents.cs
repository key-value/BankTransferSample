using System;
using ENode.Eventing;

namespace BankTransferSample.DomainEvents
{
    /// <summary>交易转入已确认
    /// </summary>
    [Serializable]
    public class CreditConfirmedEvent : SourcingEvent<Guid>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public CreditConfirmedEvent(Guid transactionId, TransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>交易预转入已确认
    /// </summary>
    [Serializable]
    public class CreditPreparationConfirmedEvent : SourcingEvent<Guid>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public CreditPreparationConfirmedEvent(Guid transactionId, TransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>交易转出已确认
    /// </summary>
    [Serializable]
    public class DebitConfirmedEvent : SourcingEvent<Guid>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public DebitConfirmedEvent(Guid transactionId, TransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>交易预转出已确认
    /// </summary>
    [Serializable]
    public class DebitPreparationConfirmedEvent : SourcingEvent<Guid>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public DebitPreparationConfirmedEvent(Guid transactionId, TransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>交易已终止
    /// </summary>
    [Serializable]
    public class TransactionAbortedEvent : SourcingEvent<Guid>, IProcessCompletedEvent
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime AbortedTime { get; private set; }

        public TransactionAbortedEvent(Guid transactionId, TransactionInfo transactionInfo, DateTime abortedTime)
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
    public class TransactionCommittedEvent : SourcingEvent<Guid>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime CommittedTime { get; private set; }

        public TransactionCommittedEvent(Guid transactionId, TransactionInfo transactionInfo, DateTime committedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            CommittedTime = committedTime;
        }
    }
    /// <summary>交易已完成
    /// </summary>
    [Serializable]
    public class TransactionCompletedEvent : SourcingEvent<Guid>, IProcessCompletedEvent
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime CompletedTime { get; private set; }

        public TransactionCompletedEvent(Guid transactionId, TransactionInfo transactionInfo, DateTime completedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            CompletedTime = completedTime;
        }

        public string ProcessId
        {
            get { return TransactionInfo.TransactionId.ToString(); }
        }
    }
    /// <summary>交易已创建
    /// </summary>
    [Serializable]
    public class TransactionCreatedEvent : SourcingEvent<Guid>
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
    public class TransactionStartedEvent : SourcingEvent<Guid>
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public DateTime StartedTime { get; private set; }

        public TransactionStartedEvent(Guid transactionId, TransactionInfo transactionInfo, DateTime startedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            StartedTime = startedTime;
        }
    }





}
