using Scheduling.Constants;
using System;
using System.Collections.Generic;

namespace Scheduling.Attributes
{

    public class Schedule : Attribute
    {
        public List<Days> Days { get; set; }

        public IntervalType IntervalType { get; set; }

        public string Start { get; set; }

        public string Stop { get; set; }

        public int Interval { get; set; }


    }

}
