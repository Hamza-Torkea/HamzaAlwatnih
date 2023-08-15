using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace alwatnia.Models
{
    public class Page
    {
        public Page()
        {
            this.Image = new List<Image>();

        }
        public int Id { get; set; }

        public string title { get; set; }

        [AllowHtml]
        [Column(TypeName = "ntext")]
        public string details { get; set; }

        public bool? active { get; set; }

        [StringLength(50)]
        public string tag { get; set; }

        public int? lang { get; set; }

        public int? company_id { get; set; }

        public string video { get; set; }

        public string photo { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Image> Image { get; set; }

    }
}