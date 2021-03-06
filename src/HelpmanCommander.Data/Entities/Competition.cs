﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DefaultValue(false)]
        public bool Finalized { get; set; } = false;

        [Required]
        public string OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual IdentityUser Owner { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        public virtual ICollection<Station> Stations { get; set; }
    }
}
