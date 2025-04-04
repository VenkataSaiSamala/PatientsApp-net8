using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(DataContext dataContext, ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto) 
        {
            if (await UserExists(registerDto.Username))
                return BadRequest("User Already Exists !");
            
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            dataContext.Users.Add(user);
            await dataContext.SaveChangesAsync();
            return new UserDto
            {
                Username = registerDto.Username,
                Token = tokenService.CreateToken(user)
            };

        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await dataContext.Users.FirstOrDefaultAsync(s => s.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computeHash.Length; i++) {
                if(computeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");            
            }
            var userdto = new UserDto
            {
                Username = user.UserName,
                Token = tokenService.CreateToken(user)
            };
            return Ok(userdto);

        }

        private async Task<bool> UserExists(string username)
        {
            return await dataContext.Users.AnyAsync(s => s.UserName.ToLower() == username.ToLower());
        }
    }
}
