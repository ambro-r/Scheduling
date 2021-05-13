using Scheduling.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduling
{
    public class SchedulerHelper
    {

        private SchedulerHelper() { }

        public static SchedulerHelper Instance { get; } = new SchedulerHelper();

        private int GetHour(string timeStamp)
        {
            int hour = -1;
            if (!string.IsNullOrEmpty(timeStamp))
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

        public DateTime? GetDateTime(string time)
        {
            int startHour = GetHour(time);
            int startMinute = GetMinute(time);

            DateTime now = DateTime.Now;
            DateTime? runDateTime = null;
            if (startHour > -1)
            {
                runDateTime = new DateTime(now.Year, now.Month, now.Day, startHour, startMinute, 0, 0);
            }

            return runDateTime;
        }

        public double GetIntervalInHours(IntervalType intervalType, int interval)
        {
            double intervalInHours;
            switch (intervalType)
            {
                case IntervalType.DAYS:
                    intervalInHours = (double)interval * 24;
                    break;
                case IntervalType.HOURS:
                    intervalInHours = (double)interval;
                    break;
                case IntervalType.MINTURES:
                    intervalInHours = (double)interval / 60;
                    break;
                case IntervalType.SECONDS:
                    intervalInHours = (double)interval / 3600;
                    break;
                default:
                    intervalInHours = 0;
                    break;
            }
            return intervalInHours;
        }

        public bool Pause(DateTime? startRun, DateTime? stopRun)
        {
            bool pause = true;
            if(startRun != null)
            {
                TimeSpan timeToStart = (DateTime) startRun - DateTime.Now;
                pause &= !(timeToStart >= TimeSpan.Zero);
            }
            if (stopRun != null)
            {
                TimeSpan timeToStop = DateTime.Now - (DateTime) stopRun;
                pause &= !(timeToStop >= TimeSpan.Zero);
            }
            return !pause;
        }

    }
}
