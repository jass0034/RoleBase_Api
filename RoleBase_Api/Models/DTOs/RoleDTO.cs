using System.ComponentModel.DataAnnotations;

namespace RoleBase_Api.Models.DTOs
{
    public class RoleDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
