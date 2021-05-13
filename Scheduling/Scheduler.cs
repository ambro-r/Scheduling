using Scheduling.Constants;
using Scheduling.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

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

        // TODO: Need to add logging;
        private static void InvokeScheduledTask(ScheduledTask scheduledTask)
        {
            try
            {
                DateTime startRun = (DateTime) (SchedulerHelper.Instance.GetDateTime(scheduledTask.Start) ?? DateTime.Now);
                scheduledTask.Timer = SchedulerService.Instance.ScheduleTask(scheduledTask.IntervalInHours, startRun, () =>
                {                    
                    if (!scheduledTask.Running && !scheduledTask.Terminate)
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
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);                
            }
        }

        // TODO: Need to add logging;
        private static void TerminateScheduledTask(ScheduledTask scheduledTask)
        {
            try
            {                 
                scheduledTask.Terminate = true;

                Task task = new Task(() =>
                {
                    int wait = (int)scheduledTask.IntervalInHours * 3600;
                    while (scheduledTask.Running)
                    {                        
                        try
                        {
                            Thread.Sleep(wait);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    scheduledTask.Timer.Dispose();                    
                });               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }        
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
                    SchedulerTaskCache.Instance.AddScheduledTask(type, method, schedule);                             
                }
            }
        }

        public static void Start() 
        {
            InitateSchedules();
            foreach (string key in SchedulerTaskCache.Instance.GetScheduleTasks())
            {
                ScheduledTask scheduleTask = SchedulerTaskCache.Instance.GetScheduledTask(key);
                InvokeScheduledTask(scheduleTask);
            }
            SchedulerWatcher.Instance.Start();
        }

        public static void HouseKeep()
        {
            Console.WriteLine("House Keep...");
            foreach (string key in SchedulerTaskCache.Instance.GetScheduleTasks())
            {
                ScheduledTask scheduleTask = SchedulerTaskCache.Instance.GetScheduledTask(key);                
                DateTime? runStop = SchedulerHelper.Instance.GetDateTime(scheduleTask.Stop);
                if(runStop != null)
                {                    
                    TimeSpan timeToStop = DateTime.Now - (DateTime) runStop;
                    if(timeToStop >= TimeSpan.Zero)
                    {
                        Console.WriteLine("Terminating...");
                        TerminateScheduledTask(scheduleTask);
                    }                    
                }                
            }
        }

        public static void Stop()
        {        
            foreach (string key in SchedulerTaskCache.Instance.GetScheduleTasks())
            {
                ScheduledTask scheduledTask = SchedulerTaskCache.Instance.GetScheduledTask(key);
                TerminateScheduledTask(scheduledTask);
            }
            SchedulerWatcher.Instance.Stop();
        }
    }
}
