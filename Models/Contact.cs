using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alwatnia.Models
{
    [Table("Contact")]
    public class Contact
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Email { get; set; }

        [Column(TypeName = "ntext")]
        public string Message { get; set; }

        public string Mobile { get; set; }

        public string Phone { get; set; }

        public bool? IsRead { get; set; }

        public DateTime? Cdate { get; set; }

        public int? CompanyId { get; set; }

        public ContactType Type { get; set; }

        public virtual Company Company { get; set; }

        public string CV { get; set; }
    }

    public enum ContactType
    {
        Company = 1,
        Main = 2,
        Job = 3
    }
}