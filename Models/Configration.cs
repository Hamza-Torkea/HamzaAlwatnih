using System.ComponentModel.DataAnnotations.Schema;

namespace alwatnia.Models
{
    [Table("Configration")]
    public class Configration
    {
        public int Id { get; set; }

        public string Config_name { get; set; }

        [Column(TypeName = "ntext")]
        public string Config_details { get; set; }
    }
}