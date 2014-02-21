using System;
using BankTransferSample.DomainEvents;
using ECommon.Utilities;
using ENode.Commanding;

namespace BankTransferSample.Commands
{
    /// <summary>发起一笔转账交易
    /// </summary>
    [Serializable]
    public class StartTransactionCommand : ProcessCommand<string>, ICreatingAggregateCommand
    {
        public TransactionInfo TransactionInfo { get; private set; }

        public StartTransactionCommand(TransactionInfo transactionInfo)
            : base(transactionInfo.TransactionId)
        {
            TransactionInfo = transactionInfo;
        }
    }
    /// <summary>确认预转出
    /// </summary>
    [Serializable]
    public class ConfirmDebitPreparationCommand : ProcessCommand<string>
    {
        public ConfirmDebitPreparationCommand(string transactionId)
            : base(transactionId)
        {
        }
    }
    /// <summary>确认预转入
    /// </summary>
    [Serializable]
    public class ConfirmCreditPreparationCommand : ProcessCommand<string>
    {
        public ConfirmCreditPreparationCommand(string transactionId)
            : base(transactionId)
        {
        }
    }
    /// <summary>确认转出
    /// </summary>
    [Serializable]
    public class ConfirmDebitCommand : ProcessCommand<string>
    {
        public ConfirmDebitCommand(string transactionId)
            : base(transactionId)
        {
        }
    }
    /// <summary>确认转入
    /// </summary>
    [Serializable]
    public class ConfirmCreditCommand : ProcessCommand<string>
    {
        public ConfirmCreditCommand(string transactionId)
            : base(transactionId)
        {
        }
    }
    /// <summary>终止转账交易
    /// </summary>
    [Serializable]
    public class AbortTransactionCommand : ProcessCommand<string>
    {
        public AbortTransactionCommand(string transactionId)
            : base(transactionId)
        {
        }
    }
}
