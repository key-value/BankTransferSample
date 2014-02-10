using System;
using ECommon.Utilities;
using ENode.Eventing;

namespace BankTransferSample.DomainEvents
{
    /// <summary>已开户
    /// </summary>
    [Serializable]
    public class AccountCreatedEvent : SourcingEvent<string>
    {
        /// <summary>账号拥有者
        /// </summary>
        public string Owner { get; private set; }
        /// <summary>开户时间
        /// </summary>
        public DateTime CreatedTime { get; private set; }

        public AccountCreatedEvent(string accountId, string owner, DateTime createdTime)
            : base(accountId)
        {
            Owner = owner;
            CreatedTime = createdTime;
        }
    }
    /// <summary>交易转入已终止
    /// </summary>
    [Serializable]
    public class CreditAbortedEvent : SourcingEvent<string>
    {
        public ObjectId TransactionId { get; private set; }
        public double Amount { get; private set; }
        public DateTime AbortedTime { get; private set; }

        public CreditAbortedEvent(string accountId, ObjectId transactionId, double amount, DateTime abortedTime)
            : base(accountId)
        {
            TransactionId = transactionId;
            Amount = amount;
            AbortedTime = abortedTime;
        }
    }
    /// <summary>交易转入已提交
    /// </summary>
    [Serializable]
    public class CreditCommittedEvent : SourcingEvent<string>
    {
        /// <summary>交易ID
        /// </summary>
        public ObjectId TransactionId { get; private set; }
        /// <summary>转入金额
        /// </summary>
        public double Amount { get; private set; }
        /// <summary>当前余额
        /// </summary>
        public double CurrentBalance { get; private set; }
        /// <summary>转入时间
        /// </summary>
        public DateTime TransactionTime { get; private set; }

        public CreditCommittedEvent(string accountId, ObjectId transactionId, double amount, double currentBalance, DateTime transactionTime)
            : base(accountId)
        {
            TransactionId = transactionId;
            Amount = amount;
            CurrentBalance = currentBalance;
            TransactionTime = transactionTime;
        }
    }
    /// <summary>交易预转入信息不存在
    /// </summary>
    [Serializable]
    public class CreditPreparationNotExistEvent : DomainEvent<string>
    {
        public ObjectId TransactionId { get; private set; }

        public CreditPreparationNotExistEvent(string accountId, ObjectId transactionId)
            : base(accountId)
        {
            TransactionId = transactionId;
        }
    }
    /// <summary>交易预转入成功
    /// </summary>
    [Serializable]
    public class CreditPreparedEvent : SourcingEvent<string>
    {
        public ObjectId TransactionId { get; private set; }
        public double Amount { get; private set; }

        public CreditPreparedEvent(string accountId, ObjectId transactionId, double amount)
            : base(accountId)
        {
            TransactionId = transactionId;
            Amount = amount;
        }
    }
    /// <summary>交易转出已终止
    /// </summary>
    [Serializable]
    public class DebitAbortedEvent : SourcingEvent<string>
    {
        public ObjectId TransactionId { get; private set; }
        public double Amount { get; private set; }
        public DateTime AbortedTime { get; private set; }

        public DebitAbortedEvent(string accountId, ObjectId transactionId, double amount, DateTime abortedTime)
            : base(accountId)
        {
            TransactionId = transactionId;
            Amount = amount;
            AbortedTime = abortedTime;
        }
    }
    /// <summary>交易转出已提交
    /// </summary>
    [Serializable]
    public class DebitCommittedEvent : SourcingEvent<string>
    {
        /// <summary>交易ID
        /// </summary>
        public ObjectId TransactionId { get; private set; }
        /// <summary>转出金额
        /// </summary>
        public double Amount { get; private set; }
        /// <summary>当前余额
        /// </summary>
        public double CurrentBalance { get; private set; }
        /// <summary>转出时间
        /// </summary>
        public DateTime TransactionTime { get; private set; }

        public DebitCommittedEvent(string accountId, ObjectId transactionId, double amount, double currentBalance, DateTime transactionTime)
            : base(accountId)
        {
            TransactionId = transactionId;
            Amount = amount;
            CurrentBalance = currentBalance;
            TransactionTime = transactionTime;
        }
    }
    /// <summary>余额不足不允许转出操作
    /// </summary>
    [Serializable]
    public class DebitInsufficientBalanceEvent : DomainEvent<string>
    {
        /// <summary>交易ID
        /// </summary>
        public ObjectId TransactionId { get; private set; }
        /// <summary>转出金额
        /// </summary>
        public double Amount { get; private set; }
        /// <summary>当前余额
        /// </summary>
        public double CurrentBalance { get; private set; }
        /// <summary>当前可用余额
        /// </summary>
        public double CurrentAvailableBalance { get; private set; }

        public DebitInsufficientBalanceEvent(string accountId, ObjectId transactionId, double amount, double currentBalance, double currentAvailableBalance)
            : base(accountId)
        {
            TransactionId = transactionId;
            Amount = amount;
            CurrentBalance = currentBalance;
            CurrentAvailableBalance = currentAvailableBalance;
        }
    }
    /// <summary>交易预转出信息不存在
    /// </summary>
    [Serializable]
    public class DebitPreparationNotExistEvent : DomainEvent<string>
    {
        public ObjectId TransactionId { get; private set; }

        public DebitPreparationNotExistEvent(string accountId, ObjectId transactionId)
            : base(accountId)
        {
            TransactionId = transactionId;
        }
    }
    /// <summary>交易预转出成功
    /// </summary>
    [Serializable]
    public class DebitPreparedEvent : SourcingEvent<string>
    {
        public ObjectId TransactionId { get; private set; }
        public double Amount { get; private set; }

        public DebitPreparedEvent(string accountId, ObjectId transactionId, double amount)
            : base(accountId)
        {
            TransactionId = transactionId;
            Amount = amount;
        }
    }
    /// <summary>已存款
    /// </summary>
    [Serializable]
    public class DepositedEvent : SourcingEvent<string>
    {
        /// <summary>存款金额
        /// </summary>
        public double Amount { get; private set; }
        /// <summary>当前余额
        /// </summary>
        public double CurrentBalance { get; private set; }
        /// <summary>存款时间
        /// </summary>
        public DateTime TransactionTime { get; private set; }

        public DepositedEvent(string accountId, double amount, double currentBalance, DateTime transactionTime)
            : base(accountId)
        {
            Amount = amount;
            CurrentBalance = currentBalance;
            TransactionTime = transactionTime;
        }
    }
    /// <summary>重复的预转入操作
    /// </summary>
    [Serializable]
    public class DuplicatedCreditPreparationEvent : DomainEvent<string>
    {
        public ObjectId TransactionId { get; private set; }

        public DuplicatedCreditPreparationEvent(string accountId, ObjectId transactionId)
            : base(accountId)
        {
            TransactionId = transactionId;
        }
    }
    /// <summary>重复的预转出操作
    /// </summary>
    [Serializable]
    public class DuplicatedDebitPreparationEvent : DomainEvent<string>
    {
        public ObjectId TransactionId { get; private set; }

        public DuplicatedDebitPreparationEvent(string accountId, ObjectId transactionId)
            : base(accountId)
        {
            TransactionId = transactionId;
        }
    }
    /// <summary>在转账交易已完成后进行了无效的操作
    /// </summary>
    [Serializable]
    public class InvalidTransactionOperationEvent : DomainEvent<string>
    {
        public ObjectId TransactionId { get; private set; }
        public TransactionOperationType OperationType { get; private set; }

        public InvalidTransactionOperationEvent(string accountId, ObjectId transactionId, TransactionOperationType operationType)
            : base(accountId)
        {
            TransactionId = transactionId;
            OperationType = operationType;
        }
    }
    /// <summary>余额不足不允许取款操作
    /// </summary>
    [Serializable]
    public class WithdrawInsufficientBalanceEvent : DomainEvent<string>
    {
        /// <summary>取款金额
        /// </summary>
        public double Amount { get; private set; }
        /// <summary>当前余额
        /// </summary>
        public double CurrentBalance { get; private set; }
        /// <summary>当前可用余额
        /// </summary>
        public double CurrentAvailableBalance { get; private set; }

        public WithdrawInsufficientBalanceEvent(string accountId, double amount, double currentBalance, double currentAvailableBalance)
            : base(accountId)
        {
            Amount = amount;
            CurrentBalance = currentBalance;
            CurrentAvailableBalance = currentAvailableBalance;
        }
    }
    /// <summary>已取款
    /// </summary>
    [Serializable]
    public class WithdrawnEvent : SourcingEvent<string>
    {
        /// <summary>取款金额
        /// </summary>
        public double Amount { get; private set; }
        /// <summary>当前余额
        /// </summary>
        public double CurrentBalance { get; private set; }
        /// <summary>取款时间
        /// </summary>
        public DateTime TransactionTime { get; private set; }

        public WithdrawnEvent(string accountId, double amount, double currentBalance, DateTime transactionTime)
            : base(accountId)
        {
            Amount = amount;
            CurrentBalance = currentBalance;
            TransactionTime = transactionTime;
        }
    }
}
