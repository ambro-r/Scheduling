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

        private ConcurrentDictionary<string, ScheduleTask> ScheduleTasks = new ConcurrentDictionary<string, ScheduleTask>();

        public string AddScheduleTask(System.Type type, MethodInfo method, Scheduling.Attributes.Schedule schedule)
        {
            string key = type.FullName + "." + method.Name;
            if (!ScheduleTasks.ContainsKey(key))
            {
                // TOOD: If the key exists, we should actually stop the schedule and re-add it. 
                ScheduleTask scheduleTask = new ScheduleTask()
                {
                    Type = type,
                    Method = method,
                    Schedule = schedule
                };
                ScheduleTasks.TryAdd(key, scheduleTask);
            }
            return key;
        }

        public ScheduleTask GetScheduleTask(string key)
        {
            ScheduleTask scheduleTask;
            ScheduleTasks.TryGetValue(key, out scheduleTask);
            return scheduleTask;
        }

        public List<string> GetScheduleTasks()
        {
            return new List<string>(ScheduleTasks.Keys);
        }


    }
}
