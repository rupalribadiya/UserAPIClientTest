using UserAPI.IServices;
using UserAPI.Models;
using UserAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UserAPI.Models.Custom;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userRepo;

        public UserController(IUserService _userService)
        {
            userRepo = _userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await userRepo.GetUsers();
                return Ok(new { data = users });
            }
            catch (Exception ex)
            {
                return BadRequest(new { data = ex.ToString() });
            }            
        }

        [HttpGet("{id}/{phoneNumber}")]
        public async Task<IActionResult> Get(int id, string phoneNumber)
        {
            try
            {
                var user = await userRepo.GetUser(id, phoneNumber);
                if (user == null)
                {
                    return NotFound(id);
                }
                return Ok(new { data = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { data = ex.ToString() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] UserModel user)
        {
            try
            {
                if (user != null)
                {
                    var newUser = await userRepo.AddUser(user);
                    return Ok(new { data = newUser });
                }
                else
                {
                    return BadRequest(new { data = "Invalid Request!!!" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { data = ex.ToString() });
            }
        }

        [HttpPatch("{id}/{phoneNumber}")]
        public async Task<IActionResult> Patch(int id, string phoneNumber, [FromForm]UserDTO user)
        {
            try
            {
                if (user != null)
                {
                    var isExist = await userRepo.GetUser(id, phoneNumber);
                    if(isExist == null)
                    {
                        return NotFound();
                    }

                    var updatedUser = await userRepo.UpdateUser(id, phoneNumber, user);
                    return Ok(new { data = updatedUser });
                }
                else
                {
                    return BadRequest(new { data = "Invalid Request!!!" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { data = ex.ToString() });
            }
        }

        [HttpDelete("{id}/{phoneNumber}")]
        public async Task<IActionResult> Delete(int id, string phoneNumber)
        {
            try
            {
                var user = await userRepo.DeleteUser(id, phoneNumber);
                if (user == null)
                {
                    return NotFound(id);
                }
                return Ok(new { data = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { data = ex.ToString() });
            }
        }

    }
}
