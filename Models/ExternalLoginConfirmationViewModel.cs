﻿using System.ComponentModel.DataAnnotations;

namespace alwatnia.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}