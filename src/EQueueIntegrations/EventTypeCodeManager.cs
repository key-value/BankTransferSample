using System;
using System.Collections.Generic;
using System.Linq;
using BankTransferSample.DomainEvents;
using ENode.EQueue;
using ENode.Eventing;

namespace BankTransferSample.EQueueIntegrations
{
    public class EventTypeCodeManager : IEventTypeCodeProvider
    {
        private IDictionary<int, Type> _typeCodeDict = new Dictionary<int, Type>();

        public EventTypeCodeManager()
        {
            _typeCodeDict.Add(100, typeof(AccountCreatedEvent));
            _typeCodeDict.Add(101, typeof(TransactionPreparationAddedEvent));
            _typeCodeDict.Add(102, typeof(TransactionPreparationCommittedEvent));
            _typeCodeDict.Add(103, typeof(TransactionPreparationCanceledEvent));
            _typeCodeDict.Add(104, typeof(InsufficientBalanceEvent));

            _typeCodeDict.Add(201, typeof(DepositTransactionStartedEvent));
            _typeCodeDict.Add(202, typeof(DepositTransactionCommittedEvent));
            _typeCodeDict.Add(203, typeof(DepositTransactionCompletedEvent));

            _typeCodeDict.Add(301, typeof(TransferTransactionStartedEvent));
            _typeCodeDict.Add(302, typeof(TransferOutPreparationConfirmedEvent));
            _typeCodeDict.Add(303, typeof(TransferInPreparationConfirmedEvent));
            _typeCodeDict.Add(304, typeof(TransferOutConfirmedEvent));
            _typeCodeDict.Add(305, typeof(TransferInConfirmedEvent));
            _typeCodeDict.Add(306, typeof(TransferTransactionCommittedEvent));
            _typeCodeDict.Add(307, typeof(TransferTransactionCompletedEvent));
            _typeCodeDict.Add(308, typeof(TransferTransactionCancelStartedEvent));
            _typeCodeDict.Add(309, typeof(TransferOutCanceledConfirmedEvent));
            _typeCodeDict.Add(310, typeof(TransferInCanceledConfirmedEvent));
            _typeCodeDict.Add(311, typeof(TransferTransactionCanceledEvent));
        }

        public int GetTypeCode(IDomainEvent domainEvent)
        {
            return _typeCodeDict.Single(x => x.Value == domainEvent.GetType()).Key;
        }
        public Type GetType(int typeCode)
        {
            return _typeCodeDict[typeCode];
        }
    }
}
