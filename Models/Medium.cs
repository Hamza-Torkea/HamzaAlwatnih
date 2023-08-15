using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alwatnia.Models
{
    public class Medium
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [Column(TypeName = "ntext")]
        public string Details { get; set; }

        public DateTime? cdate { get; set; }

        public bool? Active { get; set; }

        public string Link { get; set; }

        public int? Lang { get; set; }

        public string Title { get; set; }

        public int? OrderNo { get; set; }

        public string ETitle { get; set; }

        public string Summary { get; set; }
    }
}