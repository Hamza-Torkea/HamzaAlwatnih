using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace alwatnia.Models
{
    public class Reports : IHasPaginator
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        [AllowHtml]
        [Column(TypeName = "ntext")]
        public string Details { get; set; }

        public string hijridate { get; set; }



        public int? company_id { get; set; }

        public int? Lang { get; set; }

        public string link { get; set; }

        public DateTime? cdate { get; set; }


        public DateTime? createdate { get; set; }

        public virtual Company Company { get; set; }

        public bool? home { get; set; }
    }
}