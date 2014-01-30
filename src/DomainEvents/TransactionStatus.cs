namespace BankTransferSample.DomainEvents
{
    /// <summary>转账交易状态
    /// </summary>
    public enum TransactionStatus
    {
        Created,
        Started,
        Committed,
        Completed,
        Aborted
    }
}
