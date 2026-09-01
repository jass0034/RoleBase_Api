using Microsoft.EntityFrameworkCore;
using RoleBase_Api.Data;
using RoleBase_Api.Models;
using System.Data;

namespace RoleBase_Api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddUser(User user)
        {
            _context.Users.Add(user);
            return Save();
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users .Include(x => x.Role).ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() == 1 ? true : false; 
        }

        bool IUserRepository.DeleteUser(User user)
        {
            _context.Users.Remove(user);
            return Save();
        }

        User IUserRepository.GetUser(int id)
        {
            return _context.Users.Find(id);
        }

        bool IUserRepository.UpdateUser(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(x => x.Id == user.Id);
            if (existingUser == null)
            {
                return false;
            }
            existingUser.Name = user.Name;
            existingUser.FatherName = user.FatherName;
            existingUser.Email = user.Email;
            existingUser.Gender = user.Gender;
            existingUser.MobileNumber = user.MobileNumber;
            existingUser.IdProofType = user.IdProofType;
            existingUser.IdProofNumber = user.IdProofNumber;
            existingUser.Address = user.Address;
            existingUser.RoleId = user.RoleId;
            return Save();
        }

        bool IUserRepository.UserExists(string email, string mobileNumber, string idProofNumber)
        {
            return _context.Users.Any(x => x.Email == email && x.MobileNumber == mobileNumber && x.IdProofNumber == idProofNumber);
        }

        bool IUserRepository.UserExists(int id)
        {
           return _context.Users.Any(x => x.Id == id);
        }
    }
}
