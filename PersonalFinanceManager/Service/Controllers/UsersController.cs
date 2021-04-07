using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceManager.Server.Contexts;
using PersonalFinanceManager.Shared.Models;
using PersonalFinanceManager.Shared.Responses;

namespace PersonalFinanceManager.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly FinanceManagerContext _context;
        private readonly UserManager<User> _userManager;

        public UsersController(FinanceManagerContext context,
            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterModel model)
        {
            var newUser = new User { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return Ok(new RegisterResult { Successful = false, Errors = errors });

            }

            return Ok(new RegisterResult { Successful = true });
        }

        // GET: api/Users
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        //{
        //    return await _context.Useriauskai.ToListAsync();
        //}

        //// GET: api/Users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<UserModel>> GetUser(int id)
        //{
        //    var user = await _context.Useriauskai.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}

        //// PUT: api/Users/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(int id, UserModel user)
        //{
        //    if (id != user.UserID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Users
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<UserModel>> PostUser(UserModel user)
        //{
        //    _context.Useriauskai.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.UserID }, user);
        //}

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var user = await _context.Useriauskai.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Useriauskai.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool UserExists(int id)
        //{
        //    return _context.Useriauskai.Any(e => e.UserID == id);
        //}
    }
}
