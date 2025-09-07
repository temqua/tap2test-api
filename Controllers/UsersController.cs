using Microsoft.AspNetCore.Mvc;
using Tap2Test_Api.Dto;

namespace Tap2Test_Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static readonly List<UserDto> Users = new();


        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult CreateUser([FromForm] UserDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            user.Id = Guid.NewGuid();
            Users.Add(user);
            return Ok(new { message = "User created", user });
        }


        [HttpPut("{id:guid}")]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult UpdateUser(Guid id, [FromForm] UserDto updatedUser)
        {
            var existing = Users.FirstOrDefault(u => u.Id == id);
            if (existing == null)
                return NotFound();


            existing.Name = updatedUser.Name;
            existing.Email = updatedUser.Email;
            existing.Phone = updatedUser.Phone;
            existing.Age = updatedUser.Age;


            return Ok(new { message = "User updated", existing });
        }


        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(Users);
        }


        [HttpDelete("{id:guid}")]
        public IActionResult DeleteUser(Guid id)
        {
            var existing = Users.FirstOrDefault(u => u.Id == id);
            if (existing == null)
                return NotFound();


            Users.Remove(existing);
            return NoContent();
        }
    }
}
