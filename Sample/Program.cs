using Scheduling;
using System;

namespace Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Press any key to stop...");
            Scheduler.Start();            
            Console.ReadKey();
        }
    }
}