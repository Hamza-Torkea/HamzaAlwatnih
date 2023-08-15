using System.ComponentModel.DataAnnotations;

namespace alwatnia.Models
{
    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Display(Name = "Code")]
        [Required]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }
}