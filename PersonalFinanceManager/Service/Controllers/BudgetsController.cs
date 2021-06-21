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
            var catogeries = _context.Categories
                .Include(c => c.Statements)
                .ToList()
                .Where(c => budget.Categories.FirstOrDefault(bc => c.Name == bc.Name) != null && c.Statements.Any(s => s is Expense))
                .ToList();

            budget.Categories.Clear();

            budget.Categories = catogeries;

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
            var budgets = await _context.Budgets
                .Include(b => b.Categories)
                .ThenInclude(c => c.Statements)
                .Where(b => b.Categories.Any(c => c.Statements.Any(s => s.UserId == _currentIdentity.GetUserId())))
                .ToListAsync();

            if (budgets == null)
            {
                return NotFound();
            }

            return budgets;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBudget(int id, Budget budget)
        {
            if (id != budget.BudgetId)
            {
                return BadRequest();
            }

            var currentBudget = await _context.Budgets.Include(c => c.Categories)
                .SingleAsync(b => b.BudgetId == id);
            var currentCategory = currentBudget.Categories.FirstOrDefault();

            if (currentCategory == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            int updatedCategoryId = budget.Categories.First().CategoryId;

            if (currentCategory.CategoryId != updatedCategoryId)
            {
                // change references
                var newCategory = _context.Categories.Find(updatedCategoryId);
                currentBudget.Categories.Remove(currentCategory);
                currentBudget.Categories.Add(newCategory);
            }

            currentBudget.Name = budget.Name;
            currentBudget.Amount = budget.Amount;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var budget = await _context.Budgets.FindAsync(id);

            if (budget == null)
            {
                return NotFound();
            }

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BudgetExists(int id)
        {
            return _context.Budgets.Any(e => e.BudgetId == id);
        }

    }
}
