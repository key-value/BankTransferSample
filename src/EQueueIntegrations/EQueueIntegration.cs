﻿using System.Linq;
using System.Threading;
using ECommon.IoC;
using ECommon.Scheduling;
using ENode.Commanding;
using ENode.Configurations;
using ENode.EQueue;
using ENode.EQueue.Commanding;
using ENode.Eventing;
using EQueue.Broker;
using EQueue.Clients.Consumers;
using EQueue.Configurations;

namespace BankTransferSample.EQueueIntegrations
{
    public static class EQueueIntegration
    {
        private static BrokerController _broker;
        private static CommandService _commandService;
        private static CommandConsumer _commandConsumer;
        private static EventPublisher _eventPublisher;
        private static EventConsumer _eventConsumer;
        private static CompletedCommandProcessor _completedCommandProcessor;

        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();
            configuration.SetDefault<ICommandTopicProvider, CommandTopicManager>();
            configuration.SetDefault<IEventTopicProvider, EventTopicManager>();
            configuration.SetDefault<ICommandTypeCodeProvider, CommandTypeCodeManager>();
            configuration.SetDefault<IEventTypeCodeProvider, EventTypeCodeManager>();

            var consumerSetting = new ConsumerSetting
            {
                HeartbeatBrokerInterval = 1000,
                UpdateTopicQueueCountInterval = 1000,
                RebalanceInterval = 1000
            };
            var eventConsumerSetting = new ConsumerSetting
            {
                HeartbeatBrokerInterval = 1000,
                UpdateTopicQueueCountInterval = 1000,
                RebalanceInterval = 1000,
                MessageHandleMode = MessageHandleMode.Sequential
            };

            _broker = new BrokerController().Initialize();
            _completedCommandProcessor = new CompletedCommandProcessor(consumerSetting);

            configuration.SetDefault<CompletedCommandProcessor, CompletedCommandProcessor>(_completedCommandProcessor);

            _commandService = new CommandService();
            _eventPublisher = new EventPublisher();

            configuration.SetDefault<ICommandService, CommandService>(_commandService);
            configuration.SetDefault<IEventPublisher, EventPublisher>(_eventPublisher);

            _commandConsumer = new CommandConsumer(consumerSetting);
            _eventConsumer = new EventConsumer(eventConsumerSetting);

            _commandConsumer.Subscribe("BankTransferCommandTopic");
            _eventConsumer.Subscribe("BankTransferEventTopic");
            _completedCommandProcessor.Subscribe("BankTransferEventTopic");

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _broker.Start();
            _eventConsumer.Start();
            _commandConsumer.Start();
            _eventPublisher.Start();
            _commandService.Start();
            _completedCommandProcessor.Start();

            WaitAllConsumerLoadBalanceComplete();

            return enodeConfiguration;
        }

        private static void WaitAllConsumerLoadBalanceComplete()
        {
            var scheduleService = ObjectContainer.Resolve<IScheduleService>();
            var waitHandle = new ManualResetEvent(false);
            var taskId = scheduleService.ScheduleTask(() =>
            {
                var eventConsumerAllocatedQueues = _eventConsumer.Consumer.GetCurrentQueues();
                var commandConsumerAllocatedQueues = _commandConsumer.Consumer.GetCurrentQueues();
                var completedCommandProcessorAllocatedQueues = _completedCommandProcessor.Consumer.GetCurrentQueues();
                if (eventConsumerAllocatedQueues.Count() == 4 && commandConsumerAllocatedQueues.Count() == 4 && completedCommandProcessorAllocatedQueues.Count() == 4)
                {
                    waitHandle.Set();
                }
            }, 1000, 1000);

            waitHandle.WaitOne();
            scheduleService.ShutdownTask(taskId);
        }
    }
}
