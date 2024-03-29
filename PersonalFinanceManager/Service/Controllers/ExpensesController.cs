﻿using System;
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
    public class ExpensesController : ControllerBase
    {
        private readonly FinanceManagerContext _context;
        private readonly ClaimsIdentity _currentIdentity;

        public ExpensesController(FinanceManagerContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _currentIdentity = (ClaimsIdentity)httpContextAccessor.HttpContext.User.Identity;
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {
            return await _context.Expenses.Include(e => e.Category).Where(e => e.UserId.Equals(_currentIdentity.GetUserId())).ToListAsync();
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.Include(c => c.Statements)
                .Where(c => c.Statements.Any(e => e.UserId.Equals(_currentIdentity.GetUserId()) && e is Expense))
                .ToListAsync();
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }

        // PUT: api/Expenses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(int id, Expense expense)
        {
            if (id != expense.StatementId)
            {
                return BadRequest();
            }

            _context.Entry(expense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
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

        // POST: api/Expenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Expense>> PostExpense(Expense expense)
        {
            expense.UserId = _currentIdentity.GetUserId();

            var category = _context.Categories.Include(c => c.Statements)
                .FirstOrDefault(c => c.Statements.Any(s => s.UserId.Equals(expense.UserId)) && c.Name.Equals(expense.Category.Name));

            if (category != null)
            {
                expense.Category = category;
            }

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            var result = CreatedAtAction("GetExpense", new { id = expense.StatementId }, expense);

            return result;
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.StatementId == id);
        }
    }
}
