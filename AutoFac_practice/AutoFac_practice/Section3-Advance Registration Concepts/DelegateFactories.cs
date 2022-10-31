using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoFac_practice.Section3_Advance_Registration_Concepts
{
    public class DelegateFactories
    {

        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Loan>();
            builder.RegisterType<BankLoan>();

            var containerRef = builder.Build();

            // We have created a delegate factory in order to resolve BankLoan class because it has second argument which is string
            // and it will no automatically resolved.
            // We created a delegate for this instead of resolving bank loan class we resolved that delegate. and then call it by passing argument
            // Which will eventually call constructor of Bank Loan.

            var ref2= containerRef.Resolve<BankLoan.Factory>();
            var a = ref2("Pakistan Bank");
            Console.WriteLine(" ");
        }

    }

    public class Loan
    {
        public string Currency { get; set; }
        public int Amount { get; set; }

        public Loan()
        {
        }


    }

    public class BankLoan
    {
        private string _bankName;
        public delegate BankLoan Factory(string bankName);

        
        public BankLoan(Loan loan,string bankName)
        {
            _bankName = bankName;
        }

        public override string ToString()
        {
            return $"Welcome to {_bankName}";
        }
    }
}
