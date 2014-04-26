﻿using System;
using BankTransferSample.Domain;
using ENode.Commanding;

namespace BankTransferSample.Commands
{
    /// <summary>发起一笔转账交易
    /// </summary>
    [Serializable]
    public class StartTransferTransactionCommand : ProcessCommand<string>, ICreatingAggregateCommand
    {
        public TransferTransactionInfo TransactionInfo { get; private set; }

        public StartTransferTransactionCommand(TransferTransactionInfo transactionInfo)
            : base(transactionInfo.TransactionId, transactionInfo.TransactionId)
        {
            TransactionInfo = transactionInfo;
        }
    }
    /// <summary>确认预转出
    /// </summary>
    [Serializable]
    public class ConfirmTransferOutPreparationCommand : ProcessCommand<string>
    {
        public ConfirmTransferOutPreparationCommand(string transactionId)
            : base(transactionId, transactionId)
        {
        }
    }
    /// <summary>确认预转入
    /// </summary>
    [Serializable]
    public class ConfirmTransferInPreparationCommand : ProcessCommand<string>
    {
        public ConfirmTransferInPreparationCommand(string transactionId)
            : base(transactionId, transactionId)
        {
        }
    }
    /// <summary>确认转出
    /// </summary>
    [Serializable]
    public class ConfirmTransferOutCommand : ProcessCommand<string>
    {
        public ConfirmTransferOutCommand(string transactionId)
            : base(transactionId, transactionId)
        {
        }
    }
    /// <summary>确认转入
    /// </summary>
    [Serializable]
    public class ConfirmTransferInCommand : ProcessCommand<string>
    {
        public ConfirmTransferInCommand(string transactionId)
            : base(transactionId, transactionId)
        {
        }
    }
    /// <summary>开始取消转账交易
    /// </summary>
    [Serializable]
    public class StartCancelTransferTransactionCommand : ProcessCommand<string>
    {
        public StartCancelTransferTransactionCommand(string transactionId)
            : base(transactionId, transactionId)
        {
        }
    }
    /// <summary>确认转出操作已取消
    /// </summary>
    [Serializable]
    public class ConfirmTransferOutCanceledCommand : ProcessCommand<string>
    {
        public ConfirmTransferOutCanceledCommand(string transactionId)
            : base(transactionId, transactionId)
        {
        }
    }
    /// <summary>确认转入操作已取消
    /// </summary>
    [Serializable]
    public class ConfirmTransferInCanceledCommand : ProcessCommand<string>
    {
        public ConfirmTransferInCanceledCommand(string transactionId)
            : base(transactionId, transactionId)
        {
        }
    }
}
