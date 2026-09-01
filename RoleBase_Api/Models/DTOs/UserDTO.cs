using RoleBase_Api.Enums;

namespace RoleBase_Api.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Email { get; set; }
        public GenderType Gender { get; set; }
        public string MobileNumber { get; set; }
        public string IdProofType { get; set; }
        public string IdProofNumber { get; set; }
        public string Address { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
    }
}
