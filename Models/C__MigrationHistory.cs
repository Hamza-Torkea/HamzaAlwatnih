using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alwatnia.Models
{
    [Table("__MigrationHistory")]
    public class C__MigrationHistory
    {
        [StringLength(150)]
        [Column(Order = 0)]
        [Key]
        public string MigrationId { get; set; }

        [StringLength(300)]
        [Key]
        [Column(Order = 1)]
        public string ContextKey { get; set; }

        [Required]
        public byte[] Model { get; set; }

        [Required]
        [StringLength(32)]
        public string ProductVersion { get; set; }
    }
}