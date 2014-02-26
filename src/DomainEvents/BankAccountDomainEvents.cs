using System;
using BankTransferSample.Domain;
using ECommon.Utilities;
using ENode.Eventing;

namespace BankTransferSample.DomainEvents
{
    /// <summary>已开户
    /// </summary>
    [Serializable]
    public class AccountCreatedEvent : DomainEvent<string>
    {
        /// <summary>账户拥有者
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
    /// <summary>账户预交易已创建
    /// </summary>
    [Serializable]
    public class TransactionPreparationCreatedEvent : DomainEvent<string>
    {
        public TransactionPreparation TransactionPreparation { get; private set; }
        public DateTime CreatedTime { get; private set; }

        public TransactionPreparationCreatedEvent(TransactionPreparation transactionPreparation, DateTime createdTime)
            : base(transactionPreparation.AccountId)
        {
            TransactionPreparation = transactionPreparation;
            CreatedTime = createdTime;
        }
    }
    /// <summary>账户预交易已执行
    /// </summary>
    [Serializable]
    public class TransactionPreparationCommittedEvent : DomainEvent<string>
    {
        public double CurrentBalance { get; private set; }
        public TransactionPreparation TransactionPreparation { get; private set; }
        public DateTime CommittedTime { get; private set; }

        public TransactionPreparationCommittedEvent(double currentBalance, TransactionPreparation transactionPreparation, DateTime committedTime)
            : base(transactionPreparation.AccountId)
        {
            CurrentBalance = currentBalance;
            TransactionPreparation = transactionPreparation;
            CommittedTime = committedTime;
        }
    }
    /// <summary>账户预交易已取消
    /// </summary>
    [Serializable]
    public class TransactionPreparationCanceledEvent : DomainEvent<string>
    {
        public TransactionPreparation TransactionPreparation { get; private set; }
        public DateTime CanceledTime { get; private set; }

        public TransactionPreparationCanceledEvent(TransactionPreparation transactionPreparation, DateTime canceledTime)
            : base(transactionPreparation.AccountId)
        {
            TransactionPreparation = transactionPreparation;
            CanceledTime = canceledTime;
        }
    }
    /// <summary>余额不足，该领域事件不会改变账户的状态
    /// </summary>
    [Serializable]
    public class InsufficientBalanceEvent : DomainEvent<string>
    {
        /// <summary>交易ID
        /// </summary>
        public string TransactionId { get; private set; }
        /// <summary>交易类型
        /// </summary>
        public TransactionType TransactionType { get; private set; }
        /// <summary>预借金额
        /// </summary>
        public double Amount { get; private set; }
        /// <summary>当前余额
        /// </summary>
        public double CurrentBalance { get; private set; }
        /// <summary>当前可用余额
        /// </summary>
        public double CurrentAvailableBalance { get; private set; }

        public InsufficientBalanceEvent(string accountId, string transactionId, TransactionType transactionType, double amount, double currentBalance, double currentAvailableBalance)
            : base(accountId)
        {
            TransactionId = transactionId;
            TransactionType = transactionType;
            Amount = amount;
            CurrentBalance = currentBalance;
            CurrentAvailableBalance = currentAvailableBalance;
        }
    }
}
