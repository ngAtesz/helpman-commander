using System.Collections.Generic;

namespace HelpmanCommander.API.Models
{
    public class ExerciseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<TaskModel> Tasks { get; set; }
    }
}