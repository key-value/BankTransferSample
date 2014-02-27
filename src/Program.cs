using System;
using System.Reflection;
using BankTransferSample.Commands;
using BankTransferSample.Domain;
using BankTransferSample.DomainEvents;
using BankTransferSample.EQueueIntegrations;
using ECommon.Autofac;
using ECommon.Configurations;
using ECommon.IoC;
using ECommon.JsonNet;
using ECommon.Log4Net;
using ECommon.Utilities;
using ENode.Commanding;
using ENode.Configurations;
using ENode.Eventing;

namespace BankTransferSample
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeENodeFramework();

            var commandService = ObjectContainer.Resolve<ICommandService>();

            Console.WriteLine(string.Empty);

            //创建两个银行账户
            commandService.Execute(new CreateAccountCommand("00001", "雪华")).Wait();
            commandService.Execute(new CreateAccountCommand("00002", "凯锋")).Wait();

            Console.WriteLine(string.Empty);

            //每个账户都存入1000元
            commandService.StartProcess(new StartDepositTransactionCommand(ObjectId.GenerateNewStringId(), "00001", 1000)).Wait();
            commandService.StartProcess(new StartDepositTransactionCommand(ObjectId.GenerateNewStringId(), "00002", 1000)).Wait();

            Console.WriteLine(string.Empty);

            //账户1向账户2转账300元
            commandService.StartProcess(new StartTransferTransactionCommand(new TransferTransactionInfo(ObjectId.GenerateNewStringId(), "00001", "00002", 300D))).Wait();
            Console.WriteLine(string.Empty);

            //账户2向账户1转账500元
            commandService.StartProcess(new StartTransferTransactionCommand(new TransferTransactionInfo(ObjectId.GenerateNewStringId(), "00002", "00001", 500D))).Wait();
            Console.WriteLine(string.Empty);

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        static void InitializeENodeFramework()
        {
            var assemblies = new[] { Assembly.GetExecutingAssembly() };

            Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .CreateENode()
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseEQueue()
                .InitializeENode(assemblies)
                .StartEQueue()
                .StartEnode();
        }
    }

    #region Event Logger

    [Component]
    public class BasicTestEventLogger :
        IEventHandler<AccountCreatedEvent>,
        IEventHandler<TransactionPreparationAddedEvent>,
        IEventHandler<TransactionPreparationCommittedEvent>,
        IEventHandler<TransferTransactionStartedEvent>,
        IEventHandler<TransferOutPreparationConfirmedEvent>,
        IEventHandler<TransferInPreparationConfirmedEvent>,
        IEventHandler<TransferTransactionCommittedEvent>,
        IEventHandler<TransferTransactionCompletedEvent>
    {
        public void Handle(AccountCreatedEvent evnt)
        {
            Console.WriteLine("账户已创建，账户：{0}，所有者：{1}", evnt.AggregateRootId, evnt.Owner);
        }
        public void Handle(TransactionPreparationAddedEvent evnt)
        {
            if (evnt.TransactionPreparation.TransactionType == TransactionType.TransferTransaction)
            {
                if (evnt.TransactionPreparation.PreparationType == PreparationType.DebitPreparation)
                {
                    Console.WriteLine("账户预转出成功，交易ID：{0}，账户：{1}，金额：{2}", evnt.TransactionPreparation.TransactionId, evnt.TransactionPreparation.AccountId, evnt.TransactionPreparation.Amount);
                }
                else if (evnt.TransactionPreparation.PreparationType == PreparationType.CreditPreparation)
                {
                    Console.WriteLine("账户预转入成功，交易ID：{0}，账户：{1}，金额：{2}", evnt.TransactionPreparation.TransactionId, evnt.TransactionPreparation.AccountId, evnt.TransactionPreparation.Amount);
                }
            }
        }
        public void Handle(TransactionPreparationCommittedEvent evnt)
        {
            if (evnt.TransactionPreparation.TransactionType == TransactionType.DepositTransaction)
            {
                if (evnt.TransactionPreparation.PreparationType == PreparationType.CreditPreparation)
                {
                    Console.WriteLine("账户存款已成功，账户：{0}，金额：{1}，当前余额：{2}", evnt.TransactionPreparation.AccountId, evnt.TransactionPreparation.Amount, evnt.CurrentBalance);
                }
            }
            if (evnt.TransactionPreparation.TransactionType == TransactionType.TransferTransaction)
            {
                if (evnt.TransactionPreparation.PreparationType == PreparationType.DebitPreparation)
                {
                    Console.WriteLine("账户转出已成功，交易ID：{0}，账户：{1}，金额：{2}，当前余额：{3}", evnt.TransactionPreparation.TransactionId, evnt.TransactionPreparation.AccountId, evnt.TransactionPreparation.Amount, evnt.CurrentBalance);
                }
                if (evnt.TransactionPreparation.PreparationType == PreparationType.CreditPreparation)
                {
                    Console.WriteLine("账户转入已成功，交易ID：{0}，账户：{1}，金额：{2}，当前余额：{3}", evnt.TransactionPreparation.TransactionId, evnt.TransactionPreparation.AccountId, evnt.TransactionPreparation.Amount, evnt.CurrentBalance);
                }
            }
        }

        public void Handle(TransferTransactionStartedEvent evnt)
        {
            Console.WriteLine("转账交易已开始，交易ID：{0}，源账户：{1}，目标账户：{2}，转账金额：{3}", evnt.AggregateRootId, evnt.TransactionInfo.SourceAccountId, evnt.TransactionInfo.TargetAccountId, evnt.TransactionInfo.Amount);
        }
        public void Handle(TransferOutPreparationConfirmedEvent evnt)
        {
            Console.WriteLine("预转出确认成功，交易ID：{0}，账户：{1}", evnt.AggregateRootId, evnt.TransactionInfo.SourceAccountId);
        }
        public void Handle(TransferInPreparationConfirmedEvent evnt)
        {
            Console.WriteLine("预转入确认成功，交易ID：{0}，账户：{1}", evnt.AggregateRootId, evnt.TransactionInfo.TargetAccountId);
        }
        public void Handle(TransferTransactionCommittedEvent evnt)
        {
            Console.WriteLine("转账交易已提交，交易ID：{0}", evnt.AggregateRootId);
        }
        public void Handle(TransferTransactionCompletedEvent evnt)
        {
            Console.WriteLine("转账交易已完成，交易ID：{0}", evnt.AggregateRootId);
        }
    }

    #endregion
}
