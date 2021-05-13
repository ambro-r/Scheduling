using Scheduling.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Scheduling
{
    public class SchedulerCache
    {

        private SchedulerCache() { }

        public static SchedulerCache Instance { get; } = new SchedulerCache();

        private bool Running { get; set; } = false;

        public void Start() {
            Running = true;
        }

        public void Stop()
        {
            Running = false;
        }

        public bool IsRunning()
        {
            return Running;
        }


    }
}
