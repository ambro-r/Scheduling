using Scheduling.Constants;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Scheduling
{
    public static class Scheduler
    {

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

        private static void InvokeMethod(System.Type schedulerType, MethodInfo method, Scheduling.Attributes.Schedule schedule)
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

            SchedulerService.Instance.ScheduleTask(DateTime.Now.Hour, DateTime.Now.Minute, intervalInHours, () =>
            {
                string key = schedulerType.FullName + "." + method.Name;
                if (SchedulerCache.Instance.Start(key))
                {
                    try
                    {
                        object classObject = Activator.CreateInstance(schedulerType);
                        method.Invoke(classObject, null);
                        // TODO: Add Logging;
                    }
                    catch (Exception)
                    {
                        // TODO: Add Logging;
                    }
                    SchedulerCache.Instance.Stop(key);
                }
            });

        }

        public static void InitateSchedules()
        {
            List<System.Type> schedulerTypes = GetSchedulerTypes();
            foreach (System.Type schedulerType in schedulerTypes)
            {
                Scheduling.Attributes.Scheduler scheduler = (Scheduling.Attributes.Scheduler)schedulerType.GetCustomAttribute(typeof(Scheduling.Attributes.Scheduler));
                List<MethodInfo> scheduledTasks = new List<MethodInfo>();
                foreach (MethodInfo method in schedulerType.GetMethods())
                {
                    if (method.GetCustomAttributes(typeof(Scheduling.Attributes.Schedule), true).Length > 0)
                    {
                        scheduledTasks.Add(method);
                    }
                }

                foreach (MethodInfo scheduledTask in scheduledTasks)
                {
                    Scheduling.Attributes.Schedule schedule = (Scheduling.Attributes.Schedule)scheduledTask.GetCustomAttribute(typeof(Scheduling.Attributes.Schedule));
                    InvokeMethod(schedulerType, scheduledTask, schedule);
                }
            }
        }



    }
}
