using System;
using BankTransferSample.DomainEvents;
using ECommon.Utilities;
using ENode.Commanding;

namespace BankTransferSample.Commands
{
    /// <summary>创建一笔转账交易
    /// </summary>
    [Serializable]
    public class CreateTransactionCommand : ProcessCommand<ObjectId>, ICreatingAggregateCommand
    {
        public TransactionInfo TransactionInfo { get; private set; }

        public CreateTransactionCommand(TransactionInfo transactionInfo)
            : base(transactionInfo.TransactionId)
        {
            TransactionInfo = transactionInfo;
        }
    }
    /// <summary>发起转账交易
    /// </summary>
    [Serializable]
    public class StartTransactionCommand : ProcessCommand<ObjectId>
    {
        public StartTransactionCommand(ObjectId transactionId)
            : base(transactionId)
        {
        }
    }
    /// <summary>确认预转出
    /// </summary>
    [Serializable]
    public class ConfirmDebitPreparationCommand : ProcessCommand<ObjectId>
    {
        public ConfirmDebitPreparationCommand(ObjectId transactionId)
            : base(transactionId)
        {
        }
    }
    /// <summary>确认预转入
    /// </summary>
    [Serializable]
    public class ConfirmCreditPreparationCommand : ProcessCommand<ObjectId>
    {
        public ConfirmCreditPreparationCommand(ObjectId transactionId)
            : base(transactionId)
        {
        }
    }
    /// <summary>确认转出
    /// </summary>
    [Serializable]
    public class ConfirmDebitCommand : ProcessCommand<ObjectId>
    {
        public ConfirmDebitCommand(ObjectId transactionId)
            : base(transactionId)
        {
        }
    }
    /// <summary>确认转入
    /// </summary>
    [Serializable]
    public class ConfirmCreditCommand : ProcessCommand<ObjectId>
    {
        public ConfirmCreditCommand(ObjectId transactionId)
            : base(transactionId)
        {
        }
    }
    /// <summary>终止转账交易
    /// </summary>
    [Serializable]
    public class AbortTransactionCommand : ProcessCommand<ObjectId>
    {
        public AbortTransactionCommand(ObjectId transactionId)
            : base(transactionId)
        {
        }
    }
}
