using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alwatnia.Models
{
    [Table("Advertise")]
    public class Advertise
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string type { get; set; }

        public string details { get; set; }

        public DateTime? cdate { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        public int? lang { get; set; }

        public string Link { get; set; }
    }
}