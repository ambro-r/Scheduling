using System;
using System.Collections.Generic;
using System.Threading;

namespace Scheduling
{
    public class SchedulerService
    {
        private SchedulerService() { }

        private List<Timer> Timers = new List<Timer>();
        
        public static SchedulerService Instance { get; } = new SchedulerService();

        public void ScheduleTask(int hour, int min, double intervalInHours, Action task)
        {
            DateTime now = DateTime.Now;
            DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);
            if (now > firstRun)
            {
                firstRun = firstRun.AddDays(1);
            }
            TimeSpan timeToGo = firstRun - now;
            if (timeToGo <= TimeSpan.Zero)
            {
                timeToGo = TimeSpan.Zero;
            }
            Timer timer = new Timer(x =>
            {
                task.Invoke();
            }, null, timeToGo, TimeSpan.FromHours(intervalInHours));
            Timers.Add(timer);
        }

    }
}
