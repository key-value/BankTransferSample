using System;
using System.Reflection;
using System.Threading.Tasks;
using BankTransferSample.Commands;
using BankTransferSample.Domain;
using BankTransferSample.DomainEvents;
using BankTransferSample.EQueueIntegrations;
using ECommon.Autofac;
using ECommon.Configurations;
using ECommon.IoC;
using ECommon.JsonNet;
using ECommon.Log4Net;
using ECommon.ProtocolBuf;
using ECommon.Utilities;
using ENode.Commanding;
using ENode.Configurations;
using ENode.Domain;
using ENode.EQueue;
using ENode.Eventing;
using EQueue.Clients.Producers;
using EQueue.Protocols;
using EQueue.Utils;
using ProtoBuf.Meta;

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
            var task1 = commandService.Execute(new CreateAccountCommand("00001", "雪华"));
            var task2 = commandService.Execute(new CreateAccountCommand("00002", "凯锋"));
            Task.WaitAll(task1, task2);

            Console.WriteLine(string.Empty);

            //每个账户都存入1000元
            commandService.Execute(new DepositCommand("00001", 1000)).Wait();
            commandService.Execute(new DepositCommand("00002", 1000)).Wait();

            Console.WriteLine(string.Empty);

            //账户1向账户2转账300元
            commandService.StartProcess(new StartTransactionCommand(new TransactionInfo(ObjectId.GenerateNewStringId(), "00001", "00002", 300D))).Wait();
            Console.WriteLine(string.Empty);

            //账户2向账户1转账500元
            commandService.StartProcess(new StartTransactionCommand(new TransactionInfo(ObjectId.GenerateNewStringId(), "00002", "00001", 500D))).Wait();
            Console.WriteLine(string.Empty);

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        static void InitializeENodeFramework()
        {
            ConfigProtobufMetaData();

            var assemblies = new[] { Assembly.GetExecutingAssembly() };

            Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .UseProtoBufSerializer()
                .CreateENode()
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseEQueue()
                .InitializeENode(assemblies)
                .StartEQueue()
                .StartEnode();
        }

        static void ConfigProtobufMetaData()
        {
            var model = RuntimeTypeModel.Default;

            //Config equeue classes.
            model.Add(typeof(TypeData<string>), false).Add("TypeCode", "Data").UseConstructor = false;
            model.Add(typeof(TypeData<byte[]>), false).Add("TypeCode").UseConstructor = false;
            model[typeof(TypeData<string>)].AddSubType(10, typeof(StringTypeData)).UseConstructor = false;
            model[typeof(TypeData<byte[]>)].AddSubType(10, typeof(ByteTypeData)).UseConstructor = false;

            model.Add(typeof(ConsumerData), false).Add("ConsumerId", "GroupName", "SubscriptionTopics").UseConstructor = false;
            model.Add(typeof(Message), false).Add("Topic").UseConstructor = false;
            model[typeof(Message)].AddSubType(10, typeof(QueueMessage)).UseConstructor = false;
            model.Add(typeof(QueueMessage), false).Add("MessageOffset", "QueueId", "QueueOffset", "StoredTime").UseConstructor = false;
            model.Add(typeof(MessageQueue), false).Add("Topic", "QueueId").UseConstructor = false;
            model.Add(typeof(PullMessageRequest), false).Add("ConsumerGroup", "MessageQueue", "QueueOffset", "PullMessageBatchSize").UseConstructor = false;
            model.Add(typeof(PullMessageResponse), false).Add("Messages").UseConstructor = false;
            model.Add(typeof(QueryConsumerRequest), false).Add("GroupName", "Topic").UseConstructor = false;
            model.Add(typeof(SendMessageRequest), false).Add("QueueId", "Message").UseConstructor = false;
            model.Add(typeof(SendMessageResponse), false).Add("MessageOffset", "MessageQueue", "QueueOffset").UseConstructor = false;
            model.Add(typeof(SendResult), false).Add("SendStatus", "ErrorMessage", "MessageQueue", "QueueOffset", "MessageOffset").UseConstructor = false;

            model.Add(typeof(CommandMessage), false).Add("CommandData", "CommandExecutedMessageTopic", "DomainEventHandledMessageTopic").UseConstructor = false;
            model.Add(typeof(CommandExecutedMessage), false).Add("CommandId", "AggregateRootId", "ProcessId", "CommandStatus", "ExceptionCode", "ErrorMessage").UseConstructor = false;
            model.Add(typeof(DomainEventHandledMessage), false).Add("CommandId", "AggregateRootId", "IsProcessCompletedEvent", "ProcessId").UseConstructor = false;
            model.Add(typeof(EventMessage), false).Add("CommandId", "AggregateRootId", "AggregateRootName", "Version", "Timestamp", "Events", "ContextItems").UseConstructor = false;

            //Config enode base classes.
            model.Add(typeof(AggregateRoot<string>), false).Add("_id", "_uniqueId", "_version").UseConstructor = false;

            model.Add(typeof(Command<string>), false).Add("Id", "RetryCount", "AggregateRootId").UseConstructor = false;
            model.Add(typeof(ProcessCommand<string>), false).Add("_processId").UseConstructor = false;

            model.Add(typeof(DomainEvent<string>), false).Add("Id", "AggregateRootId").UseConstructor = false;

            model[typeof(Command<string>)].AddSubType(10, typeof(ProcessCommand<string>)).UseConstructor = false;

            //Config project related classes.
            model[typeof(AggregateRoot<string>)].AddSubType(100, typeof(BankAccount)).UseConstructor = false;
            model[typeof(AggregateRoot<string>)].AddSubType(101, typeof(Transaction)).UseConstructor = false;

            model[typeof(Command<string>)].AddSubType(100, typeof(CreateAccountCommand)).UseConstructor = false;
            model[typeof(Command<string>)].AddSubType(101, typeof(DepositCommand)).UseConstructor = false;
            model[typeof(Command<string>)].AddSubType(102, typeof(WithdrawCommand)).UseConstructor = false;

            model[typeof(ProcessCommand<string>)].AddSubType(100, typeof(PrepareDebitCommand)).UseConstructor = false;
            model[typeof(ProcessCommand<string>)].AddSubType(101, typeof(PrepareCreditCommand)).UseConstructor = false;
            model[typeof(ProcessCommand<string>)].AddSubType(102, typeof(CommitDebitCommand)).UseConstructor = false;
            model[typeof(ProcessCommand<string>)].AddSubType(103, typeof(CommitCreditCommand)).UseConstructor = false;
            model[typeof(ProcessCommand<string>)].AddSubType(104, typeof(AbortDebitCommand)).UseConstructor = false;
            model[typeof(ProcessCommand<string>)].AddSubType(105, typeof(AbortCreditCommand)).UseConstructor = false;

            model[typeof(ProcessCommand<string>)].AddSubType(106, typeof(StartTransactionCommand)).UseConstructor = false;
            model[typeof(ProcessCommand<string>)].AddSubType(107, typeof(ConfirmDebitPreparationCommand)).UseConstructor = false;
            model[typeof(ProcessCommand<string>)].AddSubType(108, typeof(ConfirmCreditPreparationCommand)).UseConstructor = false;
            model[typeof(ProcessCommand<string>)].AddSubType(109, typeof(ConfirmDebitCommand)).UseConstructor = false;
            model[typeof(ProcessCommand<string>)].AddSubType(110, typeof(ConfirmCreditCommand)).UseConstructor = false;
            model[typeof(ProcessCommand<string>)].AddSubType(111, typeof(AbortTransactionCommand)).UseConstructor = false;

            model[typeof(DomainEvent<string>)].AddSubType(100, typeof(CreditPreparationNotExistEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(101, typeof(DebitInsufficientBalanceEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(102, typeof(DebitPreparationNotExistEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(103, typeof(DuplicatedCreditPreparationEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(104, typeof(DuplicatedDebitPreparationEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(105, typeof(WithdrawInsufficientBalanceEvent)).UseConstructor = false;

            model[typeof(DomainEvent<string>)].AddSubType(106, typeof(AccountCreatedEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(107, typeof(CreditAbortedEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(108, typeof(CreditCommittedEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(109, typeof(CreditPreparedEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(110, typeof(DebitAbortedEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(111, typeof(DebitCommittedEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(112, typeof(DebitPreparedEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(113, typeof(DepositedEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(114, typeof(WithdrawnEvent)).UseConstructor = false;

            model[typeof(DomainEvent<string>)].AddSubType(115, typeof(CreditConfirmedEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(116, typeof(CreditPreparationConfirmedEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(117, typeof(DebitConfirmedEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(118, typeof(DebitPreparationConfirmedEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(119, typeof(TransactionAbortedEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(120, typeof(TransactionCommittedEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(121, typeof(TransactionCompletedEvent)).UseConstructor = false;
            model[typeof(DomainEvent<string>)].AddSubType(122, typeof(TransactionStartedEvent)).UseConstructor = false;

            model.Add(typeof(BankAccount), false).Add("_debitPreparations", "_creditPreparations", "Owner", "Balance").UseConstructor = false;
            model.Add(typeof(CreditPreparation), false).Add("TransactionId", "Amount").UseConstructor = false;
            model.Add(typeof(DebitPreparation), false).Add("TransactionId", "Amount").UseConstructor = false;
            model.Add(typeof(Transaction), false).Add("TransactionInfo", "StartedTime", "DebitPreparationConfirmed", "CreditPreparationConfirmed", "DebitConfirmed", "CreditConfirmed", "Status").UseConstructor = false;
            model.Add(typeof(TransactionInfo), false).Add("TransactionId", "SourceAccountId", "TargetAccountId", "Amount").UseConstructor = false;

            model.Add(typeof(CreateAccountCommand), false).Add("Owner").UseConstructor = false;
            model.Add(typeof(DepositCommand), false).Add("Amount").UseConstructor = false;
            model.Add(typeof(WithdrawCommand), false).Add("Amount").UseConstructor = false;
            model.Add(typeof(PrepareDebitCommand), false).Add("TransactionId", "Amount").UseConstructor = false;
            model.Add(typeof(PrepareCreditCommand), false).Add("TransactionId", "Amount").UseConstructor = false;
            model.Add(typeof(CommitDebitCommand), false).Add("TransactionId").UseConstructor = false;
            model.Add(typeof(CommitCreditCommand), false).Add("TransactionId").UseConstructor = false;
            model.Add(typeof(AbortDebitCommand), false).Add("TransactionId").UseConstructor = false;
            model.Add(typeof(AbortCreditCommand), false).Add("TransactionId").UseConstructor = false;

            model.Add(typeof(StartTransactionCommand), false).Add("TransactionInfo").UseConstructor = false;
            model.Add(typeof(ConfirmDebitPreparationCommand), false).UseConstructor = false;
            model.Add(typeof(ConfirmCreditPreparationCommand), false).UseConstructor = false;
            model.Add(typeof(ConfirmDebitCommand), false).UseConstructor = false;
            model.Add(typeof(ConfirmCreditCommand), false).UseConstructor = false;
            model.Add(typeof(AbortTransactionCommand), false).UseConstructor = false;

            model.Add(typeof(AccountCreatedEvent), false).Add("Owner", "CreatedTime").UseConstructor = false;
            model.Add(typeof(CreditAbortedEvent), false).Add("TransactionId", "Amount", "AbortedTime").UseConstructor = false;
            model.Add(typeof(CreditCommittedEvent), false).Add("TransactionId", "Amount", "CurrentBalance", "TransactionTime").UseConstructor = false;
            model.Add(typeof(CreditPreparationNotExistEvent), false).Add("TransactionId").UseConstructor = false;
            model.Add(typeof(CreditPreparedEvent), false).Add("TransactionId", "Amount").UseConstructor = false;
            model.Add(typeof(DebitAbortedEvent), false).Add("TransactionId", "Amount", "AbortedTime").UseConstructor = false;
            model.Add(typeof(DebitCommittedEvent), false).Add("TransactionId", "Amount", "CurrentBalance", "TransactionTime").UseConstructor = false;
            model.Add(typeof(DebitInsufficientBalanceEvent), false).Add("TransactionId", "Amount", "CurrentBalance", "CurrentAvailableBalance").UseConstructor = false;
            model.Add(typeof(DebitPreparationNotExistEvent), false).Add("TransactionId").UseConstructor = false;
            model.Add(typeof(DebitPreparedEvent), false).Add("TransactionId", "Amount").UseConstructor = false;
            model.Add(typeof(DepositedEvent), false).Add("Amount", "CurrentBalance", "TransactionTime").UseConstructor = false;
            model.Add(typeof(DuplicatedCreditPreparationEvent), false).Add("TransactionId").UseConstructor = false;
            model.Add(typeof(DuplicatedDebitPreparationEvent), false).Add("TransactionId").UseConstructor = false;
            model.Add(typeof(WithdrawInsufficientBalanceEvent), false).Add("Amount", "CurrentBalance", "CurrentAvailableBalance").UseConstructor = false;
            model.Add(typeof(WithdrawnEvent), false).Add("Amount", "CurrentBalance", "TransactionTime").UseConstructor = false;

            model.Add(typeof(CreditPreparationConfirmedEvent), false).Add("TransactionInfo", "ConfirmedTime").UseConstructor = false;
            model.Add(typeof(DebitPreparationConfirmedEvent), false).Add("TransactionInfo", "ConfirmedTime").UseConstructor = false;
            model.Add(typeof(CreditConfirmedEvent), false).Add("TransactionInfo", "ConfirmedTime").UseConstructor = false;
            model.Add(typeof(DebitConfirmedEvent), false).Add("TransactionInfo", "ConfirmedTime").UseConstructor = false;

            model.Add(typeof(TransactionStartedEvent), false).Add("TransactionInfo", "StartedTime").UseConstructor = false;
            model.Add(typeof(TransactionCommittedEvent), false).Add("TransactionInfo", "CommittedTime").UseConstructor = false;
            model.Add(typeof(TransactionCompletedEvent), false).Add("TransactionInfo", "CompletedTime").UseConstructor = false;
            model.Add(typeof(TransactionAbortedEvent), false).Add("TransactionInfo", "AbortedTime").UseConstructor = false;
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
        public void Handle(AccountCreatedEvent evnt)
        {
            Console.WriteLine("账号已创建，账号：{0}，所有者：{1}", evnt.AggregateRootId, evnt.Owner);
        }
        public void Handle(DepositedEvent evnt)
        {
            Console.WriteLine("存款已成功，账号：{0}，金额：{1}，当前余额：{2}", evnt.AggregateRootId, evnt.Amount, evnt.CurrentBalance);
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
        }
        public void Handle(TransactionAbortedEvent evnt)
        {
            Console.WriteLine("交易已终止，交易ID：{0}", evnt.AggregateRootId);
        }
    }
}
