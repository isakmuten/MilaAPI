using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilaAPI.Models;
using MilaAPI.DTOs;
using MilaAPI.Services;


namespace MilaAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ExpenseController : ControllerBase
	{
		private readonly MilaContext _context;

		public ExpenseController(MilaContext context)
		{
			_context = context;
		}

		// GET: api/Expense
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
		{
			return await _context.Expenses.Include(e => e.Category).Include(e => e.User).ToListAsync();
		}

		// GET: api/Expense/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<Expense>> GetExpense(int id)
		{
			var expense = await _context.Expenses.Include(e => e.Category).Include(e => e.User).FirstOrDefaultAsync(e => e.Id == id);

			if (expense == null)
			{
				return NotFound();
			}

			return expense;
		}

		// POST: api/Expense
		[HttpPost]
		public async Task<ActionResult<Expense>> CreateExpense(ExpenseDto expenseDto)
		{
			var user = await _context.Users.FindAsync(expenseDto.UserId);
			if (user == null)
			{
				return BadRequest("User not found");
			}

			var category = await _context.Categories.FindAsync(expenseDto.CategoryId);
			if (category == null)
			{
				return BadRequest("Category not found");
			}

			var expense = new Expense
			{
				Amount = expenseDto.Amount,
				Date = expenseDto.Date,
				Description = expenseDto.Description,
				UserId = expenseDto.UserId,
				CategoryId = expenseDto.CategoryId
			};

			_context.Expenses.Add(expense);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
		}

		// PUT: api/Expense/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateExpense(int id, ExpenseDto expenseDto)
		{
			var expense = await _context.Expenses.FindAsync(id);
			if (expense == null)
			{
				return NotFound();
			}

			var user = await _context.Users.FindAsync(expenseDto.UserId);
			if (user == null)
			{
				return BadRequest("User not found");
			}

			var category = await _context.Categories.FindAsync(expenseDto.CategoryId);
			if (category == null)
			{
				return BadRequest("Category not found");
			}

			expense.Amount = expenseDto.Amount;
			expense.Date = expenseDto.Date;
			expense.Description = expenseDto.Description;
			expense.UserId = expenseDto.UserId;
			expense.CategoryId = expenseDto.CategoryId;

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

		// DELETE: api/Expense/{id}
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

		// Helper method to check if an expense exists
		private bool ExpenseExists(int id)
		{
			return _context.Expenses.Any(e => e.Id == id);
		}
	}
}
