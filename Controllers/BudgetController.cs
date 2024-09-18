using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilaAPI.Models;
using MilaAPI.DTOs;

namespace MilaAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BudgetController : ControllerBase
	{
		private readonly MilaContext _context;

		public BudgetController(MilaContext context)
		{
			_context = context;
		}

		// GET: api/Budget
		[HttpGet]
		public async Task<ActionResult<IEnumerable<BudgetDto>>> GetBudgets()
		{
			var budgets = await _context.Budgets
				.Include(b => b.User)
				.Include(b => b.Category)
				.Select(b => new BudgetDto
				{
					UserId = b.UserId,
					CategoryId = b.CategoryId,
					Amount = b.Amount,
					StartDate = b.StartDate,
					EndDate = b.EndDate,
					IsActive = b.IsActive
				})
				.ToListAsync();

			return Ok(budgets);
		}

		// GET: api/Budget/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<Budget>> GetBudget(int id)
		{
			var budget = await _context.Budgets
				.Include(b => b.User)
				.Include(b => b.Category)
				.FirstOrDefaultAsync(b => b.Id == id);

			if (budget == null)
			{
				return NotFound();
			}

			return Ok(budget);
		}

		// POST: api/Budget
		[HttpPost]
		public async Task<ActionResult<Budget>> CreateBudget(BudgetDto budgetDto)
		{
			var user = await _context.Users.FindAsync(budgetDto.UserId);
			if (user == null)
			{
				return BadRequest("User not found");
			}

			var category = await _context.Categories.FindAsync(budgetDto.CategoryId);
			if (category == null)
			{
				return BadRequest("Category not found");
			}

			var budget = new Budget
			{
				UserId = budgetDto.UserId,
				CategoryId = budgetDto.CategoryId,
				Amount = budgetDto.Amount,
				StartDate = budgetDto.StartDate,
				EndDate = budgetDto.EndDate,
				IsActive = budgetDto.IsActive
			};

			_context.Budgets.Add(budget);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetBudget), new { id = budget.Id }, budget);
		}

		// PUT: api/Budget/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBudget(int id, BudgetDto budgetDto)
		{
			var budget = await _context.Budgets.FindAsync(id);
			if (budget == null)
			{
				return NotFound();
			}

			var user = await _context.Users.FindAsync(budgetDto.UserId);
			if (user == null)
			{
				return BadRequest("User not found");
			}

			var category = await _context.Categories.FindAsync(budgetDto.CategoryId);
			if (category == null)
			{
				return BadRequest("Category not found");
			}

			budget.Amount = budgetDto.Amount;
			budget.StartDate = budgetDto.StartDate;
			budget.EndDate = budgetDto.EndDate;
			budget.IsActive = budgetDto.IsActive;

			_context.Entry(budget).State = EntityState.Modified;

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

		// DELETE: api/Budget/{id}
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
			return _context.Budgets.Any(b => b.Id == id);
		}
	}
}
