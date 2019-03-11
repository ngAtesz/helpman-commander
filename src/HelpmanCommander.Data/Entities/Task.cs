using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelpmanCommander.Data.Entities
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ushort DefaultScore { get; set; }

        public bool IsDefaultTask { get; set; }

        public int? PrerequisiteTaskId { get; set; }
        public virtual Task PrerequisiteTask { get; set; }

        public HashSet<Task> DependentTasks { get; set; }

        public ICollection<ExerciseTask> Exercises { get; set; }
    }
}