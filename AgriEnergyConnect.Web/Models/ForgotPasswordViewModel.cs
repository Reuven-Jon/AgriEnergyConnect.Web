using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Web.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        [MinLength(8)]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords must match.")]
        public string ConfirmPassword { get; set; }
    }
}