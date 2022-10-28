using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoFac_practice_AdvancedRegistrationConcepts
{
    // This file contains practice related to Section 3: Advanced Registration Concepts
    public class AdvancedRegistrationConcepts
    {

        public static void RegisterDependencies()
        {

            var builder = new ContainerBuilder();

            // We have a SMS log class. Whose constructor receives a phone number argument we have to pass that argument at the time of registration.
            // There are multiple ways to do it which are mentioned below.

            // 1: named Parameter Option, WithParameter Method In which first argument is the parameter name which is receiving by contructor and the second parameter 
            // is the value.

            //builder.RegisterType<SMSLog>()
            //       .As<ILog>()
            //       .WithParameter("phoneNumber","+23456909090");

            // 2: typed Parameter Option, We will use WithParameter Method overload In which first argument is the type of parameter 
            //    which is receiving by contructor and the second parameter 
            //    is the value.

            //builder.RegisterType<SMSLog>()
            //    .As<ILog>()
            //    .WithParameter(new TypedParameter(typeof(string), "+23456909090"));

            // 3: Resolved Parameter Option, We will use WithParameter Method overload In which first argument is predicate which evaluates the parameter 
            //    which is receiving by contructor and the second parameter 
            //    is the value.

            builder.RegisterType<SMSLog>()
                .As<ILog>()
                .WithParameter(
                   new ResolvedParameter(
                       // Predicate 
                       (pi, ctx) => pi.ParameterType == typeof(string) && pi.Name == "phoneNumber",
                       // Value accessor
                       (pi, ctx) => "+23456909090"
                       )
                );


            Console.WriteLine("About to build container...");
            var container = builder.Build();
            var logObject = container.Resolve<ILog>();

            logObject.Write("Message to phone number");

            //log.Write("Testing");
        }

    }



    public interface ILog
    {
        void Write(string message);
    }

    public interface IConsole
    {

    }

    public class ConsoleLog : ILog, IConsole
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class EmailLog : ILog
    {
        private const string adminEmail = "admin@foo.com";

        public void Write(string message)
        {
            Console.WriteLine($"Email sent to {adminEmail} : {message}");
        }
    }

    public class Engine
    {
        private ILog log;
        private int id;

        public Engine(ILog log)
        {
            this.log = log;
            id = new Random().Next();
        }

        public Engine(ILog log, int id)
        {
            this.log = log;
            this.id = id;
        }

        public void Ahead(int power)
        {
            log.Write($"Engine [{id}] ahead {power}");
        }
    }

    public class SMSLog : ILog
    {
        private string phoneNumber;

        public SMSLog(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }

        public void Write(string message)
        {
            Console.WriteLine($"SMS to {phoneNumber} : {message}");
        }
    }

    public class Car
    {
        private Engine engine;
        private ILog log;

        public Car(Engine engine)
        {
            this.engine = engine;
            this.log = new EmailLog();
        }

        public Car(Engine engine, ILog log)
        {
            this.engine = engine;
            this.log = log;
        }

        public void Go()
        {
            engine.Ahead(100);
            log.Write("Car going forward...");
        }
    }



}
