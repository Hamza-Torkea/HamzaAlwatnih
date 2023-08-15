using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace alwatnia.Models
{
    public class AspNetRole
    {
        public AspNetRole()
        {
            AspNetUsers = new HashSet<AspNetUser>();
        }

        public string Id { get; set; }

        [StringLength(256)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}