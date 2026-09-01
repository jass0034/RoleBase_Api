using System.ComponentModel.DataAnnotations;

namespace RoleBase_Api.Models.DTOs
{
    public class SendOtpDTO
    {
        [Required]
        [Display(Name = "User Name")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^[6-9]\d{9}$",
        ErrorMessage = "Invalid Mobile Number")]
        public string MobileNumber { get; set; }
    }


    public class VerifyOtpDTO
    {
        [Required]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "OTP")]
        public string OTP { get; set; } = string.Empty;
    }
}
