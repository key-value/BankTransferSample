using System;
using ENode.Commanding;

namespace BankTransferSample.Commands
{
    /// <summary>开户
    /// </summary>
    [Serializable]
    public class CreateAccountCommand : Command<string>, ICreatingAggregateCommand
    {
        public string Owner { get; set; }

        public CreateAccountCommand(string accountId, string owner) : base(accountId)
        {
            Owner = owner;
        }
    }
    /// <summary>存款
    /// </summary>
    [Serializable]
    public class DepositCommand : Command<string>
    {
        public double Amount { get; set; }

        public DepositCommand(string accountId, double amount) : base(accountId)
        {
            Amount = amount;
        }
    }
    /// <summary>取款
    /// </summary>
    [Serializable]
    public class WithdrawCommand : Command<string>
    {
        public double Amount { get; set; }

        public WithdrawCommand(string accountId, double amount) : base(accountId)
        {
            Amount = amount;
        }
    }
    /// <summary>预转出
    /// </summary>
    [Serializable]
    public class PrepareDebitCommand : Command<string>
    {
        public Guid TransactionId { get; set; }
        public double Amount { get; set; }

        public PrepareDebitCommand(string accountId, Guid transactionId, double amount) : base(accountId)
        {
            TransactionId = transactionId;
            Amount = amount;
        }
    }
    /// <summary>预转入
    /// </summary>
    [Serializable]
    public class PrepareCreditCommand : Command<string>
    {
        public Guid TransactionId { get; set; }
        public double Amount { get; set; }

        public PrepareCreditCommand(string accountId, Guid transactionId, double amount) : base(accountId)
        {
            TransactionId = transactionId;
            Amount = amount;
        }
    }
    /// <summary>提交转出
    /// </summary>
    [Serializable]
    public class CommitDebitCommand : Command<string>
    {
        public Guid TransactionId { get; set; }

        public CommitDebitCommand(string accountId, Guid transactionId) : base(accountId)
        {
            TransactionId = transactionId;
        }
    }
    /// <summary>提交转入
    /// </summary>
    [Serializable]
    public class CommitCreditCommand : Command<string>
    {
        public Guid TransactionId { get; set; }

        public CommitCreditCommand(string accountId, Guid transactionId) : base(accountId)
        {
            TransactionId = transactionId;
        }
    }
    /// <summary>终止转出
    /// </summary>
    [Serializable]
    public class AbortDebitCommand : Command<string>
    {
        public Guid TransactionId { get; set; }

        public AbortDebitCommand(string accountId, Guid transactionId) : base(accountId)
        {
            TransactionId = transactionId;
        }
    }
    /// <summary>终止转入
    /// </summary>
    [Serializable]
    public class AbortCreditCommand : Command<string>
    {
        public Guid TransactionId { get; set; }

        public AbortCreditCommand(string accountId, Guid transactionId) : base(accountId)
        {
            TransactionId = transactionId;
        }
    }
}
