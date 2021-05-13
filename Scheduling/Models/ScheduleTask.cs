using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Scheduling.Models
{
    public class ScheduleTask
    {
        public bool Running { get; set; } = false;
        public int Executed { get; set; } = 0;
        public System.Type Type { get; set;  }
        public MethodInfo Method { get; set;  }        
        public Scheduling.Attributes.Schedule Schedule { get; set; }
        public System.Threading.Timer Timer { get; set; }

    }
}
