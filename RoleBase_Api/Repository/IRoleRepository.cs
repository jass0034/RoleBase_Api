using RoleBase_Api.Models;
using RoleBase_Api.Models.DTOs;

namespace RoleBase_Api.Repository
{
    public interface IRoleRepository
    {
        bool AddRole(Role role);
        bool UpdateRole(Role role);
        bool DeleteRole(Role role);
        bool RoleExists(string name);
        bool RoleExists(int id);
        Role GetRole(int id);
        ICollection<Role> GetRoles();
        bool Save();
    }
}
