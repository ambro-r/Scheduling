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

    }
}
