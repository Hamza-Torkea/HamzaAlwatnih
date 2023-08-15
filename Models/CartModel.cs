using System.Collections.Generic;

namespace alwatnia.Models
{
    public class CartModel
    {

        public string name { get; set; }

        public string email { get; set; }

        public string mobile { get; set; }

        public string Description { get; set; }

        public int? companyId { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Notes { get; set; }

        public List<ProductItem> items { get; set; }
    }
}
