using System;
using BankTransferSample.DomainEvents;
using ECommon.Utilities;
using ENode.Domain;

namespace BankTransferSample.Domain
{
    /// <summary>聚合根，表示一笔银行内账户之间的转账交易
    /// </summary>
    [Serializable]
    public class TransferTransaction : AggregateRoot<string>
    {
        #region Public Properties

        /// <summary>交易基本信息
        /// </summary>
        public TransferTransactionInfo TransactionInfo { get; private set; }
        /// <summary>交易开始时间
        /// </summary>
        public DateTime StartedTime { get; private set; }
        /// <summary>预转出已确认
        /// </summary>
        public bool IsTransferOutPreparationConfirmed { get; private set; }
        /// <summary>预转入已确认
        /// </summary>
        public bool IsTransferInPreparationConfirmed { get; private set; }
        /// <summary>转出已确认
        /// </summary>
        public bool IsTransferOutConfirmed { get; private set; }
        /// <summary>转入已确认
        /// </summary>
        public bool IsTransferInConfirmed { get; private set; }
        /// <summary>取消转出已确认
        /// </summary>
        public bool IsCancelTransferOutConfirmed { get; private set; }
        /// <summary>取消转入已确认
        /// </summary>
        public bool IsCancelTransferInConfirmed { get; private set; }
        /// <summary>交易状态
        /// </summary>
        public TransactionStatus Status { get; private set; }

        #endregion

        #region Constructors

        /// <summary>构造函数
        /// </summary>
        /// <param name="transactionInfo"></param>
        public TransferTransaction(TransferTransactionInfo transactionInfo) : base(transactionInfo.TransactionId)
        {
            RaiseEvent(new TransferTransactionStartedEvent(transactionInfo, DateTime.Now));
        }

        #endregion

        #region Public Methods

        /// <summary>确认预转出
        /// </summary>
        public void ConfirmTransferOutPreparation()
        {
            if (Status == TransactionStatus.Started)
            {
                if (!IsTransferOutPreparationConfirmed)
                {
                    RaiseEvent(new TransferOutPreparationConfirmedEvent(Id, TransactionInfo, DateTime.Now));
                    if (IsTransferInPreparationConfirmed)
                    {
                        RaiseEvent(new TransferTransactionConfirmedEvent(Id, TransactionInfo, DateTime.Now));
                    }
                }
            }
        }
        /// <summary>确认预转入
        /// </summary>
        public void ConfirmTransferInPreparation()
        {
            if (Status == TransactionStatus.Started)
            {
                if (!IsTransferInPreparationConfirmed)
                {
                    RaiseEvent(new TransferInPreparationConfirmedEvent(Id, TransactionInfo, DateTime.Now));
                    if (IsTransferOutPreparationConfirmed)
                    {
                        RaiseEvent(new TransferTransactionConfirmedEvent(Id, TransactionInfo, DateTime.Now));
                    }
                }
            }
        }
        /// <summary>确认转出
        /// </summary>
        public void ConfirmTransferOut()
        {
            if (Status == TransactionStatus.PreparationConfirmed)
            {
                if (!IsTransferOutConfirmed)
                {
                    RaiseEvent(new TransferOutConfirmedEvent(Id, TransactionInfo, DateTime.Now));
                    if (IsTransferInConfirmed)
                    {
                        RaiseEvent(new TransferTransactionCompletedEvent(Id, TransactionInfo, DateTime.Now));
                    }
                }
            }
        }
        /// <summary>确认转入
        /// </summary>
        public void ConfirmTransferIn()
        {
            if (Status == TransactionStatus.PreparationConfirmed)
            {
                if (!IsTransferInConfirmed)
                {
                    RaiseEvent(new TransferInConfirmedEvent(Id, TransactionInfo, DateTime.Now));
                    if (IsTransferOutConfirmed)
                    {
                        RaiseEvent(new TransferTransactionCompletedEvent(Id, TransactionInfo, DateTime.Now));
                    }
                }
            }
        }
        /// <summary>开始取消转账交易
        /// </summary>
        public void StartCancel()
        {
            if (Status == TransactionStatus.Started)
            {
                RaiseEvent(new TransferTransactionCancelStartedEvent(Id, TransactionInfo));
            }
        }
        /// <summary>确认转出操作已取消
        /// </summary>
        public void ConfirmTransferOutCanceled()
        {
            if (!IsCancelTransferOutConfirmed)
            {
                RaiseEvent(new TransferOutCanceledConfirmedEvent(Id, TransactionInfo, DateTime.Now));
                if (IsCancelTransferInConfirmed)
                {
                    RaiseEvent(new TransferTransactionCanceledEvent(Id, TransactionInfo, DateTime.Now));
                }
            }
        }
        /// <summary>确认转入操作已取消
        /// </summary>
        public void ConfirmTransferInCanceled()
        {
            if (!IsCancelTransferInConfirmed)
            {
                RaiseEvent(new TransferInCanceledConfirmedEvent(Id, TransactionInfo, DateTime.Now));
                if (IsCancelTransferOutConfirmed)
                {
                    RaiseEvent(new TransferTransactionCanceledEvent(Id, TransactionInfo, DateTime.Now));
                }
            }
        }

        #endregion

        #region Handler Methods

        private void Handle(TransferTransactionStartedEvent evnt)
        {
            Id = evnt.AggregateRootId;
            TransactionInfo = evnt.TransactionInfo;
            StartedTime = evnt.StartedTime;
            Status = TransactionStatus.Started;
        }
        private void Handle(TransferOutPreparationConfirmedEvent evnt)
        {
            IsTransferOutPreparationConfirmed = true;
        }
        private void Handle(TransferInPreparationConfirmedEvent evnt)
        {
            IsTransferInPreparationConfirmed = true;
        }
        private void Handle(TransferOutConfirmedEvent evnt)
        {
            IsTransferOutConfirmed = true;
        }
        private void Handle(TransferInConfirmedEvent evnt)
        {
            IsTransferInConfirmed = true;
        }
        private void Handle(TransferTransactionConfirmedEvent evnt)
        {
            Status = TransactionStatus.PreparationConfirmed;
        }
        private void Handle(TransferTransactionCompletedEvent evnt)
        {
            Status = TransactionStatus.Completed;
        }
        private void Handle(TransferTransactionCancelStartedEvent evnt)
        {
            Status = TransactionStatus.CancelStarted;
        }
        private void Handle(TransferOutCanceledConfirmedEvent evnt)
        {
            IsCancelTransferOutConfirmed = true;
        }
        private void Handle(TransferInCanceledConfirmedEvent evnt)
        {
            IsCancelTransferInConfirmed = true;
        }
        private void Handle(TransferTransactionCanceledEvent evnt)
        {
            Status = TransactionStatus.Canceled;
        }

        #endregion
    }
}
