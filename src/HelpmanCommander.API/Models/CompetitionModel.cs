using System;
using System.Collections.Generic;

namespace HelpmanCommander.API.Models
{
    public class CompetitionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime? DateOfEvent { get; set; }
        public string OwnerId { get; set; }

        public ICollection<StationModel> Stations { get; set; }
    }
}
