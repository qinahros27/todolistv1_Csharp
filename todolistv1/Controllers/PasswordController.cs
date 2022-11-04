using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todolistv1.Data;
using todolistv1.Models;
using todolistv1.Repository;

namespace todolistv1.Controllers
{
    [Authorize]
    [Route("api/v1/changePassword")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
       
            private readonly todolistv1Context _context;

            public PasswordController(todolistv1Context context)
            {
                _context = context;
            }

        // PUT: api/v1/changePassword/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
            public async Task<IActionResult> PutUser(int id, User user)

            {
             if (id != user.Id)
              {
                 return BadRequest();

              }
             user.UpdateDate = DateTime.UtcNow;
             _context.Entry(user).State = EntityState.Modified;

            try
                {
                     await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }

        private bool UserExists(int id)
            {
                return _context.User.Any(e => e.Id == id);
            }
        }
    }

