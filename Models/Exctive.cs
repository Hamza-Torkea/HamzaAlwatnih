using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace alwatnia.Models
{
    [Table("Exctive")]
    public class Exctive
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Job { get; set; }

        public string Photo { get; set; }

        public string Facebook { get; set; }

        public string Twitter { get; set; }

        public string LinkedIn { get; set; }

        public string Instagram { get; set; }

        public string Snapchat { get; set; }

        [Column(TypeName = "ntext")]
        [AllowHtml]
        public string Details { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public bool? Active { get; set; }

        public int? Lang { get; set; }

        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }
    }
}