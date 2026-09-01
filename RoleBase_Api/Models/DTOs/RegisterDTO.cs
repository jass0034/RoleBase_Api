using RoleBase_Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace RoleBase_Api.Models.DTOs
{
    public class RegisterDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]  
        public string FatherName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required]
        public GenderType Gender { get; set; }
        [Required]
        [RegularExpression(@"^[6-9]\d{9}$",
        ErrorMessage = "Invalid Mobile Number")]
        public string MobileNumber { get; set; }
        [Required]
        public string IdProofType { get; set; }

        [Required]
        public string IdProofNumber { get; set; }

        [Required]
        public string Address { get; set; }

        public int RoleId { get; set; }
    }
}
