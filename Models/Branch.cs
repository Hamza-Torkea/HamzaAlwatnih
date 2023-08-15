using System.ComponentModel.DataAnnotations;

namespace alwatnia.Models
{
    public class Branch
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string ETitle { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [StringLength(250)]
        public string Link { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public string EAdress { get; set; }
        public string Fax { get; set; }
        public string Fax2 { get; set; }
        public string Fax3 { get; set; }
        public string Fax4 { get; set; }
        public string Tell { get; set; }
        public string Tell2 { get; set; }
        public string Tell3 { get; set; }
        public string Tell4 { get; set; }
    }
}