using System;
using BankTransferSample.DomainEvents;
using ECommon.Utilities;
using ENode.Domain;

namespace BankTransferSample.Domain
{
    /// <summary>聚合根，表示一笔银行存款交易
    /// </summary>
    [Serializable]
    public class DepositTransaction : AggregateRoot<string>
    {
        #region Public Properties

        /// <summary>账户ID
        /// </summary>
        public string AccountId { get; private set; }
        /// <summary>存款金额
        /// </summary>
        public double Amount { get; private set; }
        /// <summary>交易开始时间
        /// </summary>
        public DateTime StartedTime { get; private set; }
        /// <summary>预存款已确认
        /// </summary>
        public bool IsDepositPreparationConfirmed { get; private set; }
        /// <summary>交易状态
        /// </summary>
        public TransactionStatus Status { get; private set; }

        #endregion

        #region Constructors

        /// <summary>构造函数
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="amount"></param>
        public DepositTransaction(string transactionId, string accountId, double amount)
            : base(transactionId)
        {
            RaiseEvent(new DepositTransactionStartedEvent(transactionId, accountId, amount, DateTime.Now));
        }

        #endregion

        #region Public Methods

        /// <summary>确认预存款
        /// </summary>
        public void ConfirmDepositPreparation()
        {
            if (!IsDepositPreparationConfirmed)
            {
                RaiseEvent(new DepositPreparationConfirmedEvent(Id, AccountId, DateTime.Now));
            }
        }
        /// <summary>确认存款
        /// </summary>
        public void ConfirmDeposit()
        {
            if (Status != TransactionStatus.Completed)
            {
                RaiseEvent(new DepositTransactionCompletedEvent(Id, DateTime.Now));
            }
        }

        #endregion

        #region Handler Methods

        private void Handle(DepositTransactionStartedEvent evnt)
        {
            Id = evnt.AggregateRootId;
            AccountId = evnt.AccountId;
            Amount = evnt.Amount;
            StartedTime = evnt.StartedTime;
            Status = TransactionStatus.Started;
        }
        private void Handle(DepositPreparationConfirmedEvent evnt)
        {
            IsDepositPreparationConfirmed = true;
        }
        private void Handle(DepositTransactionCompletedEvent evnt)
        {
            Status = TransactionStatus.Completed;
        }

        #endregion
    }
}
