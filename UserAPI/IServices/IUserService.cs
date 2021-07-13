using UserAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models.Custom;

namespace UserAPI.IServices
{
    public interface IUserService
    {
        Task<List<User>> GetUsers();
        Task<User> GetUser(int id, string phoneNumber);
        Task<User> AddUser(UserModel user);
        Task<User> UpdateUser(int id, string phoneNumber, UserDTO user);
        Task<bool> DeleteUser(int id, string phoneNumber);
    }
}
