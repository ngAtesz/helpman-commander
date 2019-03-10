using System.ComponentModel.DataAnnotations;

namespace HelpmanCommander.Data.Models
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
        public virtual Competition Competition { get; set; }
    }
}
