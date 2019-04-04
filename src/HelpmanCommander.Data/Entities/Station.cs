using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpmanCommander.Data.Entities
{
    public class Station
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public byte Number { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public int CompetitionId { get; set; }

        [ForeignKey(nameof(CompetitionId))]
        public Competition Competition { get; set; }

        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
