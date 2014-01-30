using System;

namespace BankTransferSample.DomainEvents
{
    /// <summary>转账交易操作类型枚举
    /// </summary>
    public enum TransactionOperationType
    {
        PrepareDebit,
        PrepareCredit,
        CommitDebit,
        CommitCredit,
        AbortDebit,
        AbortCredit
    }
}
