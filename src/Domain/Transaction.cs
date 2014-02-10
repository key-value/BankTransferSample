using System;
using BankTransferSample.DomainEvents;
using ECommon.Utilities;
using ENode.Domain;

namespace BankTransferSample.Domain
{
    /// <summary>银行转账交易聚合根，封装一次转账交易的数据一致性
    /// </summary>
    [Serializable]
    public class Transaction : AggregateRoot<ObjectId>
    {
        #region Public Properties

        /// <summary>交易基本信息
        /// </summary>
        public TransactionInfo TransactionInfo { get; private set; }
        /// <summary>预转出已确认
        /// </summary>
        public bool DebitPreparationConfirmed { get; private set; }
        /// <summary>预转入已确认
        /// </summary>
        public bool CreditPreparationConfirmed { get; private set; }
        /// <summary>转出已确认
        /// </summary>
        public bool DebitConfirmed { get; private set; }
        /// <summary>转入已确认
        /// </summary>
        public bool CreditConfirmed { get; private set; }
        /// <summary>当前状态
        /// </summary>
        public TransactionStatus Status { get; private set; }

        #endregion

        #region Constructors

        /// <summary>构造函数
        /// </summary>
        /// <param name="transactionInfo"></param>
        public Transaction(TransactionInfo transactionInfo)
        {
            RaiseEvent(new TransactionCreatedEvent(transactionInfo));
        }

        #endregion

        #region Public Methods

        /// <summary>开始交易
        /// </summary>
        public void Start()
        {
            if (Status == TransactionStatus.Created)
            {
                RaiseEvent(new TransactionStartedEvent(Id, TransactionInfo, DateTime.Now));
            }
        }
        /// <summary>确认预转出
        /// </summary>
        public void ConfirmDebitPreparation()
        {
            if (Status == TransactionStatus.Started)
            {
                if (!DebitPreparationConfirmed)
                {
                    RaiseEvent(new DebitPreparationConfirmedEvent(Id, TransactionInfo, DateTime.Now));
                    if (CreditPreparationConfirmed)
                    {
                        RaiseEvent(new TransactionCommittedEvent(Id, TransactionInfo, DateTime.Now));
                    }
                }
            }
        }
        /// <summary>确认预转入
        /// </summary>
        public void ConfirmCreditPreparation()
        {
            if (Status == TransactionStatus.Started)
            {
                if (!CreditPreparationConfirmed)
                {
                    RaiseEvent(new CreditPreparationConfirmedEvent(Id, TransactionInfo, DateTime.Now));
                    if (DebitPreparationConfirmed)
                    {
                        RaiseEvent(new TransactionCommittedEvent(Id, TransactionInfo, DateTime.Now));
                    }
                }
            }
        }
        /// <summary>确认转出
        /// </summary>
        public void ConfirmDebit()
        {
            if (Status == TransactionStatus.Committed)
            {
                if (!DebitConfirmed)
                {
                    RaiseEvent(new DebitConfirmedEvent(Id, TransactionInfo, DateTime.Now));
                    if (CreditConfirmed)
                    {
                        RaiseEvent(new TransactionCompletedEvent(Id, TransactionInfo, DateTime.Now));
                    }
                }
            }
        }
        /// <summary>确认转入
        /// </summary>
        public void ConfirmCredit()
        {
            if (Status == TransactionStatus.Committed)
            {
                if (!CreditConfirmed)
                {
                    RaiseEvent(new CreditConfirmedEvent(Id, TransactionInfo, DateTime.Now));
                    if (DebitConfirmed)
                    {
                        RaiseEvent(new TransactionCompletedEvent(Id, TransactionInfo, DateTime.Now));
                    }
                }
            }
        }
        /// <summary>终止交易
        /// </summary>
        public void Abort()
        {
            if (Status == TransactionStatus.Started)
            {
                RaiseEvent(new TransactionAbortedEvent(Id, TransactionInfo, DateTime.Now));
            }
        }

        #endregion

        #region Handler Methods

        private void Handle(TransactionCreatedEvent evnt)
        {
            Id = evnt.AggregateRootId;
            TransactionInfo = evnt.TransactionInfo;
            Status = TransactionStatus.Created;
        }
        private void Handle(TransactionStartedEvent evnt)
        {
            Status = TransactionStatus.Started;
        }
        private void Handle(DebitPreparationConfirmedEvent evnt)
        {
            DebitPreparationConfirmed = true;
        }
        private void Handle(CreditPreparationConfirmedEvent evnt)
        {
            CreditPreparationConfirmed = true;
        }
        private void Handle(DebitConfirmedEvent evnt)
        {
            DebitConfirmed = true;
        }
        private void Handle(CreditConfirmedEvent evnt)
        {
            CreditConfirmed = true;
        }
        private void Handle(TransactionCommittedEvent evnt)
        {
            Status = TransactionStatus.Committed;
        }
        private void Handle(TransactionCompletedEvent evnt)
        {
            Status = TransactionStatus.Completed;
        }
        private void Handle(TransactionAbortedEvent evnt)
        {
            Status = TransactionStatus.Aborted;
        }

        #endregion
    }
}
