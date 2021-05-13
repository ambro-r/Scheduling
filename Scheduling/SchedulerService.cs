using System;
using System.Collections.Generic;
using System.Threading;
using Scheduling.Constants;

namespace Scheduling
{
    public class SchedulerService
    {
        private SchedulerService() { }

        private List<Timer> Timers = new List<Timer>();

        public static SchedulerService Instance { get; } = new SchedulerService();

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

        private int GetHour(string timeStamp)
        {
            int hour = -1;
            if(!string.IsNullOrEmpty(timeStamp))
            {
                hour = Int32.Parse(timeStamp.Substring(0, timeStamp.IndexOf(":")));                
            }
            return hour;
        }

        private int GetMinute(string timeStamp)
        {
            int minute = -1;
            if (!string.IsNullOrEmpty(timeStamp))
            {
                minute = Int32.Parse(timeStamp.Substring(timeStamp.IndexOf(":") + 1));
            }
            return minute;
        }

        private DateTime GetStartDateTime(Scheduling.Attributes.Schedule schedule)
        {            
            int startHour = GetHour(schedule.Start);
            int startMinute = GetMinute(schedule.Start);

            DateTime now = DateTime.Now;
            DateTime startRun = now;
            if (startHour > -1)
            {
                startRun = new DateTime(now.Year, now.Month, now.Day, startHour, startMinute, 0, 0);
            }

            return startRun;
        }

        private DateTime? GetStopDateTime(Scheduling.Attributes.Schedule schedule)
        {
            int stopHour = GetHour(schedule.Stop);
            int stopMinute = GetMinute(schedule.Stop);

            DateTime now = DateTime.Now;
            DateTime? stopRun = null;
            if (stopHour > -1)
            {
                stopRun = new DateTime(now.Year, now.Month, now.Day, stopHour, stopMinute, 0, 0);
            }

            return stopRun;
        }
      
        public void ScheduleTask(Scheduling.Attributes.Schedule schedule, Action task)
        {
            DateTime startRun = GetStartDateTime(schedule);
            DateTime? stopRun = GetStopDateTime(schedule);

            TimeSpan timeToStart = startRun - DateTime.Now;
            if (timeToStart <= TimeSpan.Zero)
            {
                timeToStart = TimeSpan.Zero;
            }

            
            Timer timer = new Timer(x =>
            {
                task.Invoke();
            }, null, timeToStart, TimeSpan.FromHours(GetIntervalInHours(schedule)));

            Timers.Add(timer);
        }

    }
}
