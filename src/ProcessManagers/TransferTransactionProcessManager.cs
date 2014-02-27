using BankTransferSample.Commands;
using BankTransferSample.Domain;
using BankTransferSample.DomainEvents;
using ECommon.IoC;
using ENode.Commanding;
using ENode.Eventing;

namespace BankTransferSample.ProcessManagers
{
    /// <summary>银行转账交易流程管理器，用于协调银行转账交易流程中各个参与者聚合根之间的消息交互。
    /// </summary>
    [Component]
    public class TransferTransactionProcessManager :
        IEventHandler<TransferTransactionStartedEvent>,                  //转账交易已开始
        IEventHandler<TransactionPreparationAddedEvent>,                 //账户预交易已添加
        IEventHandler<TransactionPreparationCommittedEvent>,             //账户预交易已提交
        IEventHandler<TransactionPreparationCanceledEvent>,              //账户预交易已取消
        IEventHandler<TransferTransactionCommittedEvent>,                //转账交易已提交
        IEventHandler<TransferTransactionCancelStartedEvent>,            //转账交易取消已开始
        IEventHandler<InsufficientBalanceEvent>                          //账户余额不足
    {
        private readonly ICommandService _commandService;

        public TransferTransactionProcessManager(ICommandService commandService)
        {
            _commandService = commandService;
        }

        public void Handle(TransferTransactionStartedEvent evnt)
        {
            _commandService.Send(new AddTransactionPreparationCommand(
                evnt.TransactionInfo.SourceAccountId,
                evnt.TransactionInfo.TransactionId,
                TransactionType.TransferTransaction,
                PreparationType.DebitPreparation,
                evnt.TransactionInfo.Amount));

            _commandService.Send(new AddTransactionPreparationCommand(
                evnt.TransactionInfo.TargetAccountId,
                evnt.TransactionInfo.TransactionId,
                TransactionType.TransferTransaction,
                PreparationType.CreditPreparation,
                evnt.TransactionInfo.Amount));
        }
        public void Handle(TransactionPreparationAddedEvent evnt)
        {
            if (evnt.TransactionPreparation.TransactionType == TransactionType.TransferTransaction)
            {
                if (evnt.TransactionPreparation.PreparationType == PreparationType.DebitPreparation)
                {
                    _commandService.Send(new ConfirmTransferOutPreparationCommand(evnt.TransactionPreparation.TransactionId));
                }
                else if (evnt.TransactionPreparation.PreparationType == PreparationType.CreditPreparation)
                {
                    _commandService.Send(new ConfirmTransferInPreparationCommand(evnt.TransactionPreparation.TransactionId));
                }
            }
        }
        public void Handle(TransactionPreparationCommittedEvent evnt)
        {
            if (evnt.TransactionPreparation.TransactionType == TransactionType.TransferTransaction)
            {
                if (evnt.TransactionPreparation.PreparationType == PreparationType.DebitPreparation)
                {
                    _commandService.Send(new ConfirmTransferOutCommand(evnt.TransactionPreparation.TransactionId));
                }
                else if (evnt.TransactionPreparation.PreparationType == PreparationType.CreditPreparation)
                {
                    _commandService.Send(new ConfirmTransferInCommand(evnt.TransactionPreparation.TransactionId));
                }
            }
        }
        public void Handle(TransactionPreparationCanceledEvent evnt)
        {
            if (evnt.TransactionPreparation.TransactionType == TransactionType.TransferTransaction)
            {
                if (evnt.TransactionPreparation.PreparationType == PreparationType.DebitPreparation)
                {
                    _commandService.Send(new ConfirmTransferOutCanceledCommand(evnt.TransactionPreparation.TransactionId));
                }
                else if (evnt.TransactionPreparation.PreparationType == PreparationType.CreditPreparation)
                {
                    _commandService.Send(new ConfirmTransferInCanceledCommand(evnt.TransactionPreparation.TransactionId));
                }
            }
        }
        public void Handle(TransferTransactionCommittedEvent evnt)
        {
            _commandService.Send(new CommitTransactionPreparationCommand(
                evnt.TransactionInfo.SourceAccountId,
                evnt.TransactionInfo.TransactionId,
                TransactionType.TransferTransaction,
                PreparationType.DebitPreparation));

            _commandService.Send(new CommitTransactionPreparationCommand(
                evnt.TransactionInfo.TargetAccountId,
                evnt.TransactionInfo.TransactionId,
                TransactionType.TransferTransaction,
                PreparationType.CreditPreparation));
        }
        public void Handle(TransferTransactionCancelStartedEvent evnt)
        {
            _commandService.Send(new CancelTransactionPreparationCommand(
                evnt.TransactionInfo.SourceAccountId,
                evnt.TransactionInfo.TransactionId,
                TransactionType.TransferTransaction,
                PreparationType.DebitPreparation));

            _commandService.Send(new CancelTransactionPreparationCommand(
                evnt.TransactionInfo.TargetAccountId,
                evnt.TransactionInfo.TransactionId,
                TransactionType.TransferTransaction,
                PreparationType.CreditPreparation));
        }
        public void Handle(InsufficientBalanceEvent evnt)
        {
            if (evnt.TransactionType == TransactionType.TransferTransaction)
            {
                _commandService.Send(new StartCancelTransferTransactionCommand(evnt.TransactionId));
            }
        }
    }
}
