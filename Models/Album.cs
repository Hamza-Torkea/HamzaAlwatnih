using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace alwatnia.Models
{
    public class Album
    {
        public Album()
        {
            Image = new HashSet<Image>();
        }

        public int Id { get; set; }

        [StringLength(500)]
        public string ArabicTitle { get; set; }

        [StringLength(500)]
        public string EnglishTitle { get; set; }

        [Column(TypeName = "ntext")]
        [AllowHtml]
        public string ArabicDetails { get; set; }

        [AllowHtml]
        public string EnglishDetails { get; set; }

        public DateTime? cdate { get; set; }

        public int? OrderNo { get; set; }

        public int? type { get; set; }

        public virtual ICollection<Image> Image { get; set; }
    }
}