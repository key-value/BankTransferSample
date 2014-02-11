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
            _typeCodeDict.Add(101, typeof(CreditAbortedEvent));
            _typeCodeDict.Add(102, typeof(CreditCommittedEvent));
            _typeCodeDict.Add(103, typeof(CreditPreparationNotExistEvent));
            _typeCodeDict.Add(104, typeof(CreditPreparedEvent));
            _typeCodeDict.Add(105, typeof(DebitAbortedEvent));
            _typeCodeDict.Add(106, typeof(DebitCommittedEvent));
            _typeCodeDict.Add(107, typeof(DebitInsufficientBalanceEvent));
            _typeCodeDict.Add(108, typeof(DebitPreparationNotExistEvent));
            _typeCodeDict.Add(109, typeof(DebitPreparedEvent));
            _typeCodeDict.Add(110, typeof(DepositedEvent));
            _typeCodeDict.Add(111, typeof(DuplicatedCreditPreparationEvent));
            _typeCodeDict.Add(112, typeof(DuplicatedDebitPreparationEvent));
            _typeCodeDict.Add(113, typeof(InvalidTransactionOperationEvent));
            _typeCodeDict.Add(114, typeof(WithdrawInsufficientBalanceEvent));
            _typeCodeDict.Add(115, typeof(WithdrawnEvent));

            _typeCodeDict.Add(201, typeof(CreditConfirmedEvent));
            _typeCodeDict.Add(202, typeof(CreditPreparationConfirmedEvent));
            _typeCodeDict.Add(203, typeof(DebitConfirmedEvent));
            _typeCodeDict.Add(204, typeof(DebitPreparationConfirmedEvent));
            _typeCodeDict.Add(205, typeof(TransactionAbortedEvent));
            _typeCodeDict.Add(206, typeof(TransactionCommittedEvent));
            _typeCodeDict.Add(207, typeof(TransactionCompletedEvent));
            _typeCodeDict.Add(208, typeof(TransactionStartedEvent));
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
