using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRecipe.Api.Data;
using SmartRecipe.Api.DTOs;
using SmartRecipe.Api.Models;

namespace SmartRecipe.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("User not found");

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Weight = user.Weight,
                Height = user.Height,
                Goal = user.Goal
            };
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, UserUpdateDto updateDto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("User not found");

            if (!string.IsNullOrEmpty(updateDto.Username))
                user.Username = updateDto.Username;

            if (!string.IsNullOrEmpty(updateDto.Email))
            {
                if (await _context.Users.AnyAsync(x => x.Email == updateDto.Email && x.Id != id))
                    return BadRequest("Email is already taken");
                user.Email = updateDto.Email;
            }

            if (!string.IsNullOrEmpty(updateDto.Password))
                user.PasswordHash = updateDto.Password;

            if (updateDto.Weight.HasValue)
                user.Weight = updateDto.Weight.Value;

            if (updateDto.Height.HasValue)
                user.Height = updateDto.Height.Value;

            if (!string.IsNullOrEmpty(updateDto.Goal))
                user.Goal = updateDto.Goal;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
