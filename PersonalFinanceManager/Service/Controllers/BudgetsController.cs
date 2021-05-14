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
using PersonalFinanceManager.Service.Helpers;
using PersonalFinanceManager.Shared.Models;

namespace PersonalFinanceManager.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BudgetsController : Controller
    {
        private readonly FinanceManagerContext _context;
        private readonly ClaimsIdentity _currentIdentity;

        public BudgetsController(FinanceManagerContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _currentIdentity = (ClaimsIdentity)httpContextAccessor.HttpContext.User.Identity;
        }

        [HttpPost]
        public async Task<ActionResult<Budget>> PostBudget(Budget budget)
        {
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBudget", new { id = budget.BudgetId }, budget);
        }

        [HttpGet]
        public async Task<ActionResult<Budget>> GetBudget(int id)
        {
            var budget = await _context.Budgets.FindAsync(id);

            if (budget == null)
            {
                return NotFound();
            }

            return budget;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IList<Budget>>> GetAllBudgets()
        {
            var budgets = await _context.Budgets.Where(b => b.Categories.Any(c => c.Statements.Any(s => s.UserId == _currentIdentity.GetUserId()))).ToListAsync();

            if (budgets == null)
            {
                return NotFound();
            }

            return budgets;
        }

    }
}
