using System;
using BankTransferSample.Domain;

namespace BankTransferSample.Exceptions
{
    public class TransactionPreparationNotExistException : Exception
    {
        public TransactionPreparationNotExistException(string accountId, string transactionId, TransactionType transactionType)
            : base(string.Format("TransactionPreparation[transactionId={0},transactionType={1}] not exist in account[id={2}].", transactionId, transactionType, accountId))
        {
        }
    }
}
