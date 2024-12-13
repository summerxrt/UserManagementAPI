using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                {
                    return BadRequest(new { Message = "Page and pageSize must be greater than 0." });
                }

                var users = await _context.Users
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (!users.Any())
                {
                    return NotFound(new { Message = "No users found for the specified page and page size." });
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving users.", Details = ex.Message });
            }
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { Message = $"User with ID {id} not found." });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the user.", Details = ex.Message });
            }
        }

        // POST: api/users
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            // Validate input fields
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest(new { Message = "Name and Email are required." });
            }

            if (!new EmailAddressAttribute().IsValid(user.Email))
            {
                return BadRequest(new { Message = "Invalid Email address." });
            }

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the user.", Details = ex.Message });
            }
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest(new { Message = "The provided ID does not match the user's ID." });
            }

            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest(new { Message = "Name and Email are required." });
            }

            if (!new EmailAddressAttribute().IsValid(user.Email))
            {
                return BadRequest(new { Message = "Invalid Email address." });
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.Id == id))
                {
                    return NotFound(new { Message = $"User with ID {id} not found." });
                }
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the user.", Details = ex.Message });
            }
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { Message = $"User with ID {id} not found." });
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the user.", Details = ex.Message });
            }
        }
    }
}

