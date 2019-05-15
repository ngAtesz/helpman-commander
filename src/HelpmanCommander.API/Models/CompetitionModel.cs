using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HelpmanCommander.API.Models
{
    public class CompetitionModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Location { get; set; }
        public DateTime? DateOfEvent { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Finalized { get; set; } = false;

        public string OwnerId { get; set; }

        public int CategoryId { get; set; }

        public ICollection<StationModel> Stations { get; set; }
    }
}
