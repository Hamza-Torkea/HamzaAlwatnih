using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alwatnia.Models
{
    [Table("MailingList")]
    public class MailingList
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string Email { get; set; }

        public string cat1 { get; set; }

        public string cat2 { get; set; }

        public string cat3 { get; set; }
    }
}