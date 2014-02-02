using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BankTransferSample.Commands;
using BankTransferSample.DomainEvents;
using BankTransferSample.EQueueIntegrations;
using ECommon.Autofac;
using ECommon.Configurations;
using ECommon.IoC;
using ECommon.JsonNet;
using ECommon.Log4Net;
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
            var task1 = commandService.Send(new CreateAccountCommand("00001", "雪华"));
            var task2 = commandService.Send(new CreateAccountCommand("00002", "凯锋"));
            Task.WaitAll(task1, task2);

            //每个账户都存入1000元
            task1 = commandService.Send(new DepositCommand("00001", 1000));
            task2 = commandService.Send(new DepositCommand("00002", 1000));
            Task.WaitAll(task1, task2);

            //账户1向账户2转账300元
            commandService.Send(new CreateTransactionCommand(new TransactionInfo(Guid.NewGuid(), "00001", "00002", 300D))).Wait();
            //账户2向账户1转账500元
            commandService.Send(new CreateTransactionCommand(new TransactionInfo(Guid.NewGuid(), "00002", "00001", 500D))).Wait();

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

    [Component]
    public class BasicTestEventLogger :
        IEventHandler<AccountCreatedEvent>,                      //账号已创建
        IEventHandler<DepositedEvent>,                           //已存款
        IEventHandler<WithdrawnEvent>,                           //已取款
        IEventHandler<TransactionStartedEvent>,                  //交易已开始
        IEventHandler<DebitPreparedEvent>,                       //交易已预转出
        IEventHandler<CreditPreparedEvent>,                      //交易已预转入
        IEventHandler<DebitInsufficientBalanceEvent>,            //余额不足不允许预转出操作
        IEventHandler<DebitPreparationConfirmedEvent>,           //交易预转出已确认
        IEventHandler<CreditPreparationConfirmedEvent>,          //交易预转入已确认
        IEventHandler<TransactionCommittedEvent>,                //交易已提交
        IEventHandler<DebitCommittedEvent>,                      //交易转出已提交
        IEventHandler<CreditCommittedEvent>,                     //交易转入已提交
        IEventHandler<DebitAbortedEvent>,                        //交易转出已终止
        IEventHandler<CreditAbortedEvent>,                       //交易转入已终止
        IEventHandler<DebitConfirmedEvent>,                      //交易转出已确认
        IEventHandler<CreditConfirmedEvent>,                     //交易转入已确认
        IEventHandler<TransactionCompletedEvent>,                //交易已完成
        IEventHandler<TransactionAbortedEvent>                   //交易已终止
    {
        private static int _accountCreatedCount = 0;
        private static int _depositedCount = 0;
        private static int _transactionCompletedCount = 0;

        public void Handle(AccountCreatedEvent evnt)
        {
            Console.WriteLine("账号已创建，账号：{0}，所有者：{1}", evnt.AggregateRootId, evnt.Owner);
            if (Interlocked.Increment(ref _accountCreatedCount) == 2)
            {
                Console.WriteLine(string.Empty);
            }
        }
        public void Handle(DepositedEvent evnt)
        {
            Console.WriteLine("存款已成功，账号：{0}，金额：{1}，当前余额：{2}", evnt.AggregateRootId, evnt.Amount, evnt.CurrentBalance);
            if (Interlocked.Increment(ref _depositedCount) == 2)
            {
                Console.WriteLine(string.Empty);
            }
        }
        public void Handle(WithdrawnEvent evnt)
        {
            Console.WriteLine("取款已成功，账号：{0}，金额：{1}，当前余额：{2}", evnt.AggregateRootId, evnt.Amount, evnt.CurrentBalance);
        }
        public void Handle(TransactionStartedEvent evnt)
        {
            Console.WriteLine("交易已开始，交易ID：{0}，源账号：{1}，目标账号：{2}，转账金额：{3}", evnt.AggregateRootId, evnt.TransactionInfo.SourceAccountId, evnt.TransactionInfo.TargetAccountId, evnt.TransactionInfo.Amount);
        }
        public void Handle(DebitPreparedEvent evnt)
        {
            Console.WriteLine("交易预转出成功，交易ID：{0}，账号：{1}，金额：{2}", evnt.TransactionId, evnt.AggregateRootId, evnt.Amount);
        }
        public void Handle(CreditPreparedEvent evnt)
        {
            Console.WriteLine("交易预转入成功，交易ID：{0}，账号：{1}，金额：{2}", evnt.TransactionId, evnt.AggregateRootId, evnt.Amount);
        }
        public void Handle(DebitInsufficientBalanceEvent evnt)
        {
            Console.WriteLine("余额不足不允许预转出操作，交易ID：{0}，账号：{1}，金额：{2}，当前余额：{3}，当前可用余额：{4}", evnt.TransactionId, evnt.AggregateRootId, evnt.Amount, evnt.CurrentBalance, evnt.CurrentAvailableBalance);
        }
        public void Handle(DebitPreparationConfirmedEvent evnt)
        {
            Console.WriteLine("交易预转出确认成功，交易ID：{0}", evnt.AggregateRootId);
        }
        public void Handle(CreditPreparationConfirmedEvent evnt)
        {
            Console.WriteLine("交易预转入确认成功，交易ID：{0}", evnt.AggregateRootId);
        }
        public void Handle(TransactionCommittedEvent evnt)
        {
            Console.WriteLine("交易已提交，交易ID：{0}", evnt.AggregateRootId);
        }
        public void Handle(DebitCommittedEvent evnt)
        {
            Console.WriteLine("交易转出已提交，交易ID：{0}，账号：{1}，金额：{2}，当前余额：{3}", evnt.TransactionId, evnt.AggregateRootId, evnt.Amount, evnt.CurrentBalance);
        }
        public void Handle(CreditCommittedEvent evnt)
        {
            Console.WriteLine("交易转入已提交，交易ID：{0}，账号：{1}，金额：{2}，当前余额：{3}", evnt.TransactionId, evnt.AggregateRootId, evnt.Amount, evnt.CurrentBalance);
        }
        public void Handle(DebitAbortedEvent evnt)
        {
            Console.WriteLine("交易转出已终止，交易ID：{0}，账号：{1}，金额：{2}", evnt.TransactionId, evnt.AggregateRootId, evnt.Amount);
        }
        public void Handle(CreditAbortedEvent evnt)
        {
            Console.WriteLine("交易转入已终止，交易ID：{0}，账号：{1}，金额：{2}", evnt.TransactionId, evnt.AggregateRootId, evnt.Amount);
        }
        public void Handle(DebitConfirmedEvent evnt)
        {
            Console.WriteLine("交易转出确认成功，交易ID：{0}", evnt.AggregateRootId);
        }
        public void Handle(CreditConfirmedEvent evnt)
        {
            Console.WriteLine("交易转入确认成功，交易ID：{0}", evnt.AggregateRootId);
        }
        public void Handle(TransactionCompletedEvent evnt)
        {
            Console.WriteLine("交易已完成，交易ID：{0}", evnt.AggregateRootId);
            Console.WriteLine(string.Empty);

            if (Interlocked.Increment(ref _transactionCompletedCount) == 2)
            {
                Console.WriteLine("Press Enter to exit...");
            }
        }
        public void Handle(TransactionAbortedEvent evnt)
        {
            Console.WriteLine("交易已终止，交易ID：{0}", evnt.AggregateRootId);
            Console.WriteLine(string.Empty);
        }
    }
}
