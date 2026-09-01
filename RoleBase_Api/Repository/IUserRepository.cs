using RoleBase_Api.Models;

namespace RoleBase_Api.Repository
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        bool AddUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool UserExists(string email, string mobileNumber, string idProofNumber);
        bool UserExists(int id);
        bool Save();
    }
}
