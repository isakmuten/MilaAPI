using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilaAPI.Models;
using MilaAPI.DTOs;

namespace MilaAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RecurringExpenseController : ControllerBase
	{
		private readonly MilaContext _context;

		public RecurringExpenseController(MilaContext context)
		{
			_context = context;
		}

		// GET: api/RecurringExpense
		[HttpGet]
		public async Task<ActionResult<IEnumerable<RecurringExpenseDto>>> GetRecurringExpenses()
		{
			var recurringExpenses = await _context.RecurringExpenses
				.Include(re => re.Category)
				.Include(re => re.User)
				.Select(re => new RecurringExpenseDto
				{
					UserId = re.UserId,
					CategoryId = re.CategoryId,
					Amount = re.Amount,
					StartDate = re.StartDate,
					EndDate = re.EndDate,
					Recurrence = re.Recurrence,
					IsActive = re.IsActive
				})
				.ToListAsync();

			return Ok(recurringExpenses);
		}

		// GET: api/RecurringExpense/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<RecurringExpense>> GetRecurringExpense(int id)
		{
			var recurringExpense = await _context.RecurringExpenses
				.Include(re => re.Category)
				.Include(re => re.User)
				.FirstOrDefaultAsync(re => re.Id == id);

			if (recurringExpense == null)
			{
				return NotFound();
			}

			return Ok(recurringExpense);
		}

		// POST: api/RecurringExpense
		[HttpPost]
		public async Task<ActionResult<RecurringExpense>> CreateRecurringExpense(RecurringExpenseDto recurringExpenseDto)
		{
			var user = await _context.Users.FindAsync(recurringExpenseDto.UserId);
			if (user == null)
			{
				return BadRequest("User not found");
			}

			var category = await _context.Categories.FindAsync(recurringExpenseDto.CategoryId);
			if (category == null)
			{
				return BadRequest("Category not found");
			}

			var recurringExpense = new RecurringExpense
			{
				UserId = recurringExpenseDto.UserId,
				CategoryId = recurringExpenseDto.CategoryId,
				Amount = recurringExpenseDto.Amount,
				StartDate = recurringExpenseDto.StartDate,
				EndDate = recurringExpenseDto.EndDate,
				Recurrence = recurringExpenseDto.Recurrence,
				IsActive = recurringExpenseDto.IsActive
			};

			_context.RecurringExpenses.Add(recurringExpense);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetRecurringExpense), new { id = recurringExpense.Id }, recurringExpense);
		}

		// PUT: api/RecurringExpense/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateRecurringExpense(int id, RecurringExpenseDto recurringExpenseDto)
		{
			var recurringExpense = await _context.RecurringExpenses.FindAsync(id);
			if (recurringExpense == null)
			{
				return NotFound();
			}

			var user = await _context.Users.FindAsync(recurringExpenseDto.UserId);
			if (user == null)
			{
				return BadRequest("User not found");
			}

			var category = await _context.Categories.FindAsync(recurringExpenseDto.CategoryId);
			if (category == null)
			{
				return BadRequest("Category not found");
			}

			recurringExpense.Amount = recurringExpenseDto.Amount;
			recurringExpense.StartDate = recurringExpenseDto.StartDate;
			recurringExpense.EndDate = recurringExpenseDto.EndDate;
			recurringExpense.Recurrence = recurringExpenseDto.Recurrence;
			recurringExpense.IsActive = recurringExpenseDto.IsActive;

			_context.Entry(recurringExpense).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!RecurringExpenseExists(id))
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

		// DELETE: api/RecurringExpense/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRecurringExpense(int id)
		{
			var recurringExpense = await _context.RecurringExpenses.FindAsync(id);
			if (recurringExpense == null)
			{
				return NotFound();
			}

			_context.RecurringExpenses.Remove(recurringExpense);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool RecurringExpenseExists(int id)
		{
			return _context.RecurringExpenses.Any(re => re.Id == id);
		}
	}
}
