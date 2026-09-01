using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RoleBase_Api.Data;
using RoleBase_Api.Models;
using RoleBase_Api.Models.DTOs;

namespace RoleBase_Api.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddRole(Role role)
        {
            _context.Roles.Add(role);
            return Save();
        }

        public ICollection<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public bool Save()
        {
            var result = _context.SaveChanges();
            return result > 0;
        }

        bool IRoleRepository.DeleteRole(Role role)
        {
            _context.Roles.Remove(role);
            return Save();
        }

        Role IRoleRepository.GetRole(int id)
        {
            return _context.Roles.Find(id);
        }

        bool IRoleRepository.RoleExists(string name)
        {
            return _context.Roles.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        bool IRoleRepository.RoleExists(int id)
        {
            return _context.Roles.Any(x => x.Id == id);
        }

        bool IRoleRepository.UpdateRole(Role role)
        {
            var existingRole = _context.Roles.FirstOrDefault(x => x.Id == role.Id);
            if(existingRole == null)
            {
                return false;
            }
            existingRole.Name = role.Name;
            return Save();
        }
    }
}
