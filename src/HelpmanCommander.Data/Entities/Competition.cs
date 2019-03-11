using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HelpmanCommander.Data.Entities
{
    public class Competition
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(100)]
        public string Location { get; set; }
        
        public DateTime? DateOfEvent { get; set; }

        [Required]
        public virtual IdentityUser Owner { get; set; }

        public List<Station> Stations { get; set; }
    }
}
