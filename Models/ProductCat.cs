using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace alwatnia.Models
{
    public class ProductCat : IHasPaginator
    {
        public ProductCat()
        {
            Products = new HashSet<Product>();
        }
        public int Id { get; set; }

        public string ArabicTitle { get; set; }

        public string EnglishTitle { get; set; }

        [StringLength(500)]
        public string Photo { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool? Active { get; set; }

        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}