using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual IdentityUser Owner { get; set; }

        public virtual ICollection<Station> Stations { get; set; }
    }
}
