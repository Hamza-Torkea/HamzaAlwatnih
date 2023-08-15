using System.ComponentModel.DataAnnotations;

namespace alwatnia.Models
{
    public class sysdiagram
    {
        [StringLength(128)]
        [Required]
        public string name { get; set; }

        public int principal_id { get; set; }

        [Key]
        public int diagram_id { get; set; }

        public int? version { get; set; }

        public byte[] definition { get; set; }
    }
}