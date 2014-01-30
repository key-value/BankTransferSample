using BankTransferSample.Commands;
using BankTransferSample.Domain;
using ECommon.IoC;
using ENode.Commanding;

namespace BankTransferSample.CommandHandlers
{
    /// <summary>银行转账交易相关命令处理
    /// </summary>
    [Component]
    public class TransactionCommandHandlers :
        ICommandHandler<CreateTransactionCommand>,                //创建交易
        ICommandHandler<StartTransactionCommand>,                 //开始交易
        ICommandHandler<ConfirmDebitPreparationCommand>,          //确认预转出
        ICommandHandler<ConfirmCreditPreparationCommand>,         //确认预转入
        ICommandHandler<ConfirmDebitCommand>,                     //确认转出
        ICommandHandler<ConfirmCreditCommand>,                    //确认转入
        ICommandHandler<AbortTransactionCommand>                  //终止交易
    {
        public void Handle(ICommandContext context, CreateTransactionCommand command)
        {
            context.Add(new Transaction(command.TransactionInfo));
        }
        public void Handle(ICommandContext context, StartTransactionCommand command)
        {
            context.Get<Transaction>(command.AggregateRootId).Start();
        }
        public void Handle(ICommandContext context, ConfirmDebitPreparationCommand command)
        {
            context.Get<Transaction>(command.AggregateRootId).ConfirmDebitPreparation();
        }
        public void Handle(ICommandContext context, ConfirmCreditPreparationCommand command)
        {
            context.Get<Transaction>(command.AggregateRootId).ConfirmCreditPreparation();
        }
        public void Handle(ICommandContext context, ConfirmDebitCommand command)
        {
            context.Get<Transaction>(command.AggregateRootId).ConfirmDebit();
        }
        public void Handle(ICommandContext context, ConfirmCreditCommand command)
        {
            context.Get<Transaction>(command.AggregateRootId).ConfirmCredit();
        }
        public void Handle(ICommandContext context, AbortTransactionCommand command)
        {
            context.Get<Transaction>(command.AggregateRootId).Abort();
        }
    }
}
