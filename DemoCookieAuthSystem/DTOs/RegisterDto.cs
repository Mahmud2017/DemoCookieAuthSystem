using System.ComponentModel.DataAnnotations;

namespace DemoCookieAuthSystem.DTOs
{
    public class RegisterDto : LoginDto
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = "";
    }
}
