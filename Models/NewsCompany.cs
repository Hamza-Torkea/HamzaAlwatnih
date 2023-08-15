using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alwatnia.Models
{
    public class NewsCompany
    {
        [Key]
        [Required]
        [Column(Order = 1)]
        public int NewsId { get; set; }
        public virtual News News { get; set; }

        [Key]
        [Required]
        [Column(Order = 2)]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}