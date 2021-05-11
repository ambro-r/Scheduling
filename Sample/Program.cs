using Scheduling;
using System;

namespace Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Scheduler.InitateSchedules();
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }
    }
}