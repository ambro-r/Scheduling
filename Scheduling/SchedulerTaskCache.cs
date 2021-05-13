using Scheduling.Constants;
using Scheduling.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Scheduling
{
    class SchedulerTaskCache
    {
        private SchedulerTaskCache() { }

        public static SchedulerTaskCache Instance { get; } = new SchedulerTaskCache();

        private ConcurrentDictionary<string, ScheduledTask> ScheduledTasks = new ConcurrentDictionary<string, ScheduledTask>();

        private double GetIntervalInHours(Scheduling.Attributes.Schedule schedule)
        {
            double intervalInHours;
            switch (schedule.IntervalType)
            {
                case IntervalType.DAYS:
                    intervalInHours = (double)schedule.Interval * 24;
                    break;
                case IntervalType.HOURS:
                    intervalInHours = (double)schedule.Interval;
                    break;
                case IntervalType.MINTURES:
                    intervalInHours = (double)schedule.Interval / 60;
                    break;
                case IntervalType.SECONDS:
                    intervalInHours = (double)schedule.Interval / 3600;
                    break;
                default:
                    intervalInHours = 0;
                    break;
            }
            return intervalInHours;
        }
      
        public string AddScheduledTask(System.Type type, MethodInfo method, Scheduling.Attributes.Schedule schedule)
        {
            string key = type.FullName + "." + method.Name;
            if (!ScheduledTasks.ContainsKey(key))
            {
                // TOOD: If the key exists, we should actually stop the schedule and re-add it. 
                ScheduledTask scheduleTask = new ScheduledTask()
                {
                    Type = type,
                    Method = method,
                    IntervalInHours = GetIntervalInHours(schedule),  
                    Start = schedule.Start,
                    Stop = schedule.Stop
                };                
                ScheduledTasks.TryAdd(key, scheduleTask);
            }
            return key;
        }

        public ScheduledTask GetScheduledTask(string key)
        {
            ScheduledTask scheduleTask;
            ScheduledTasks.TryGetValue(key, out scheduleTask);
            return scheduleTask;
        }

        public List<string> GetScheduleTasks()
        {
            return new List<string>(ScheduledTasks.Keys);
        }

    
    }
}
