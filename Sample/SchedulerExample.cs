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

        [Schedule(IntervalType = IntervalType.ONCE, Start = "21:20")]
        public void TerminateTasks()
        {
            LogToConsole("Terminate Tasks", ConsoleColor.Cyan);
            Scheduling.Scheduler.Stop();
        }


        [Schedule(IntervalType = IntervalType.ONCE)]
        public void OnceOff()
        {
            LogToConsole("Once Off", ConsoleColor.Magenta);
        }

        [Schedule(IntervalType = IntervalType.SECONDS, Interval = 5, Start = "17:00", Stop = "17:12")]
        public void FiveSecondIntervalBetweenXAndY()
        {
            LogToConsole("Five Second Interval between X and Y", ConsoleColor.Gray);
        }

        [Schedule(IntervalType = IntervalType.SECONDS, Interval = 10)]
        public void TenSeconIntervalSixitySecondSleep()
        {
            LogToConsole("Ten Second Interval (Thirty Second Execution)", ConsoleColor.White);
            try
            {
                Thread.Sleep(30000);
            }
            catch (Exception e)
            {
                LogToConsole("Exception occurred on MultipleInstances method: " + e.Message, ConsoleColor.White);
            }
        }

        [Schedule(IntervalType = IntervalType.SECONDS, Interval = 30)]
        public void ThirtySecondInterval()
        {
            LogToConsole("Thirty Second Interval", ConsoleColor.Red);
        }


        [Schedule(IntervalType = IntervalType.MINTURES, Interval = 1)]
        public void OneMinuteInterval()
        {
            LogToConsole("One Minute Interval", ConsoleColor.Yellow);
        }     
           
    }
}
