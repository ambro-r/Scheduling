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
       
        private void LogToConsole(String text, System.ConsoleColor consoleColor)
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = consoleColor;
                Console.WriteLine(string.Format("{0}:{1}", DateTime.Now.ToString("hh:mm:ss"), text));
            }
        }

        [Schedule(IntervalType = IntervalType.SECONDS, Interval = 50)]
        public void FifthySecondInterval()
        {
            LogToConsole("Fifthy Second Interval", ConsoleColor.Green);
        }

        [Schedule(IntervalType = IntervalType.MINTURES, Interval = 2)]
        public void TwoMinuteInterval()
        {
            LogToConsole("Two Minute Interval", ConsoleColor.Yellow);
        }

        [Schedule(IntervalType = IntervalType.SECONDS, Interval = 30)]
        public void ThirtySecondInterval()
        {
            LogToConsole("Thirty Second Interval", ConsoleColor.Red);
        }

        [Schedule(IntervalType = IntervalType.SECONDS, Interval = 20)]
        public void TwentySecondIntervalSixitySecondSleep()
        {
            LogToConsole("Twenty Second Interval + Sixity Second Sleep", ConsoleColor.White);
            try
            {
                Thread.Sleep(60000);
            }
            catch (Exception e)
            {
                LogToConsole("Exception occurred on MultipleInstances method: " + e.Message, ConsoleColor.White);
            }
        }

        [Schedule(IntervalType = IntervalType.ONCE)]
        public void OnceOff()
        {
            LogToConsole("Once Off", ConsoleColor.Magenta);
        }
        
        [Schedule(IntervalType = IntervalType.SECONDS, Interval = 5, Start = "09:55", Stop = "09:00")]
        public void FiveSecondInterval()
        {
            LogToConsole("Five Second Interval between 08:00 and 09:00", ConsoleColor.Gray);
        }

    }
}
