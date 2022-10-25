using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoFac_practice
{
    // This section contains details of Registration Concepts udemy course by Dmitri Nesteruk
    public class RegistrationConceptsSec1
    {
        public static void RegisterDependecies()
        {
            // This code is without DI

            //var log = new ConsoleLog();
            //var engine = new Engine(log);
            //var car = new Car(engine, log);
            //car.Go();

            // With AutoFac Container and Using DI

            // In order to work with AutoFac first we have to build a container
            var builder = new ContainerBuilder();

            // Register all the types which we want to resolve

            // If we registered multiple classes with one interface like the example mentioned below we will get the class refrence which is registerd
            // at the last. In this case we will get emailLog object.
            // to prevent overriding default class registration we can use "PreserveExistingDefaults" method.

            builder.RegisterType<ConsoleLog>().As<ILog>();
            builder.RegisterType<EmailLog>().As<ILog>().PreserveExistingDefaults();

            // By default Container we call the constructor which has most arguments. In order to call the constructor of our own choice we have 
            // to use the UsingConstructor() method which is mentioned below.

            //builder.RegisterType<Car>()
            //    .UsingConstructor(typeof(Engine));

            // If there is a requirement in which you have register Instance of class instead of Type. You have to use RegisterInstance Method I will give
            // you the instance of that object which you create every time you made a request to get that class refrence as mentioned below

            //. builder.RegisterInstance(new ConsoleLog()).As<ILog>();

            builder.RegisterType<Car>();

            // Engine class has a constructor with two agruments one is Log and the other one is engineId which is of type Int.
            // If we registered it the way mentioned below we will get exception because engine class will not be resolved by the container due to 
            // second argument which is Int.

            //builder.RegisterType<Engine>();

            // To resolve this issue we have to specify the constructor and pass the arguments manually at the time of registration.
            // IComponentContext contains all the classes which  are registerd with the container.

            builder.Register((IComponentContext c) => new Engine(c.Resolve<ILog>(), 123));

            // Registration of generic components we specity <> as empty due to we are not restricting it to any type

            //builder.RegisterGeneric(typeof(List<>)).As(typeof(IList<>));
            //var listRefrence = container.Resolve<IList<int>>();
            //Console.WriteLine(listRefrence.GetType().ToString());

            var container = builder.Build();
            container.Resolve<Car>().Go();
        }


    }


    // Class Hierarchy provided in tutorial

    public interface ILog
    {
        void Write(string message);
    }

    public class ConsoleLog : ILog
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class EmailLog : ILog
    {
        public void Write(string message)
        {
            Console.WriteLine("Email Log: " + message);
        }
    }

    public class Engine
    {
        private ILog log;
        private int id;

        public Engine(ILog log, int engineId)
        {
            this.log = log;
            this.id = engineId;
        }

        public void Ahead(int power)
        {
            log.Write($"Engine [{id}] ahead {power}");
        }
    }

    public class Car
    {
        private Engine engine;
        private ILog log;

        public Car(Engine engine)
        {
            this.engine = engine;
            this.log = new EmailLog(); // assigning it to verify some concepts by default we have registered ConsoleLog
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
