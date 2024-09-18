using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilaAPI.DTOs;
using MilaAPI.Models;
using MilaAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilaAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TransactionController : ControllerBase
	{
		private readonly MilaContext _context;
		private readonly TransactionProcessingService _transactionProcessingService;

		public TransactionController(MilaContext context, TransactionProcessingService transactionProcessingService )
		{
			_context = context;
			_transactionProcessingService = transactionProcessingService;
		}

		// GET: api/Transaction
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions()
		{
			var transactions = await _context.Transactions
				.Include(t => t.Category)
				.Include(t => t.User)
				.Select(t => new TransactionDto
				{
					Amount = t.Amount,
					Balance = t.Balance,
					Date = t.Date,
					Description = t.Description,
					Type = t.Type.ToString(),
					CategoryId = t.CategoryId,
					UserId = t.UserId,
					PaymentMethod = t.PaymentMethod.ToString()
				})
				.ToListAsync();

			return Ok(transactions);
		}

		// GET: api/Transaction/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<TransactionDto>> GetTransaction(int id)
		{
			var transaction = await _context.Transactions
				.Include(t => t.Category)
				.Include(t => t.User)
				.FirstOrDefaultAsync(t => t.Id == id);

			if (transaction == null)
			{
				return NotFound();
			}

			var transactionDto = new TransactionDto
			{
				Amount = transaction.Amount,
				Balance = transaction.Balance,
				Date = transaction.Date,
				Description = transaction.Description,
				Type = transaction.Type.ToString(),
				CategoryId = transaction.CategoryId,
				UserId = transaction.UserId,
				PaymentMethod = transaction.PaymentMethod.ToString()
			};

			return Ok(transactionDto);
		}

		// POST: api/Transaction
		[HttpPost]
		public async Task<ActionResult<Transaction>> CreateTransaction(TransactionDto transactionDto)
		{
			// Create the transaction entity from the DTO
			var transaction = new Transaction
			{
				Amount = transactionDto.Amount,
				Balance = transactionDto.Balance,
				Date = transactionDto.Date,
				Description = transactionDto.Description,
				Type = Enum.Parse<TransactionType>(transactionDto.Type),
				CategoryId = transactionDto.CategoryId,
				UserId = transactionDto.UserId,
				PaymentMethod = Enum.Parse<PaymentMethod>(transactionDto.PaymentMethod)
			};

			try
			{
				// Process the transaction using the service, which handles both adding and linking expenses.
				await _transactionProcessingService.ProcessTransactionAsync(transactionDto);
				return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
			}
			catch (ArgumentException ex)
			{
				// Return a bad request if there's an error with the arguments
				return BadRequest(ex.Message);
			}
		}


		// PUT: api/Transaction/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateTransaction(int id, TransactionDto transactionDto)
		{
			var transaction = await _context.Transactions.FindAsync(id);
			if (transaction == null)
			{
				return NotFound();
			}

			transaction.Amount = transactionDto.Amount;
			transaction.Balance = transactionDto.Balance;
			transaction.Date = transactionDto.Date;
			transaction.Description = transactionDto.Description;
			transaction.Type = Enum.Parse<TransactionType>(transactionDto.Type);
			transaction.CategoryId = transactionDto.CategoryId;
			transaction.UserId = transactionDto.UserId;
			transaction.PaymentMethod = Enum.Parse<PaymentMethod>(transactionDto.PaymentMethod);

			_context.Entry(transaction).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// DELETE: api/Transaction/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTransaction(int id)
		{
			var transaction = await _context.Transactions.FindAsync(id);
			if (transaction == null)
			{
				return NotFound();
			}

			_context.Transactions.Remove(transaction);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
