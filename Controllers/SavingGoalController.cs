using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilaAPI.Models;
using MilaAPI.DTOs;

namespace MilaAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SavingGoalController : ControllerBase
	{
		private readonly MilaContext _context;

		public SavingGoalController(MilaContext context)
		{
			_context = context;
		}

		// GET: api/SavingGoal
		[HttpGet]
		public async Task<ActionResult<IEnumerable<SavingGoalDto>>> GetSavingGoals()
		{
			var savingGoals = await _context.SavingGoals
				.Include(sg => sg.User)  // Load related User
				.Select(sg => new SavingGoalDto
				{
					UserId = sg.UserId,
					Name = sg.Name,
					TargetAmount = sg.TargetAmount,
					CurrentAmount = sg.CurrentAmount,
					Deadline = sg.Deadline,
					IsAchieved = sg.IsAchieved
				})
				.ToListAsync();

			return Ok(savingGoals);
		}

		// GET: api/SavingGoal/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<SavingGoal>> GetSavingGoal(int id)
		{
			var savingGoal = await _context.SavingGoals
				.Include(sg => sg.User)
				.FirstOrDefaultAsync(sg => sg.Id == id);

			if (savingGoal == null)
			{
				return NotFound();
			}

			return Ok(savingGoal);
		}

		// POST: api/SavingGoal
		[HttpPost]
		public async Task<ActionResult<SavingGoal>> CreateSavingGoal(SavingGoalDto savingGoalDto)
		{
			var user = await _context.Users.FindAsync(savingGoalDto.UserId);
			if (user == null)
			{
				return BadRequest("User not found");
			}

			var savingGoal = new SavingGoal
			{
				UserId = savingGoalDto.UserId,
				Name = savingGoalDto.Name,
				TargetAmount = savingGoalDto.TargetAmount,
				CurrentAmount = savingGoalDto.CurrentAmount,
				Deadline = savingGoalDto.Deadline,
				IsAchieved = savingGoalDto.IsAchieved
			};

			_context.SavingGoals.Add(savingGoal);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetSavingGoal), new { id = savingGoal.Id }, savingGoal);
		}

		// PUT: api/SavingGoal/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateSavingGoal(int id, SavingGoalDto savingGoalDto)
		{
			var savingGoal = await _context.SavingGoals.FindAsync(id);
			if (savingGoal == null)
			{
				return NotFound();
			}

			savingGoal.Name = savingGoalDto.Name;
			savingGoal.TargetAmount = savingGoalDto.TargetAmount;
			savingGoal.CurrentAmount = savingGoalDto.CurrentAmount;
			savingGoal.Deadline = savingGoalDto.Deadline;
			savingGoal.IsAchieved = savingGoalDto.IsAchieved;

			_context.Entry(savingGoal).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SavingGoalExists(id))
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

		// DELETE: api/SavingGoal/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteSavingGoal(int id)
		{
			var savingGoal = await _context.SavingGoals.FindAsync(id);
			if (savingGoal == null)
			{
				return NotFound();
			}

			_context.SavingGoals.Remove(savingGoal);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool SavingGoalExists(int id)
		{
			return _context.SavingGoals.Any(sg => sg.Id == id);
		}
	}
}
