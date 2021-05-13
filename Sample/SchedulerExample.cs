using Scheduling.Attributes;
using Scheduling.Constants;
using System;
using System.Threading;

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
            lock (Console.Out)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(string.Format("{0}:{1}", DateTime.Now.ToString("hh:mm:ss"), "Fifthy Second Interval"));
            }
        }

        [Schedule(IntervalType = IntervalType.MINTURES, Interval = 2)]
        public void TwoMinuteInterval()
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(string.Format("{0}:{1}", DateTime.Now.ToString("hh:mm:ss"), "Two Minute Interval"));
            }
        }

        [Schedule(IntervalType = IntervalType.SECONDS, Interval = 30)]
        public void ThirtySecondInterval()
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("{0}:{1}", DateTime.Now.ToString("hh:mm:ss"), "Thirty Second Interval"));
            }
        }

        [Schedule(IntervalType = IntervalType.SECONDS, Interval = 20)]
        public void TwentySecondIntervalSixitySecondSleep()
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(string.Format("{0}:{1}", DateTime.Now.ToString("hh:mm:ss"), "Twenty Second Interval + Sixity Second Sleep"));
            }
            try
            {
                Thread.Sleep(60000);
            }
            catch (Exception e)
            {
                lock (Console.Out)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Exception occurred on MultipleInstances method: " + e.Message);
                }
            }
        }

        [Schedule(IntervalType = IntervalType.ONCE)]
        public void OnceOff()
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(string.Format("{0}:{1}", DateTime.Now.ToString("hh:mm:ss"), "Once Off"));
            }
        }
        
        [Schedule(IntervalType = IntervalType.SECONDS, Interval = 5, Start = "09:55", Stop = "09:00")]
        public void FiveSecondInterval()
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(string.Format("{0}:{1}", DateTime.Now.ToString("hh:mm:ss"), "Five Second Interval between 08:00 and 09:00"));
            }
        }

    }
}
