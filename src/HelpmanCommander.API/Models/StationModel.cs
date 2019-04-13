using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelpmanCommander.API.Models
{
    public class StationModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public byte Number { get; set; }
        public string Description { get; set; }

        public ICollection<ExerciseModel> Exercises { get; set; }
    }
}