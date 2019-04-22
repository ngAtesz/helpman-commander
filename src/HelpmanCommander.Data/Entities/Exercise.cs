using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpmanCommander.Data.Entities
{
    public class Exercise
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public int StationId { get; set; }

        [ForeignKey(nameof(StationId))]
        public Station Station { get; set; }

        public virtual ICollection<ExerciseTask> Tasks { get; set; }
    }
}
