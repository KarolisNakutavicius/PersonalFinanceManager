using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceManager.Server.Contexts;
using PersonalFinanceManager.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using System.Security.Authentication;
using PersonalFinanceManager.Service.Helpers;

namespace PersonalFinanceManager.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class IncomesController : ControllerBase
    {
        private readonly FinanceManagerContext _context;
        private readonly ClaimsIdentity _currentIdentity;

        public IncomesController(FinanceManagerContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _currentIdentity = (ClaimsIdentity) httpContextAccessor.HttpContext.User.Identity;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeModel>>> GetIncomes()
        {
            return await _context.Incomes.Where(i => i.UserId == _currentIdentity.GetUserId()).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IncomeModel>> GetIncome(int id)
        {
            var income = await _context.Incomes.FindAsync(id);

            if (income == null)
            {
                return NotFound();
            }

            return income;
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncome(int id, IncomeModel income)
        {
            if (id != income.StatementId)
            {
                return BadRequest();
            }

            _context.Entry(income).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncomeExists(id))
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

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IncomeModel>> PostIncome(IncomeModel income)
        {
            income.UserId = _currentIdentity.GetUserId();

            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIncome", new { id = income.StatementId }, income);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            var income = await _context.Incomes.FindAsync(id);

            if (income == null)
            {
                return NotFound();
            }

            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IncomeExists(int id)
        {
            return _context.Incomes.Any(e => e.StatementId == id);
        }
    }
}
