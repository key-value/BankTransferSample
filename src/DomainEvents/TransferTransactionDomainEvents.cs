using System;
using BankTransferSample.Domain;
using ENode.Eventing;

namespace BankTransferSample.DomainEvents
{
    /// <summary>转账交易已开始
    /// </summary>
    [Serializable]
    public class TransferTransactionStartedEvent : DomainEvent<string>
    {
        public TransferTransactionInfo TransactionInfo { get; private set; }
        public DateTime StartedTime { get; private set; }

        public TransferTransactionStartedEvent(TransferTransactionInfo transactionInfo, DateTime startedTime)
            : base(transactionInfo.TransactionId)
        {
            TransactionInfo = transactionInfo;
            StartedTime = startedTime;
        }
    }
    /// <summary>转账交易预转出已确认
    /// </summary>
    [Serializable]
    public class TransferOutPreparationConfirmedEvent : DomainEvent<string>
    {
        public TransferTransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public TransferOutPreparationConfirmedEvent(string transactionId, TransferTransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>转账交易预转入已确认
    /// </summary>
    [Serializable]
    public class TransferInPreparationConfirmedEvent : DomainEvent<string>
    {
        public TransferTransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public TransferInPreparationConfirmedEvent(string transactionId, TransferTransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>转账交易转出已确认
    /// </summary>
    [Serializable]
    public class TransferOutConfirmedEvent : DomainEvent<string>
    {
        public TransferTransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public TransferOutConfirmedEvent(string transactionId, TransferTransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>转账交易转入已确认
    /// </summary>
    [Serializable]
    public class TransferInConfirmedEvent : DomainEvent<string>
    {
        public TransferTransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public TransferInConfirmedEvent(string transactionId, TransferTransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>转账交易已确认，表示转出账户和转入账户都已确认成功
    /// </summary>
    [Serializable]
    public class TransferTransactionConfirmedEvent : DomainEvent<string>
    {
        public TransferTransactionInfo TransactionInfo { get; private set; }
        public DateTime CommittedTime { get; private set; }

        public TransferTransactionConfirmedEvent(string transactionId, TransferTransactionInfo transactionInfo, DateTime committedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            CommittedTime = committedTime;
        }
    }
    /// <summary>转账交易已完成
    /// </summary>
    [Serializable]
    public class TransferTransactionCompletedEvent : DomainEvent<string>, IProcessCompletedEvent
    {
        public TransferTransactionInfo TransactionInfo { get; private set; }
        public DateTime CompletedTime { get; private set; }

        public TransferTransactionCompletedEvent(string transactionId, TransferTransactionInfo transactionInfo, DateTime completedTime)
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
    /// <summary>转账交易取消已开始
    /// </summary>
    [Serializable]
    public class TransferTransactionCancelStartedEvent : DomainEvent<string>
    {
        public TransferTransactionInfo TransactionInfo { get; private set; }

        public TransferTransactionCancelStartedEvent(string transactionId, TransferTransactionInfo transactionInfo)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
        }
    }
    /// <summary>转账交易取消转出已确认
    /// </summary>
    [Serializable]
    public class TransferOutCanceledConfirmedEvent : DomainEvent<string>
    {
        public TransferTransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public TransferOutCanceledConfirmedEvent(string transactionId, TransferTransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>转账交易取消转入已确认
    /// </summary>
    [Serializable]
    public class TransferInCanceledConfirmedEvent : DomainEvent<string>
    {
        public TransferTransactionInfo TransactionInfo { get; private set; }
        public DateTime ConfirmedTime { get; private set; }

        public TransferInCanceledConfirmedEvent(string transactionId, TransferTransactionInfo transactionInfo, DateTime confirmedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            ConfirmedTime = confirmedTime;
        }
    }
    /// <summary>转账交易已取消（结束），交易已失败
    /// </summary>
    [Serializable]
    public class TransferTransactionCanceledEvent : DomainEvent<string>, IProcessCompletedEvent
    {
        public TransferTransactionInfo TransactionInfo { get; private set; }
        public DateTime CanceledTime { get; private set; }

        public TransferTransactionCanceledEvent(string transactionId, TransferTransactionInfo transactionInfo, DateTime abortedTime)
            : base(transactionId)
        {
            TransactionInfo = transactionInfo;
            CanceledTime = abortedTime;
        }

        public string ProcessId
        {
            get { return TransactionInfo.TransactionId.ToString(); }
        }
    }
}
