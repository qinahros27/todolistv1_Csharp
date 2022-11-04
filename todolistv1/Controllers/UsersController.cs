using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todolistv1.Data;
using todolistv1.Models;

namespace todolistv1.Controllers
{
    [Route("api/v1/signup")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly todolistv1Context _context;

        public UsersController(todolistv1Context context)
        {
            _context = context;
        }

        // POST: api/v1/signup
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}

