using System;
using System.Collections.Generic;
using System.Linq;
using BankTransferSample.DomainEvents;
using ECommon.Utilities;
using ENode.Domain;

namespace BankTransferSample.Domain
{
    /// <summary>银行账号聚合根，封装银行账户余额变动的数据一致性
    /// </summary>
    [Serializable]
    public class BankAccount : AggregateRoot<string>
    {
        #region Private Variables

        private IList<DebitPreparation> _debitPreparations;
        private IList<CreditPreparation> _creditPreparations;
        private IList<ObjectId> _completedTransactions;

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
        /// <param name="accountId"></param>
        /// <param name="owner"></param>
        public BankAccount(string accountId, string owner)
        {
            RaiseEvent(new AccountCreatedEvent(accountId, owner, DateTime.Now));
        }

        #endregion

        #region Protected Methods

        /// <summary>初始化账号聚合根
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            _debitPreparations = new List<DebitPreparation>();
            _creditPreparations = new List<CreditPreparation>();
            _completedTransactions = new List<ObjectId>();
        }

        #endregion

        #region Public Methods

        /// <summary>存款
        /// </summary>
        /// <param name="amount"></param>
        public void Deposit(double amount)
        {
            RaiseEvent(new DepositedEvent(Id, amount, Balance + amount, DateTime.Now));
        }
        /// <summary>取款
        /// </summary>
        /// <param name="amount"></param>
        public void Withdraw(double amount)
        {
            var availableBalance = GetAvailableBalance();
            if (availableBalance < amount)
            {
                RaiseEvent(new WithdrawInsufficientBalanceEvent(Id, amount, Balance, availableBalance));
                return;
            }
            RaiseEvent(new WithdrawnEvent(Id, amount, Balance - amount, DateTime.Now));
        }
        /// <summary>预转出
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="amount"></param>
        public void PrepareDebit(ObjectId transactionId, double amount)
        {
            if (_completedTransactions.Any(x => x== transactionId))
            {
                RaiseEvent(new InvalidTransactionOperationEvent(Id, transactionId, TransactionOperationType.PrepareDebit));
                return;
            }
            if (_debitPreparations.Any(x => x.TransactionId == transactionId))
            {
                RaiseEvent(new DuplicatedDebitPreparationEvent(Id, transactionId));
                return;
            }
            var availableBalance = GetAvailableBalance();
            if (availableBalance < amount)
            {
                RaiseEvent(new DebitInsufficientBalanceEvent(Id, transactionId, amount, Balance, availableBalance));
                return;
            }

            RaiseEvent(new DebitPreparedEvent(Id, transactionId, amount));
        }
        /// <summary>预转入
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="amount"></param>
        public void PrepareCredit(ObjectId transactionId, double amount)
        {
            if (_completedTransactions.Any(x => x == transactionId))
            {
                RaiseEvent(new InvalidTransactionOperationEvent(Id, transactionId, TransactionOperationType.PrepareCredit));
                return;
            }
            if (_creditPreparations.Any(x => x.TransactionId == transactionId))
            {
                RaiseEvent(new DuplicatedCreditPreparationEvent(Id, transactionId));
                return;
            }

            RaiseEvent(new CreditPreparedEvent(Id, transactionId, amount));
        }
        /// <summary>提交转出
        /// </summary>
        /// <param name="transactionId"></param>
        public void CommitDebit(ObjectId transactionId)
        {
            if (_completedTransactions.Any(x => x == transactionId))
            {
                RaiseEvent(new InvalidTransactionOperationEvent(Id, transactionId, TransactionOperationType.CommitDebit));
                return;
            }
            var preparation = _debitPreparations.SingleOrDefault(x => x.TransactionId == transactionId);
            if (preparation == null)
            {
                RaiseEvent(new DebitPreparationNotExistEvent(Id, transactionId));
                return;
            }

            RaiseEvent(new DebitCommittedEvent(Id, transactionId, preparation.Amount, Balance - preparation.Amount, DateTime.Now));
        }
        /// <summary>提交转入
        /// </summary>
        /// <param name="transactionId"></param>
        public void CommitCredit(ObjectId transactionId)
        {
            if (_completedTransactions.Any(x => x == transactionId))
            {
                RaiseEvent(new InvalidTransactionOperationEvent(Id, transactionId, TransactionOperationType.CommitCredit));
                return;
            }

            var preparation = _creditPreparations.SingleOrDefault(x => x.TransactionId == transactionId);
            if (preparation == null)
            {
                RaiseEvent(new CreditPreparationNotExistEvent(Id, transactionId));
                return;
            }

            RaiseEvent(new CreditCommittedEvent(Id, transactionId, preparation.Amount, Balance + preparation.Amount, DateTime.Now));
        }
        /// <summary>终止转出
        /// </summary>
        /// <param name="transactionId"></param>
        public void AbortDebit(ObjectId transactionId)
        {
            if (_completedTransactions.Any(x => x == transactionId))
            {
                RaiseEvent(new InvalidTransactionOperationEvent(Id, transactionId, TransactionOperationType.AbortDebit));
                return;
            }
            var preparation = _debitPreparations.SingleOrDefault(x => x.TransactionId == transactionId);
            if (preparation != null)
            {
                RaiseEvent(new DebitAbortedEvent(Id, transactionId, preparation.Amount, DateTime.Now));
            }
        }
        /// <summary>终止转入
        /// </summary>
        /// <param name="transactionId"></param>
        public void AbortCredit(ObjectId transactionId)
        {
            if (_completedTransactions.Any(x => x == transactionId))
            {
                RaiseEvent(new InvalidTransactionOperationEvent(Id, transactionId, TransactionOperationType.AbortCredit));
                return;
            }
            var preparation = _creditPreparations.SingleOrDefault(x => x.TransactionId == transactionId);
            if (preparation != null)
            {
                RaiseEvent(new CreditAbortedEvent(Id, transactionId, preparation.Amount, DateTime.Now));
            }
        }

        #endregion

        #region Private Methods

        /// <summary>获取当前真正可用的余额，需要将已冻结的余额计算在内
        /// </summary>
        /// <returns></returns>
        private double GetAvailableBalance()
        {
            if (_debitPreparations.Count == 0)
            {
                return Balance;
            }

            var totalDebitPreparationAmount = 0D;
            foreach (var preparation in _debitPreparations)
            {
                totalDebitPreparationAmount += preparation.Amount;
            }

            return Balance - totalDebitPreparationAmount;
        }

        #endregion

        #region Handler Methods

        private void Handle(AccountCreatedEvent evnt)
        {
            Id = evnt.AggregateRootId;
            Owner = evnt.Owner;
        }
        private void Handle(DepositedEvent evnt)
        {
            Balance = evnt.CurrentBalance;
        }
        private void Handle(WithdrawnEvent evnt)
        {
            Balance = evnt.CurrentBalance;
        }
        private void Handle(DebitPreparedEvent evnt)
        {
            _debitPreparations.Add(new DebitPreparation(evnt.TransactionId, evnt.Amount));
        }
        private void Handle(CreditPreparedEvent evnt)
        {
            _creditPreparations.Add(new CreditPreparation(evnt.TransactionId, evnt.Amount));
        }
        private void Handle(DebitCommittedEvent evnt)
        {
            Balance = evnt.CurrentBalance;
            _debitPreparations.Remove(_debitPreparations.Single(x => x.TransactionId == evnt.TransactionId));
            _completedTransactions.Add(evnt.TransactionId);
        }
        private void Handle(CreditCommittedEvent evnt)
        {
            Balance = evnt.CurrentBalance;
            _creditPreparations.Remove(_creditPreparations.Single(x => x.TransactionId == evnt.TransactionId));
            _completedTransactions.Add(evnt.TransactionId);
        }
        private void Handle(DebitAbortedEvent evnt)
        {
            _debitPreparations.Remove(_debitPreparations.Single(x => x.TransactionId == evnt.TransactionId));
        }
        private void Handle(CreditAbortedEvent evnt)
        {
            _creditPreparations.Remove(_creditPreparations.Single(x => x.TransactionId == evnt.TransactionId));
        }

        #endregion
    }
}
