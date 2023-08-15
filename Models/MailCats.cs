using System.ComponentModel.DataAnnotations.Schema;

namespace alwatnia.Models
{
    [Table("MailCats")]
    public class MailCats
    {
        public int Id { get; set; }

        public string name { get; set; }
    }
}