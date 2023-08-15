using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alwatnia.Models
{
    public class AspNetUserLogin
    {
        [Column(Order = 0)]
        [Key]
        public string LoginProvider { get; set; }

        [Column(Order = 1)]
        [Key]
        public string ProviderKey { get; set; }

        [Key]
        [Column(Order = 2)]
        public string UserId { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}