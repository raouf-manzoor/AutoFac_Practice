using AutoFac_practice.Section3_Advance_Registration_Concepts;
using AutoFac_practice_AdvancedRegistrationConcepts;
using System;

namespace AutoFac_practice
{
    class Program
    {
        static void Main(string[] args)
        {
            //RegistrationConceptsSec1.RegisterDependecies();
            //AdvancedRegistrationConcepts.RegisterDependencies();
            DelegateFactories.RegisterDependencies();
            Console.WriteLine("Hello World!");
        }
    }
}
