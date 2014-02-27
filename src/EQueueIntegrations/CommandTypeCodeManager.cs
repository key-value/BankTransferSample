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
            _typeCodeDict.Add(101, typeof(AddTransactionPreparationCommand));
            _typeCodeDict.Add(102, typeof(CommitTransactionPreparationCommand));
            _typeCodeDict.Add(103, typeof(CancelTransactionPreparationCommand));

            _typeCodeDict.Add(201, typeof(StartDepositTransactionCommand));
            _typeCodeDict.Add(202, typeof(ConfirmDepositPreparationCommand));
            _typeCodeDict.Add(203, typeof(ConfirmDepositCommand));

            _typeCodeDict.Add(301, typeof(StartTransferTransactionCommand));
            _typeCodeDict.Add(302, typeof(ConfirmTransferOutPreparationCommand));
            _typeCodeDict.Add(303, typeof(ConfirmTransferInPreparationCommand));
            _typeCodeDict.Add(304, typeof(ConfirmTransferOutCommand));
            _typeCodeDict.Add(305, typeof(ConfirmTransferInCommand));
            _typeCodeDict.Add(306, typeof(StartCancelTransferTransactionCommand));
            _typeCodeDict.Add(307, typeof(ConfirmTransferOutCanceledCommand));
            _typeCodeDict.Add(308, typeof(ConfirmTransferInCanceledCommand));
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
