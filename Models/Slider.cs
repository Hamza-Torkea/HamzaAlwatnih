using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace alwatnia.Models
{
    [Table("Slider")]
    public class Slider
    {
        public int Id { get; set; }

        [AllowHtml]
        public string Title { get; set; }

        [AllowHtml]
        [Column(TypeName = "ntext")]
        public string Details { get; set; }

        public string Summery { get; set; }

        public bool? HasDetails { get; set; }

        public int? Lang { get; set; }

        public int? OrderNo { get; set; }

        public DateTime? Cdate { get; set; }

        public string Url { get; set; }

        [StringLength(500)]
        public string Photo { get; set; }
    }
}