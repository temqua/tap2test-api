using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tap2Test_Api.Data;
using Tap2Test_Api.Dto;

namespace Tap2Test_Api.Controllers
{
    [Route("api/users/form")]
    [ApiController]
    public class UsersController(AppDbContext context) : ControllerBase
    {

        private readonly AppDbContext _context = context;

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> CreateUser([FromForm] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = userDto.ToEntity();
            entity.Id = Guid.NewGuid();

            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User created", user = entity.ToDto() });
        }

        [HttpPut("{id:guid}")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromForm] UserDto updatedDto)
        {
            var existing = await _context.Users.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = updatedDto.Name;
            existing.Email = updatedDto.Email;
            existing.Phone = updatedDto.Phone;
            existing.Age = updatedDto.Age;

            await _context.SaveChangesAsync();

            return Ok(new { message = "User updated", user = existing.ToDto() });
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            var dtos = users.Select(user => user.ToDto()).ToList();
            return Ok(dtos);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var existing = await _context.Users.FindAsync(id);
            if (existing == null)
                return NotFound();

            _context.Users.Remove(existing);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 — успешное удаление без тела ответа
        }
    }
}
