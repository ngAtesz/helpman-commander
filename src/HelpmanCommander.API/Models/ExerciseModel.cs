using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelpmanCommander.API.Models
{
    public class ExerciseModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        public ICollection<TaskModel> Tasks { get; set; }
    }
}