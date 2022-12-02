using Autofac;
using Autofac.Features.OwnedInstances;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoFac_practice.Section4_Implicit_Relationship_Types
{
    public class DelayedInstantiation
    {

       public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>();
            builder.RegisterType<Reporting>();

            using(var c = builder.Build())
            {
                c.Resolve<Reporting>().Report();
            }
        }

    }

    public interface ILog : IDisposable
    {
        void Write(string message);
    }

    public class ConsoleLog : ILog
    {

        public ConsoleLog()
        {
            Console.WriteLine($"Console log created at {DateTime.Now.Ticks}");
        }
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
        public void Dispose()
        {
            Console.WriteLine("Console logger no longer required");
        }


    }
    public class Reporting
    {
        private readonly Lazy<ConsoleLog> log;

        public Reporting(Lazy<ConsoleLog> log)
        {
            this.log = log;
            Console.WriteLine("Reporting component created");
        }

        public void Report()
        {
            if (log.IsValueCreated)
                Console.WriteLine("just testing");

            log.Value.Write($"Log started");
        }
    }


}
