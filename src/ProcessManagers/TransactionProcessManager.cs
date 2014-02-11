using BankTransferSample.Commands;
using BankTransferSample.DomainEvents;
using ECommon.IoC;
using ENode.Commanding;
using ENode.Eventing;

namespace BankTransferSample.ProcessManagers
{
    /// <summary>银行转账交易流程管理器，用于协调银行转账交易流程中各个参与者聚合根之间的消息交互。
    /// </summary>
    [Component]
    public class TransactionProcessManager :
        IEventHandler<TransactionStartedEvent>,                  //交易已开始
        IEventHandler<DebitPreparedEvent>,                       //交易已预转出
        IEventHandler<CreditPreparedEvent>,                      //交易已预转入
        IEventHandler<DebitInsufficientBalanceEvent>,            //余额不足不允许预转出操作
        IEventHandler<TransactionCommittedEvent>,                //交易已提交
        IEventHandler<TransactionAbortedEvent>,                  //交易已终止
        IEventHandler<DebitCommittedEvent>,                      //交易转出已提交
        IEventHandler<CreditCommittedEvent>                      //交易转入已提交
    {
        private readonly ICommandService _commandService;

        public TransactionProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public void Handle(TransactionStartedEvent evnt)
        {
            _commandService.Send(new PrepareDebitCommand(evnt.TransactionInfo.SourceAccountId, evnt.AggregateRootId, evnt.TransactionInfo.Amount));
            _commandService.Send(new PrepareCreditCommand(evnt.TransactionInfo.TargetAccountId, evnt.AggregateRootId, evnt.TransactionInfo.Amount));
        }
        public void Handle(DebitPreparedEvent evnt)
        {
            _commandService.Send(new ConfirmDebitPreparationCommand(evnt.TransactionId));
        }
        public void Handle(CreditPreparedEvent evnt)
        {
            _commandService.Send(new ConfirmCreditPreparationCommand(evnt.TransactionId));
        }
        public void Handle(DebitInsufficientBalanceEvent evnt)
        {
            _commandService.Send(new AbortTransactionCommand(evnt.TransactionId));
        }
        public void Handle(TransactionCommittedEvent evnt)
        {
            _commandService.Send(new CommitDebitCommand(evnt.TransactionInfo.SourceAccountId, evnt.AggregateRootId));
            _commandService.Send(new CommitCreditCommand(evnt.TransactionInfo.TargetAccountId, evnt.AggregateRootId));
        }
        public void Handle(TransactionAbortedEvent evnt)
        {
            _commandService.Send(new AbortDebitCommand(evnt.TransactionInfo.SourceAccountId, evnt.AggregateRootId));
            _commandService.Send(new AbortCreditCommand(evnt.TransactionInfo.TargetAccountId, evnt.AggregateRootId));
        }
        public void Handle(DebitCommittedEvent evnt)
        {
            _commandService.Send(new ConfirmDebitCommand(evnt.TransactionId));
        }
        public void Handle(CreditCommittedEvent evnt)
        {
            _commandService.Send(new ConfirmCreditCommand(evnt.TransactionId));
        }
    }
}
