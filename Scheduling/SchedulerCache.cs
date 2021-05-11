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

        private ConcurrentDictionary<string, ScheduleTracking> ScheduleStatus = new ConcurrentDictionary<string, ScheduleTracking>();

        public bool Start(string key)
        {
            bool start = true;
            if (!ScheduleStatus.ContainsKey(key))
            {
                // Don't "start" if we can't add the key
                start = ScheduleStatus.TryAdd(key, new ScheduleTracking() { Running = false });
            }

            if (start && !ScheduleStatus[key].Running)
            {                 
                ScheduleStatus[key].Executed++;
                ScheduleStatus[key].Started = DateTime.Now;
                ScheduleStatus[key].Running = true;
                ScheduleStatus[key].Stopwatch.Start();                
            } else
            {
                // Don't "start" if the ScheduleStatus[key].Running = true
                start = false;
            }
            return start;
        }

        public void Stop(string key)
        {
            if (ScheduleStatus.ContainsKey(key))
            {
                ScheduleStatus[key].Stopped = DateTime.Now;
                ScheduleStatus[key].Running = false;
                ScheduleStatus[key].Stopwatch.Stop();
            }
        }

        public long ElapsedMilliseconds(string key)
        {
            return ScheduleStatus.TryGetValue(key, out ScheduleTracking scheduleTracking) ? scheduleTracking.Stopwatch.ElapsedMilliseconds : -1;
        }

        public bool IsRunning(string key)
        {
            return ScheduleStatus.TryGetValue(key, out ScheduleTracking scheduleTracking) ? scheduleTracking.Running : false;
        }

        public DateTime? StartedTimeStamp(string key)
        {
            return ScheduleStatus.TryGetValue(key, out ScheduleTracking scheduleTracking) ? scheduleTracking.Started : null;
        }

        public DateTime? StoppedTimeStamp(string key)
        {
            return ScheduleStatus.TryGetValue(key, out ScheduleTracking scheduleTracking) ? scheduleTracking.Stopped : null;
        }

    }
}
