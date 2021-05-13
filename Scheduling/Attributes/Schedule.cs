using Scheduling.Constants;
using System;
using System.Collections.Generic;

namespace Scheduling.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class Schedule : Attribute
    {
        public Day Day { get; set; }

        public IntervalType IntervalType { get; set; }

        public string Start { get; set; }

        public string Stop { get; set; }

        public int Interval { get; set; }


    }

}
