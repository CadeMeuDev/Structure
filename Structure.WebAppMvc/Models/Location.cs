using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Structure.WebAppMvc.Models
{
    public class Location
    {
        [Key]
        public long Id { get; set; }

        public Guid Uuid { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "varchar(50)")]
        public string Acronym { get; set; } = string.Empty;

        public long? ParentId { get; set; }
        public Location? Parent { get; set; }

        public ICollection<Location> Children { get; set; } = new HashSet<Location>();
        public bool IsDeleted { get; set; }
        public LocationType LocationType { get; set; }
    }
}