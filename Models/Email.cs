using System.ComponentModel.DataAnnotations;

namespace alwatnia.Models
{
    public class GeneralEmail
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}