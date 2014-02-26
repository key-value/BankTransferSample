﻿using BankTransferSample.Commands;
using BankTransferSample.Domain;
using ECommon.IoC;
using ENode.Commanding;

namespace BankTransferSample.CommandHandlers
{
    /// <summary>银行账户相关命令处理
    /// </summary>
    [Component]
    public class BankAccountCommandHandlers :
        ICommandHandler<CreateAccountCommand>,                       //开户
        ICommandHandler<CreateTransactionPreparationCommand>,        //创建预交易
        ICommandHandler<CommitTransactionPreparationCommand>,        //提交预交易
        ICommandHandler<CancelTransactionPreparationCommand>         //取消预交易
    {
        public void Handle(ICommandContext context, CreateAccountCommand command)
        {
            context.Add(new BankAccount(command.AggregateRootId, command.Owner));
        }
        public void Handle(ICommandContext context, CreateTransactionPreparationCommand command)
        {
            context.Get<BankAccount>(command.AggregateRootId).CreateTransactionPreparation(command.TransactionId, command.TransactionType, command.PreparationType, command.Amount);
        }
        public void Handle(ICommandContext context, CommitTransactionPreparationCommand command)
        {
            context.Get<BankAccount>(command.AggregateRootId).CommitTransactionPreparation(command.TransactionId, command.TransactionType, command.PreparationType);
        }
        public void Handle(ICommandContext context, CancelTransactionPreparationCommand command)
        {
            context.Get<BankAccount>(command.AggregateRootId).CancelTransactionPreparation(command.TransactionId, command.TransactionType, command.PreparationType);
        }
    }
}
