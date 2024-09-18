using Microsoft.EntityFrameworkCore;
using MilaAPI.DTOs;
using MilaAPI.Models;
using MilaAPI.Services;

public class TransactionProcessingService
{
	private readonly MilaContext _context;
	private readonly NotificationService _notificationService; // Assuming you have a notification service

	public TransactionProcessingService(MilaContext context, NotificationService notificationService)
	{
		_context = context;
		_notificationService = notificationService;
	}

	public async Task ProcessTransactionAsync(TransactionDto transactionDto)
	{
		// Create a new transaction based on DTO
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

		_context.Transactions.Add(transaction);
		await _context.SaveChangesAsync();

		// Call to link the transaction to an expense
		await LinkToExpense(transaction);
	}

	private async Task LinkToExpense(Transaction transaction)
	{
		// Fetch the category to check if it's an expense category
		var category = await _context.Categories.FindAsync(transaction.CategoryId);

		if (category == null)
		{
			throw new ArgumentException("Category not found");
		}

		// If the category is related to an expense
		if (category.IsExpenseCategory)
		{
			var existingExpense = await _context.Expenses
				.FirstOrDefaultAsync(e => e.CategoryId == transaction.CategoryId && e.Date.Date == transaction.Date.Date && e.UserId == transaction.UserId);

			if (existingExpense != null)
			{
				// Update existing expense amount
				existingExpense.Amount += transaction.Amount;
			}
			else
			{
				// Create a new expense
				var expense = new Expense
				{
					Amount = transaction.Amount,
					Date = transaction.Date,
					Description = transaction.Description,
					CategoryId = transaction.CategoryId,
					UserId = transaction.UserId
				};

				_context.Expenses.Add(expense);
			}

			await _context.SaveChangesAsync();

			// Trigger notification if expense exceeds threshold
			if (existingExpense != null && existingExpense.Amount > 500) // Example threshold
			{
				var user = await _context.Users.FindAsync(transaction.UserId);
				await _notificationService.SendNotificationAsync(user, "Expense limit exceeded", $"Your spending on {category.Name} exceeded the limit.");
			}
		}
	}
}
