using BankTransferSample.Commands;
using BankTransferSample.Domain;
using ECommon.IoC;
using ENode.Commanding;

namespace BankTransferSample.CommandHandlers
{
    /// <summary>银行账户相关命令处理
    /// </summary>
    [Component]
    public class BankAccountCommandHandlers :
        ICommandHandler<CreateAccountCommand>,         //开户
        ICommandHandler<DepositCommand>,               //存钱
        ICommandHandler<WithdrawCommand>,              //取钱
        ICommandHandler<PrepareDebitCommand>,          //预转出
        ICommandHandler<PrepareCreditCommand>,         //预转入
        ICommandHandler<CommitDebitCommand>,           //提交转出
        ICommandHandler<CommitCreditCommand>,          //提交转入
        ICommandHandler<AbortDebitCommand>,            //终止转出
        ICommandHandler<AbortCreditCommand>            //终止转入
    {
        public void Handle(ICommandContext context, CreateAccountCommand command)
        {
            context.Add(new BankAccount(command.AggregateRootId, command.Owner));
        }
        public void Handle(ICommandContext context, DepositCommand command)
        {
            context.Get<BankAccount>(command.AggregateRootId).Deposit(command.Amount);
        }
        public void Handle(ICommandContext context, WithdrawCommand command)
        {
            context.Get<BankAccount>(command.AggregateRootId).Withdraw(command.Amount);
        }
        public void Handle(ICommandContext context, PrepareDebitCommand command)
        {
            context.Get<BankAccount>(command.AggregateRootId).PrepareDebit(command.TransactionId, command.Amount);
        }
        public void Handle(ICommandContext context, PrepareCreditCommand command)
        {
            context.Get<BankAccount>(command.AggregateRootId).PrepareCredit(command.TransactionId, command.Amount);
        }
        public void Handle(ICommandContext context, CommitDebitCommand command)
        {
            context.Get<BankAccount>(command.AggregateRootId).CommitDebit(command.TransactionId);
        }
        public void Handle(ICommandContext context, CommitCreditCommand command)
        {
            context.Get<BankAccount>(command.AggregateRootId).CommitCredit(command.TransactionId);
        }
        public void Handle(ICommandContext context, AbortDebitCommand command)
        {
            context.Get<BankAccount>(command.AggregateRootId).AbortDebit(command.TransactionId);
        }
        public void Handle(ICommandContext context, AbortCreditCommand command)
        {
            context.Get<BankAccount>(command.AggregateRootId).AbortCredit(command.TransactionId);
        }
    }
}
