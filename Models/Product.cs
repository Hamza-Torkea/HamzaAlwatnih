using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace alwatnia.Models
{
    public class Product:IHasPaginator
    {
        public Product()
        {
            Comments = new HashSet<Comment>();
            RequestedProduct = new HashSet<RequestedProducts>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        [AllowHtml]
        [Column(TypeName = "ntext")]
        public string Details { get; set; }

        [StringLength(500)]
        public string Photo { get; set; }

        public bool? Type { get; set; }

        public decimal? Cost { get; set; }

        public decimal? Discount_Cost { get; set; }

        public int? Lang { get; set; }

        public DateTime? Cdate { get; set; }

        public bool? Active { get; set; }

        public int? company_Id { get; set; }

        public int? ProductCatId { get; set; }

        public virtual Company Company { get; set; }

        public virtual ProductCat ProductCat { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<RequestedProducts> RequestedProduct { get; set; }
    }
}