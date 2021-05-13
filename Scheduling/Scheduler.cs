using Scheduling.Constants;
using Scheduling.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduling
{
    public static class Scheduler
    {

        private static ConcurrentDictionary<string, ScheduledTask> ScheduledTasks = new ConcurrentDictionary<string, ScheduledTask>();

        private static double GetIntervalInHours(Scheduling.Attributes.Schedule schedule)
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

        private static string AddScheduledTask(System.Type type, MethodInfo method, Scheduling.Attributes.Schedule schedule)
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

        private static ScheduledTask GetScheduledTask(string key)
        {
            ScheduledTask scheduleTask;
            ScheduledTasks.TryGetValue(key, out scheduleTask);
            return scheduleTask;
        }

        private static List<string> GetScheduleTasks()
        {
            return new List<string>(ScheduledTasks.Keys);
        }

        private static List<System.Type> GetSchedulerTypes()
        {
            List<System.Type> types = new List<System.Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(typeof(Scheduling.Attributes.Scheduler), true).Length > 0)
                    {
                        types.Add(type);
                    }
                }
            }
            return types;
        }

        // TODO: Need to add logging;
        private static void InitiateScheduledTask(ScheduledTask scheduledTask)
        {
            try
            {
                DateTime startRun = (DateTime)(SchedulerHelper.Instance.GetDateTime(scheduledTask.Start) ?? DateTime.Now);
                DateTime? stopRun = SchedulerHelper.Instance.GetDateTime(scheduledTask.Stop);

                scheduledTask.Timer = SchedulerService.Instance.ScheduleTask(scheduledTask.IntervalInHours, startRun, () =>
                {
                    // First check if the scheduled task needs to be "paused"
                    scheduledTask.Paused = SchedulerHelper.Instance.Pause(startRun, stopRun);

                    // Run the task if it is not currently running or does not need to be paused
                    if (!scheduledTask.Running && !scheduledTask.Paused)
                    {
                        scheduledTask.Running = true;
                        try
                        {
                            object classObject = Activator.CreateInstance(scheduledTask.Type);
                            scheduledTask.Method.Invoke(classObject, null);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        scheduledTask.Running = false;
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // TODO: Need to add logging;
        private static void PauseScheduledTask(ScheduledTask scheduledTask)
        {
            scheduledTask.Paused = true;          
        }

        private static void InitateSchedules()
        {            
            List<System.Type> types = GetSchedulerTypes();
            foreach (System.Type type in types)
            {
                Scheduling.Attributes.Scheduler scheduler = (Scheduling.Attributes.Scheduler)type.GetCustomAttribute(typeof(Scheduling.Attributes.Scheduler));
                List<MethodInfo> methods = new List<MethodInfo>();
                foreach (MethodInfo method in type.GetMethods())
                {
                    if (method.GetCustomAttributes(typeof(Scheduling.Attributes.Schedule), true).Length > 0)
                    {
                        methods.Add(method);
                    }
                }

                foreach (MethodInfo method in methods)
                {
                    Scheduling.Attributes.Schedule schedule = (Scheduling.Attributes.Schedule) method.GetCustomAttribute(typeof(Scheduling.Attributes.Schedule));
                    AddScheduledTask(type, method, schedule);                             
                }
            }
        }

        public static void Start() 
        {
            InitateSchedules();
            foreach (string key in GetScheduleTasks())
            {
                ScheduledTask scheduleTask = GetScheduledTask(key);
                InitiateScheduledTask(scheduleTask);
            }
        }

        public static void Stop()
        {        
            foreach (string key in GetScheduleTasks())
            {
                ScheduledTask scheduledTask = GetScheduledTask(key);
                PauseScheduledTask(scheduledTask);
            }
            
        }
    }
}
