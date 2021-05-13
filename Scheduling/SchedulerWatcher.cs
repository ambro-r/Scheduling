using Scheduling.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Scheduling
{
    public class SchedulerWatcher
    {

        private SchedulerWatcher() { }

        public static SchedulerWatcher Instance { get; } = new SchedulerWatcher();

        private bool Running { get; set; } = false;

        private Timer Watcher { get; set; } = null;

        private void InitiateWatcher()
        {
            if (Watcher == null)
            {
                double intervalInHours = (double) 1 / 6; // Essentially every 10 minutes
                Console.WriteLine(intervalInHours);
                TimeSpan timeToStart = TimeSpan.Zero;
                Watcher = new Timer(x =>
                {
                    Scheduler.HouseKeep();
                }, null, timeToStart, TimeSpan.FromHours(intervalInHours));
            } else
            {
                Console.WriteLine("Watcher is running already...");
            }
        }

        private void KillWatcher()
        {
            if (Watcher != null)
            {
                Watcher.Dispose();
                Watcher = null;
            }
        }

        public void Start() {
            if(!Running)
            {
                InitiateWatcher();
                Running = true;
            }            
        }

        public void Stop()
        {
            if (Running)
            {
                KillWatcher();
            }
            Running = false;
        }

        public bool IsRunning()
        {
            return Running;
        }


    }
}
