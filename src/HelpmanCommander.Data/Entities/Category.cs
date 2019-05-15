using System.ComponentModel.DataAnnotations;

namespace HelpmanCommander.Data.Entities
{
    // TODO: check how to store it as enum: https://github.com/snavarropino/EfCoreEnums#attempt-3-improved
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
