using System;
using System.Collections.Generic;
using System.Text;

namespace DailyPlanner.Models
{
    public enum TaskPriority
    {
        Low,
        Normal,
        High
    }


    public class Task
    {
        public int TaskId { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public int Hour { get; set; }
        public int Minute { get; set; }

        public bool KaizenMode { get; set; }

        public TaskPriority TaskPriority { get; set; }

        public virtual TaskCollection TaskCollection { get; set; }
    }
}
