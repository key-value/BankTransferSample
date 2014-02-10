using System;
using BankTransferSample.DomainEvents;
using ENode.Commanding;

namespace BankTransferSample.Commands
{
    /// <summary>创建一笔转账交易
    /// </summary>
    [Serializable]
    public class CreateTransactionCommand : ProcessCommand<Guid>, ICreatingAggregateCommand
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public override string ProcessId { get { return TransactionInfo.TransactionId.ToString(); } }

        public CreateTransactionCommand(TransactionInfo transactionInfo) : base(transactionInfo.TransactionId)
        {
            TransactionInfo = transactionInfo;
        }
    }
    /// <summary>发起转账交易
    /// </summary>
    [Serializable]
    public class StartTransactionCommand : ProcessCommand<Guid>
    {
        public override string ProcessId { get { return AggregateRootId.ToString(); } }

        public StartTransactionCommand(Guid transactionId) : base(transactionId)
        {
        }
    }
    /// <summary>确认预转出
    /// </summary>
    [Serializable]
    public class ConfirmDebitPreparationCommand : ProcessCommand<Guid>
    {
        public override string ProcessId { get { return AggregateRootId.ToString(); } }

        public ConfirmDebitPreparationCommand(Guid transactionId) : base(transactionId)
        {
        }
    }
    /// <summary>确认预转入
    /// </summary>
    [Serializable]
    public class ConfirmCreditPreparationCommand : ProcessCommand<Guid>
    {
        public override string ProcessId { get { return AggregateRootId.ToString(); } }

        public ConfirmCreditPreparationCommand(Guid transactionId) : base(transactionId)
        {
        }
    }
    /// <summary>确认转出
    /// </summary>
    [Serializable]
    public class ConfirmDebitCommand : ProcessCommand<Guid>
    {
        public override string ProcessId { get { return AggregateRootId.ToString(); } }

        public ConfirmDebitCommand(Guid transactionId) : base(transactionId)
        {
        }
    }
    /// <summary>确认转入
    /// </summary>
    [Serializable]
    public class ConfirmCreditCommand : ProcessCommand<Guid>
    {
        public override string ProcessId { get { return AggregateRootId.ToString(); } }

        public ConfirmCreditCommand(Guid transactionId) : base(transactionId)
        {
        }
    }
    /// <summary>终止转账交易
    /// </summary>
    [Serializable]
    public class AbortTransactionCommand : ProcessCommand<Guid>
    {
        public override string ProcessId { get { return AggregateRootId.ToString(); } }

        public AbortTransactionCommand(Guid transactionId) : base(transactionId)
        {
        }
    }
}
