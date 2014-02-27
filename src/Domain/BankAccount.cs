using System;
using System.Collections.Generic;
using System.Linq;
using BankTransferSample.DomainEvents;
using BankTransferSample.Exceptions;
using ENode.Domain;

namespace BankTransferSample.Domain
{
    /// <summary>银行账户聚合根，封装银行账户余额变动的数据一致性
    /// </summary>
    [Serializable]
    public class BankAccount : AggregateRoot<string>
    {
        #region Private Variables

        private IList<TransactionPreparation> _transactionPreparations;

        #endregion

        #region Public Properties

        /// <summary>拥有者
        /// </summary>
        public string Owner { get; private set; }
        /// <summary>当前余额
        /// </summary>
        public double Balance { get; private set; }

        #endregion

        #region Constructors

        /// <summary>构造函数
        /// </summary>
        public BankAccount(string accountId, string owner) : base(accountId)
        {
            RaiseEvent(new AccountCreatedEvent(accountId, owner));
        }

        #endregion

        #region Public Methods

        /// <summary>添加一笔预交易
        /// </summary>
        public void AddTransactionPreparation(string transactionId, TransactionType transactionType, PreparationType preparationType, double amount)
        {
            if (_transactionPreparations == null)
            {
                _transactionPreparations = new List<TransactionPreparation>();
            }
            if (_transactionPreparations.Any(x => x.TransactionId == transactionId && x.TransactionType == transactionType && x.PreparationType == preparationType))
            {
                return;
            }
            var availableBalance = GetAvailableBalance();
            if (preparationType == PreparationType.DebitPreparation && availableBalance < amount)
            {
                RaiseEvent(new InsufficientBalanceEvent(Id, transactionId, transactionType, amount, Balance, availableBalance));
                return;
            }

            RaiseEvent(new TransactionPreparationAddedEvent(new TransactionPreparation(Id, transactionId, transactionType, preparationType, amount)));
        }
        /// <summary>执行预交易
        /// </summary>
        public void CommitTransactionPreparation(string transactionId, TransactionType transactionType, PreparationType preparationType)
        {
            var transactionPreparation = GetTransactionPreparation(transactionId, transactionType, preparationType);
            var currentBalance = Balance;
            if (preparationType == PreparationType.DebitPreparation)
            {
                currentBalance -= transactionPreparation.Amount;
            }
            else if (preparationType == PreparationType.CreditPreparation)
            {
                currentBalance += transactionPreparation.Amount;
            }
            RaiseEvent(new TransactionPreparationCommittedEvent(currentBalance, transactionPreparation));
        }
        /// <summary>取消预交易
        /// </summary>
        public void CancelTransactionPreparation(string transactionId, TransactionType transactionType, PreparationType preparationType)
        {
            RaiseEvent(new TransactionPreparationCanceledEvent(GetTransactionPreparation(transactionId, transactionType, preparationType)));
        }

        #endregion

        #region Private Methods

        /// <summary>获取当前账户内的一笔预交易，如果预交易不存在，则抛出异常
        /// </summary>
        private TransactionPreparation GetTransactionPreparation(string transactionId, TransactionType transactionType, PreparationType preparationType)
        {
            if (_transactionPreparations == null || _transactionPreparations.Count == 0)
            {
                throw new TransactionPreparationNotExistException(Id, transactionId, transactionType);
            }
            var transactionPreparation = _transactionPreparations.SingleOrDefault(x => x.TransactionId == transactionId && x.TransactionType == transactionType && x.PreparationType == preparationType);
            if (transactionPreparation == null)
            {
                throw new TransactionPreparationNotExistException(Id, transactionId, transactionType);
            }
            return transactionPreparation;
        }
        /// <summary>获取当前账户的可用余额，需要将已冻结的余额计算在内
        /// </summary>
        private double GetAvailableBalance()
        {
            if (_transactionPreparations == null || _transactionPreparations.Count == 0)
            {
                return Balance;
            }

            var totalDebitTransactionPreparationAmount = 0D;
            foreach (var debitTransactionPreparation in _transactionPreparations.Where(x => x.PreparationType == PreparationType.DebitPreparation))
            {
                totalDebitTransactionPreparationAmount += debitTransactionPreparation.Amount;
            }

            return Balance - totalDebitTransactionPreparationAmount;
        }

        #endregion

        #region Handler Methods

        private void Handle(AccountCreatedEvent evnt)
        {
            _transactionPreparations = new List<TransactionPreparation>();
            Id = evnt.AggregateRootId;
            Owner = evnt.Owner;
        }
        private void Handle(InsufficientBalanceEvent evnt) { }
        private void Handle(TransactionPreparationAddedEvent evnt)
        {
            _transactionPreparations.Add(evnt.TransactionPreparation);
        }
        private void Handle(TransactionPreparationCommittedEvent evnt)
        {
            _transactionPreparations.Remove(_transactionPreparations.Single(x =>
                x.TransactionId == evnt.TransactionPreparation.TransactionId &&
                x.TransactionType == evnt.TransactionPreparation.TransactionType &&
                x.PreparationType == evnt.TransactionPreparation.PreparationType));
            Balance = evnt.CurrentBalance;
        }
        private void Handle(TransactionPreparationCanceledEvent evnt)
        {
            _transactionPreparations.Remove(_transactionPreparations.Single(x =>
                x.TransactionId == evnt.TransactionPreparation.TransactionId &&
                x.TransactionType == evnt.TransactionPreparation.TransactionType &&
                x.PreparationType == evnt.TransactionPreparation.PreparationType));
        }

        #endregion
    }
}
