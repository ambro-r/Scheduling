using Scheduling.Constants;
using Scheduling.Models;
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

        // TODO: Need to add logging;
        private static void InvokeTask(ScheduleTask scheduleTask)
        {
            try
            {
                scheduleTask.Timer = SchedulerService.Instance.ScheduleTask(scheduleTask.Schedule, () =>
                {                    
                    if (!scheduleTask.Running)
                    {
                        scheduleTask.Running = true;
                        try
                        {
                            object classObject = Activator.CreateInstance(scheduleTask.Type);
                            scheduleTask.Method.Invoke(classObject, null);                            
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        scheduleTask.Running = false;
                    }
                });
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);                
            }
        }

        private static void TerminateTask(ScheduleTask scheduleTask)
        {
            try
            {               
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
                    SchedulerTaskCache.Instance.AddScheduleTask(type, method, schedule);                             
                }
            }
        }

        public static void Start() 
        {
            InitateSchedules();
            foreach (string key in SchedulerTaskCache.Instance.GetScheduleTasks())
            {
                ScheduleTask scheduleTask = SchedulerTaskCache.Instance.GetScheduleTask(key);
                InvokeTask(scheduleTask);
            }
            SchedulerWatcher.Instance.Start();
        }

        public static void HouseKeep()
        {
            Console.WriteLine("House Keep...");
        }

        public static void Stop()
        {        
            foreach (string key in SchedulerTaskCache.Instance.GetScheduleTasks())
            {
                ScheduleTask scheduleTask = SchedulerTaskCache.Instance.GetScheduleTask(key);
                TerminateTask(scheduleTask);
            }
            SchedulerWatcher.Instance.Stop();
        }
    }
}
