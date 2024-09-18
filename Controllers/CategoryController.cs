using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilaAPI.Models;
using MilaAPI.DTOs;

namespace MilaAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly MilaContext _context;

		public CategoryController(MilaContext context)
		{
			_context = context;
		}

		// GET: api/Category
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
		{
			return await _context.Categories.ToListAsync();
		}

		// GET: api/Category/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<CategoryDto>> GetCategory(int id)
		{
			var category = await _context.Categories.FindAsync(id);

			if (category == null)
			{
				return NotFound();
			}

			var categoryDto = new CategoryDto
			{
				Name = category.Name,
				Description = category.Description
			};

			return Ok(categoryDto);
		}


		// POST: api/Category
		[HttpPost]
		public async Task<ActionResult<CategoryDto>> CreateCategory(CategoryDto categoryDto)
		{
			var category = new Category
			{
				Name = categoryDto.Name,
				Description = categoryDto.Description
			};

			_context.Categories.Add(category);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, categoryDto);
		}

		// PUT: api/Category/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCategory(int id, CategoryDto categoryDto)
		{
			var category = await _context.Categories.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			category.Name = categoryDto.Name;
			category.Description = categoryDto.Description;

			_context.Entry(category).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CategoryExists(id))
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

		// DELETE: api/Category/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			var category = await _context.Categories.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			_context.Categories.Remove(category);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool CategoryExists(int id)
		{
			return _context.Categories.Any(c => c.Id == id);
		}
	}
}
