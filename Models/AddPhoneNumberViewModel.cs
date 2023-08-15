using System.ComponentModel.DataAnnotations;

namespace alwatnia.Models
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        public string Number { get; set; }
    }
}