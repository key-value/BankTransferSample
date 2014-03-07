﻿using System;
using System.Reflection;
using BankTransferSample.Commands;
using BankTransferSample.Domain;
using BankTransferSample.DomainEvents;
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
                .SetProviders()
                .RegisterBusinessComponents(assemblies)
                .UseEQueue()
                .InitializeBusinessAssemblies(assemblies)
                .StartRetryCommandService()
                .StartWaitingCommandService()
                .StartEQueue();

            Console.WriteLine("ENode framework started...");
        }
    }
}
