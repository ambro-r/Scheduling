using Scheduling.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduling.Attributes
{

    public class Schedule : Attribute
    {
        public List<Days> Days { get; set; }

        public IntervalType IntervalType { get; set; }

        public int Interval { get; set; }

    }

}
