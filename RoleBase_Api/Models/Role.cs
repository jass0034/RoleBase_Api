using System.ComponentModel.DataAnnotations;

namespace RoleBase_Api.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
