using System;
using BankTransferSample.DomainEvents;
using ENode.Commanding;

namespace BankTransferSample.Commands
{
    /// <summary>创建一笔转账交易
    /// </summary>
    [Serializable]
    public class CreateTransactionCommand : Command<Guid>, ICreatingAggregateCommand, IStartProcessCommand
    {
        public TransactionInfo TransactionInfo { get; private set; }
        public object ProcessId { get; private set; }

        public CreateTransactionCommand(TransactionInfo transactionInfo) : base(transactionInfo.TransactionId)
        {
            TransactionInfo = transactionInfo;
            ProcessId = transactionInfo.TransactionId;
        }
    }
    /// <summary>发起转账交易
    /// </summary>
    [Serializable]
    public class StartTransactionCommand : Command<Guid>
    {
        public StartTransactionCommand(Guid transactionId) : base(transactionId)
        {
        }
    }
    /// <summary>确认预转出
    /// </summary>
    [Serializable]
    public class ConfirmDebitPreparationCommand : Command<Guid>
    {
        public ConfirmDebitPreparationCommand(Guid transactionId) : base(transactionId)
        {
        }
    }
    /// <summary>确认预转入
    /// </summary>
    [Serializable]
    public class ConfirmCreditPreparationCommand : Command<Guid>
    {
        public ConfirmCreditPreparationCommand(Guid transactionId) : base(transactionId)
        {
        }
    }
    /// <summary>确认转出
    /// </summary>
    [Serializable]
    public class ConfirmDebitCommand : Command<Guid>
    {
        public ConfirmDebitCommand(Guid transactionId) : base(transactionId)
        {
        }
    }
    /// <summary>确认转入
    /// </summary>
    [Serializable]
    public class ConfirmCreditCommand : Command<Guid>
    {
        public ConfirmCreditCommand(Guid transactionId) : base(transactionId)
        {
        }
    }
    /// <summary>终止转账交易
    /// </summary>
    [Serializable]
    public class AbortTransactionCommand : Command<Guid>
    {
        public AbortTransactionCommand(Guid transactionId) : base(transactionId)
        {
        }
    }
}
