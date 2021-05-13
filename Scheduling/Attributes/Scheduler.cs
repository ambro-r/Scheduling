using System;

namespace Scheduling.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class Scheduler : Attribute
    {
    }
}
