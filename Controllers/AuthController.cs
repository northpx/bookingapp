using System.Security.Cryptography;
using System.Text;
using API1.DatingApp.API.Service;
using Microsoft.AspNetCore.Mvc;
using API1.DatingApp.API.DTOs;
using API1.DatingApp.API.Data.Entities;
using API1.DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;

namespace API1.DatingApp.API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AuthController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }
 
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthUserDto authUserDto)
        {
            authUserDto.Username = authUserDto.Username.ToLower();
            if (_context.AppUsers.Any(u => u.Username == authUserDto.Username))
            {
                return BadRequest("This username already exists!");
            }
 
            using var hmac = new HMACSHA512();
            var passwordBytes = Encoding.UTF8.GetBytes(authUserDto.Password);
            var user = new User
            {
                Username = authUserDto.Username,
                PasswordHash = hmac.ComputeHash(passwordBytes),
                PasswordSalt = hmac.Key
            };
            _context.AppUsers.Add(user);
            _context.SaveChanges();
            var token = _tokenService.CreateToken(user.Username);
            return Ok(token);
        }
 
        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthUserDto authUserDto)
        {
            authUserDto.Username = authUserDto.Username.ToLower();
 
            var currentUser = _context.AppUsers
                .FirstOrDefault(u => u.Username == authUserDto.Username);
 
            if (currentUser == null)
            {
                return Unauthorized("Username is invalid.");
            }
 
            using var hmac = new HMACSHA512(currentUser.PasswordSalt);
            var passwordBytes = hmac.ComputeHash(
                Encoding.UTF8.GetBytes(authUserDto.Password)
            );
            for (int i = 0; i < currentUser.PasswordHash.Length; i++)
            {
                if (currentUser.PasswordHash[i] != passwordBytes[i])
                {
                    return Unauthorized("Password is invalid.");
                }
            }
           
            var token = _tokenService.CreateToken(currentUser.Username);
            return Ok(token);
        }
 
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.AppUsers.ToList());
        }
    }
}
