using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DailyPlanner.Models
{
    public class TaskCollection
    {
        public int TaskCollectionId { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfTasks { get; set; }

        public virtual ICollection<Task> Tasks{ get; private set; } = new ObservableCollection<Task>();
    }
}
