using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserAPI.IServices;
using UserAPI.Models;
using UserAPI.Models.Custom;

namespace TestUserAPI.NUnitTest
{
    public class UserUnitTest
    {
        private Mock<IUserService> _userService;

        int id = 3;
        string phoneNumber = "1234567890";

        [SetUp]
        public void Setup()
        {
            _userService = new Mock<IUserService>();
        }

        [Test]
        public async Task TestGetUsers()
        {
            _userService.Setup(repo => repo.GetUsers()).ReturnsAsync(new List<User>()
            { 
                new User()
                {
                    UserId = id,
                    FirstName = "Test",
                    LastName = "Test",
                    EmailAddress = "test@gmail.com",
                    Password = "test@123",
                    Gender = "Male",
                    PhoneNumber = phoneNumber
                }
            });

            var result = await _userService.Object.GetUsers();
            Assert.True(result.Count == 1);
        }

        [Test]
        public async Task Create_Users()
        {
            UserModel usersEntity = null;
            _userService.Setup(r => r.AddUser(It.IsAny<UserModel>()))
                .Callback<UserModel>(x => usersEntity = x);
            var userMock = new UserModel
            {
                UserId = id,
                FirstName = "Test",
                LastName = "Test",
                EmailAddress = "test@gmail.com",
                Password = "test@123",
                Gender = "Male",
                PhoneNumber = phoneNumber
            };
            await _userService.Object.AddUser(userMock);
            _userService.Verify(x => x.AddUser(It.IsAny<UserModel>()), Times.Once);

            Assert.AreSame(usersEntity.FirstName, userMock.FirstName);
            Assert.AreSame(usersEntity.LastName, userMock.LastName);
            Assert.AreSame(usersEntity.EmailAddress, userMock.EmailAddress);
            Assert.AreSame(usersEntity.Gender, userMock.Gender);
            Assert.AreSame(usersEntity.PhoneNumber, userMock.PhoneNumber);
        }

        [Test]
        public async Task Update_Users()
        {
            UserDTO updateUsersEntity = null;
            _userService.Setup(r => r.UpdateUser(id, phoneNumber, It.IsAny<UserDTO>()))
                .Callback<int, string, UserDTO>((uId, PhNo, user) =>
                {
                    updateUsersEntity = user;
                    Assert.That(uId, Is.Not.Null);
                    Assert.That(PhNo, Is.EqualTo(PhNo));
                    Assert.That(user, Is.EqualTo(user));
                });
            var updateUserMock = new UserDTO
            {
                FirstName = "Test",
                LastName = "Updated",
                Password = "test@123",
                Gender = "Female"
            };
            await _userService.Object.UpdateUser(id, phoneNumber, updateUserMock);
            _userService.Verify(x => x.UpdateUser(id, phoneNumber, It.IsAny<UserDTO>()), Times.Once);

            Assert.AreSame(updateUsersEntity.FirstName, updateUserMock.FirstName);
            Assert.AreSame(updateUsersEntity.LastName, updateUserMock.LastName);
            Assert.AreSame(updateUsersEntity.Gender, updateUserMock.Gender);
        }


        [Test]
        public async Task Delete_Users()
        {
            _userService.Setup(repo => repo.DeleteUser(id, phoneNumber));

            await _userService.Object.DeleteUser(id, phoneNumber);

            _userService.Verify(repo => repo.DeleteUser(id, phoneNumber), Times.Once);
        }
    }
}