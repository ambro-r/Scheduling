using System.Reflection;

namespace Scheduling.Models
{
    public class ScheduledTask
    {
        public bool Running { get; set; } = false;
        public bool Paused { get; set; } = false;
        public System.Type Type { get; set;  }
        public MethodInfo Method { get; set;  }                
        public System.Threading.Timer Timer { get; set; } 
        public string Start { get; set; }
        public string Stop { get; set; }
        public double IntervalInHours { get; set; }
        //TODO: Days

    }
}
