using System.ComponentModel.DataAnnotations;

namespace alwatnia.Models
{
    public class ForgotPasswordViewModel
    {
        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
