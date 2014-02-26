using BankTransferSample.Domain;
using ENode.Domain;

namespace BankTransferSample.Exceptions
{
    public class MismatchTransactionPreparationException : DomainException
    {
        public MismatchTransactionPreparationException(TransactionType transactionType, PreparationType preparationType)
            : base((int)ExceptionCode.MismatchTransactionPreparationType, "Mismatch transaction type [{0}] and preparation type [{1}].", transactionType, preparationType)
        {

        }
    }
}
