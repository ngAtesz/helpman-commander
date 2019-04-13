using System.ComponentModel.DataAnnotations;

namespace HelpmanCommander.API.Models
{
    //TODO: do we need separated ExerciseTaskModel? issue is the Score-DefaultScore
    public class TaskModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public ushort DefaultScore { get; set; }
        public bool IsDefault { get; set; }
        public int? PrerequisiteTaskId { get; set; }
    }
}