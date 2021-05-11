using System;
using System.Diagnostics;

namespace Scheduling.Models
{
    public class ScheduleTracking
    {
        public int Executed { get; set; } = 0;
        public DateTime? Started { get; set; }
        public DateTime? Stopped { get; set; }
        public bool Running { get; set; } = false;
        public Stopwatch Stopwatch = new Stopwatch();
    }
}
