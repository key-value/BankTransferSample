using System;
using BankTransferSample.Domain;
using ENode.Eventing;

namespace BankTransferSample.DomainEvents
{
    /// <summary>转账交易已开始
    /// </summary>
    [Serializable]
    public abstract class AbstractTransferTransactionEvent : DomainEvent<string>
    {
        public TransferTransactionInfo TransactionInfo { get; private set; }

        public AbstractTransferTransactionEvent(TransferTransactionInfo transactionInfo) : base(transactionInfo.TransactionId)
        {
            TransactionInfo = transactionInfo;
        }
    }
    /// <summary>转账交易已开始
    /// </summary>
    [Serializable]
    public class TransferTransactionStartedEvent : AbstractTransferTransactionEvent
    {
        public TransferTransactionStartedEvent(TransferTransactionInfo transactionInfo) : base(transactionInfo) { }
    }
    /// <summary>转账交易预转出已确认
    /// </summary>
    [Serializable]
    public class TransferOutPreparationConfirmedEvent : AbstractTransferTransactionEvent
    {
        public TransferOutPreparationConfirmedEvent(TransferTransactionInfo transactionInfo) : base(transactionInfo) { }
    }
    /// <summary>转账交易预转入已确认
    /// </summary>
    [Serializable]
    public class TransferInPreparationConfirmedEvent : AbstractTransferTransactionEvent
    {
        public TransferInPreparationConfirmedEvent(TransferTransactionInfo transactionInfo) : base(transactionInfo) { }
    }
    /// <summary>转账交易已提交
    /// </summary>
    [Serializable]
    public class TransferTransactionCommittedEvent : AbstractTransferTransactionEvent
    {
        public TransferTransactionCommittedEvent(TransferTransactionInfo transactionInfo) : base(transactionInfo) { }
    }
    /// <summary>转账交易转出已确认
    /// </summary>
    [Serializable]
    public class TransferOutConfirmedEvent : AbstractTransferTransactionEvent
    {
        public TransferOutConfirmedEvent(TransferTransactionInfo transactionInfo) : base(transactionInfo) { }
    }
    /// <summary>转账交易转入已确认
    /// </summary>
    [Serializable]
    public class TransferInConfirmedEvent : AbstractTransferTransactionEvent
    {
        public TransferInConfirmedEvent(TransferTransactionInfo transactionInfo) : base(transactionInfo) { }
    }
    /// <summary>转账交易已完成
    /// </summary>
    [Serializable]
    public class TransferTransactionCompletedEvent : AbstractTransferTransactionEvent, IProcessCompletedEvent
    {
        public TransferTransactionCompletedEvent(TransferTransactionInfo transactionInfo) : base(transactionInfo) { }

        string IProcessCompletedEvent.ProcessId
        {
            get { return TransactionInfo.TransactionId.ToString(); }
        }
    }
    /// <summary>转账交易取消已开始
    /// </summary>
    [Serializable]
    public class TransferTransactionCancelStartedEvent : AbstractTransferTransactionEvent
    {
        public TransferTransactionCancelStartedEvent(TransferTransactionInfo transactionInfo) : base(transactionInfo) { }
    }
    /// <summary>转账交易取消转出已确认
    /// </summary>
    [Serializable]
    public class TransferOutCanceledConfirmedEvent : AbstractTransferTransactionEvent
    {
        public TransferOutCanceledConfirmedEvent(TransferTransactionInfo transactionInfo) : base(transactionInfo) { }
    }
    /// <summary>转账交易取消转入已确认
    /// </summary>
    [Serializable]
    public class TransferInCanceledConfirmedEvent : AbstractTransferTransactionEvent
    {
        public TransferInCanceledConfirmedEvent(TransferTransactionInfo transactionInfo) : base(transactionInfo) { }
    }
    /// <summary>转账交易已取消（结束），交易已失败
    /// </summary>
    [Serializable]
    public class TransferTransactionCanceledEvent : AbstractTransferTransactionEvent, IProcessCompletedEvent
    {
        public TransferTransactionCanceledEvent(TransferTransactionInfo transactionInfo) : base(transactionInfo) { }

        string IProcessCompletedEvent.ProcessId
        {
            get { return TransactionInfo.TransactionId.ToString(); }
        }
    }
}
