using UserAPI.IServices;
using UserAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAPI.Helpers;
using UserAPI.Models.Custom;

namespace UserAPI.Repositories
{
    public class UserService : IUserService
    {
        public readonly UserDBContext _context;
        public UserService(UserDBContext context)
        {
            _context = context;
        }
        public async Task<User> GetUser(int id, string phoneNumber)
        {
            try
            {
                return await _context.Users.Where(r => r.UserId == id && (r.PhoneNumber == phoneNumber || phoneNumber == "")).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<User>> GetUsers()
        {
            try
            {
                List<User> list = await _context.Users.ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> AddUser(UserModel user)
        {
            try
            {
                User newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailAddress = user.EmailAddress,
                    Password = Cryptography.HashPassword(user.Password),
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber
                };
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return newUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> UpdateUser(int id, string phoneNumber, UserDTO user)
        {
            try
            {
                User updatedUser = await GetUser(id, phoneNumber);
                if (updatedUser != null)
                {
                    updatedUser.FirstName = user.FirstName;
                    updatedUser.LastName = user.LastName;
                    updatedUser.Gender = user.Gender;
                    if (!Cryptography.VerifyHashedPassword(updatedUser.Password, user.Password))
                    {
                        updatedUser.Password = Cryptography.HashPassword(user.Password);
                    }
                };
                await _context.SaveChangesAsync();
                return updatedUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public async Task<bool> DeleteUser(int id, string phoneNumber)
        {
            try
            {
                User deletedUser = await GetUser(id, phoneNumber);
                if (deletedUser != null)
                {
                    _context.Users.Remove(deletedUser);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

    }
}
