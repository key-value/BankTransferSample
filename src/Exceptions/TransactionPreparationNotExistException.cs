using BankTransferSample.Domain;
using ENode.Domain;

namespace BankTransferSample.Exceptions
{
    public class TransactionPreparationNotExistException : DomainException
    {
        public TransactionPreparationNotExistException(string accountId, string transactionId, TransactionType transactionType)
            : base((int)ExceptionCode.TransactionPreparationNotExist,
            "TransactionPreparation[transactionId={0},transactionType={1}] not exist in account[id={2}].", transactionId, transactionType, accountId)
        {

        }
    }
}
