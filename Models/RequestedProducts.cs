using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alwatnia.Models
{
    [Table("RequestedProducts")]
    public class RequestedProducts
    {
        public int Id { get; set; }

        public int? product_id { get; set; }

        public int? company_id { get; set; }

        public int? UserProductId { get; set; }

        public int? amount { get; set; }

        public int? status { get; set; }

        [StringLength(500)]
        public string Photo { get; set; }

        public DateTime? cdate { get; set; }

        public virtual Product Product { get; set; }

        public virtual Company Company { get; set; }

        public virtual UsersProducts UsersProduct { get; set; }
    }
}