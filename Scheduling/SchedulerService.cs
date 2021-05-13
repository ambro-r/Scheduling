using System;
using System.Collections.Generic;
using System.Threading;
using Scheduling.Constants;

namespace Scheduling
{
    public class SchedulerService
    {
        private SchedulerService() { }
        
        public static SchedulerService Instance { get; } = new SchedulerService();
    
        public Timer ScheduleTask(double intervalInHours, Action task)
        {
            return ScheduleTask(intervalInHours, DateTime.Now, task);
        }

        public Timer ScheduleTask(double intervalInHours, DateTime start, Action task)
        {                        
            TimeSpan timeToStart = start - DateTime.Now;
            if (timeToStart <= TimeSpan.Zero)
            {
                timeToStart = TimeSpan.Zero;
            }
            
            Timer timer = new Timer(x =>
            {
                task.Invoke();
            }, null, timeToStart, TimeSpan.FromHours(intervalInHours));

            return timer;
        }
    
    }
}
