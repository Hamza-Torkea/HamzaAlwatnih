using System.ComponentModel.DataAnnotations;

namespace alwatnia.areas.cpanel.Models
{
    public class ChangePasswordViewModel
    {
        [Required, DataType(DataType.Password)]
        public string OldPassword { get; set; }


        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}