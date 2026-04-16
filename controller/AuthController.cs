using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRecipe.Api.Data;
using SmartRecipe.Api.DTOs;
using SmartRecipe.Api.Interfaces;
using SmartRecipe.Api.Models;

namespace SmartRecipe.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(x => x.Email == registerDto.Email))
                return BadRequest("Email is already taken");

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Role = user.Role
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user == null) return Unauthorized("Invalid Email");

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash)) return Unauthorized("Invalid Password");

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Role = user.Role
            };
        }
    }
}