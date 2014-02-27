using BankTransferSample.Commands;
using BankTransferSample.Domain;
using BankTransferSample.DomainEvents;
using ECommon.IoC;
using ENode.Commanding;
using ENode.Eventing;

namespace BankTransferSample.ProcessManagers
{
    /// <summary>银行存款交易流程管理器，用于协调银行转账交易流程中各个参与者聚合根之间的消息交互。
    /// </summary>
    [Component]
    public class DepositTransactionProcessManager :
        IEventHandler<DepositTransactionStartedEvent>,                    //存款交易已开始
        IEventHandler<DepositPreparationConfirmedEvent>,                  //存款交易预存款已确认
        IEventHandler<TransactionPreparationAddedEvent>,                  //账户预交易已添加
        IEventHandler<TransactionPreparationCommittedEvent>               //账户预交易已提交
    {
        private readonly ICommandService _commandService;

        public DepositTransactionProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public void Handle(DepositTransactionStartedEvent evnt)
        {
            _commandService.Send(new AddTransactionPreparationCommand(
                evnt.AccountId,
                evnt.AggregateRootId,
                TransactionType.DepositTransaction,
                PreparationType.CreditPreparation,
                evnt.Amount));
        }
        public void Handle(TransactionPreparationAddedEvent evnt)
        {
            if (evnt.TransactionPreparation.TransactionType == TransactionType.DepositTransaction)
            {
                if (evnt.TransactionPreparation.PreparationType == PreparationType.CreditPreparation)
                {
                    _commandService.Send(new ConfirmDepositPreparationCommand(evnt.TransactionPreparation.TransactionId));
                }
            }
        }
        public void Handle(DepositPreparationConfirmedEvent evnt)
        {
            _commandService.Send(new CommitTransactionPreparationCommand(
                evnt.AccountId,
                evnt.AggregateRootId,
                TransactionType.DepositTransaction,
                PreparationType.CreditPreparation));
        }
        public void Handle(TransactionPreparationCommittedEvent evnt)
        {
            if (evnt.TransactionPreparation.TransactionType == TransactionType.DepositTransaction)
            {
                if (evnt.TransactionPreparation.PreparationType == PreparationType.CreditPreparation)
                {
                    _commandService.Send(new ConfirmDepositCommand(evnt.TransactionPreparation.TransactionId));
                }
            }
        }
    }
}
