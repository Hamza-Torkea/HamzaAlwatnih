using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace alwatnia.Models
{
    [Table("UsersProducts")]
    public class UsersProducts
    {
        public UsersProducts()
        {
            RequestedProduct = new HashSet<RequestedProducts>();
        }

        public int Id { get; set; }

        public string fullname { get; set; }

        public string email { get; set; }

        public string mobile { get; set; }

        public DateTime? cdate { get; set; }

        public int? company_id { get; set; }

        public int? status { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<RequestedProducts> RequestedProduct { get; set; }

        public virtual Company Company { get; set; }
    }
}