using System;
using System.Collections.Generic;
using System.Linq;
using BankTransferSample.Commands;
using ENode.Commanding;
using ENode.EQueue;

namespace BankTransferSample.EQueueIntegrations
{
    public class CommandTypeCodeManager : ICommandTypeCodeProvider
    {
        private IDictionary<int, Type> _typeCodeDict = new Dictionary<int, Type>();

        public CommandTypeCodeManager()
        {
            _typeCodeDict.Add(100, typeof(CreateAccountCommand));
            _typeCodeDict.Add(101, typeof(DepositCommand));
            _typeCodeDict.Add(102, typeof(WithdrawCommand));
            _typeCodeDict.Add(103, typeof(PrepareDebitCommand));
            _typeCodeDict.Add(104, typeof(PrepareCreditCommand));
            _typeCodeDict.Add(105, typeof(CommitDebitCommand));
            _typeCodeDict.Add(106, typeof(CommitCreditCommand));
            _typeCodeDict.Add(107, typeof(AbortDebitCommand));
            _typeCodeDict.Add(108, typeof(AbortCreditCommand));

            _typeCodeDict.Add(201, typeof(CreateTransactionCommand));
            _typeCodeDict.Add(202, typeof(StartTransactionCommand));
            _typeCodeDict.Add(203, typeof(ConfirmDebitPreparationCommand));
            _typeCodeDict.Add(204, typeof(ConfirmCreditPreparationCommand));
            _typeCodeDict.Add(205, typeof(ConfirmDebitCommand));
            _typeCodeDict.Add(206, typeof(ConfirmCreditCommand));
            _typeCodeDict.Add(207, typeof(AbortTransactionCommand));
        }

        public int GetTypeCode(ICommand command)
        {
            return _typeCodeDict.Single(x => x.Value == command.GetType()).Key;
        }
        public Type GetType(int typeCode)
        {
            return _typeCodeDict[typeCode];
        }
    }
}
