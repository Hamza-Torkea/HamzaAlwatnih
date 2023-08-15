using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace alwatnia.Models
{
    [Table("Company")]
    public class Company
    {
        public Company()
        {
            Reports = new HashSet<Reports>();
            Staff = new HashSet<Staff>();
            Exctive = new HashSet<Exctive>();
            Page = new HashSet<Page>();
            Product = new HashSet<Product>();
            Image = new HashSet<Image>();
            UsersProducts = new HashSet<UsersProducts>();
            RequestedProduct = new HashSet<RequestedProducts>();
            ProductCats = new HashSet<ProductCat>();
        }

        public int Id { get; set; }

        public string Title { get; set; }
        public string ETitle { get; set; }
        public string Video { get; set; }

        public bool? ShowProductsInstedOfCategories { get; set; }


        public string Email { get; set; }
        public string Password { get; set; }


        [StringLength(250)]
        public string City { get; set; }

        //        public decimal? lat { get; set; }
        //
        //        public decimal? lng { get; set; }

        public int? Lang { get; set; }

        [StringLength(250)]
        public string Facebook { get; set; }

        [StringLength(250)]
        public string Twitter { get; set; }

        [StringLength(250)]
        public string Instagram { get; set; }

        [StringLength(250)]
        public string Linkedin { get; set; }

        public DateTime? Cdate { get; set; }

        public string Details { get; set; }

        public string EDetails { get; set; }

        [AllowHtml]
        public string Map { get; set; }

        public string Photo { get; set; }

        public string image2 { get; set; }

        [AllowHtml]
        public string about { get; set; }

        public string Summary { get; set; }
        public string ESummary { get; set; }


        [AllowHtml]
        public string Eabout { get; set; }

        [AllowHtml]
        public string vision { get; set; }

        [AllowHtml]
        public string Evision { get; set; }

        [AllowHtml]
        public string message { get; set; }

        [AllowHtml]
        public string Emessage { get; set; }

        public string other_Id { get; set; }

        public virtual ICollection<Reports> Reports { get; set; }

        public virtual ICollection<Page> Page { get; set; }

        public virtual ICollection<Staff> Staff { get; set; }
        
            public virtual ICollection<Exctive> Exctive { get; set; }
        public virtual ICollection<Product> Product { get; set; }

        public virtual ICollection<ProductCat> ProductCats { get; set; }

        public virtual ICollection<Image> Image { get; set; }

        public virtual ICollection<UsersProducts> UsersProducts { get; set; }

        public virtual ICollection<RequestedProducts> RequestedProduct { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }

        public virtual ICollection<Branch> Branches { get; set; }
    }
}