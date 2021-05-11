using Scheduling.Attributes;
using Scheduling.Constants;
using System;

namespace Sample
{
    [Scheduler]
    public class SchedulerExample
    {

        public SchedulerExample()
        {
        }

        [Schedule(IntervalType = IntervalType.SECONDS, Interval = 50)]
        public void FifthySecondInterval()
        {
            Console.WriteLine("Firing task scheduled to go off every 50 seconds...");
        }

        [Schedule(IntervalType = IntervalType.MINTURES, Interval = 2)]
        public void TwoMinuteInterval()
        {
            Console.WriteLine("Firing task scheduled to go off every 2 minutes...");
        }

        [Schedule(IntervalType = IntervalType.ONCE)]
        public void OnceOff()
        {
            Console.WriteLine("Firing task scheduled to go off only once...");
        }

    }
}
