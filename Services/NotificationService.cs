using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MilaAPI.Models;

public class NotificationService : IDisposable
{
	private readonly IServiceProvider _serviceProvider;
	private Timer _timer;

	public NotificationService(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public void StartNotificationChecks()
	{
		// Trigger the notifications every hour or as needed
		_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));
	}

	private void DoWork(object state)
	{
		using (var scope = _serviceProvider.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<MilaContext>();

			// Trigger different notification checks
			CheckRecurringExpenses(context);
			CheckSavingGoals(context);
			CheckBudgets(context);
		}
	}

	private async void CheckRecurringExpenses(MilaContext context)
	{
		var today = DateTime.Today;
		var recurringExpenses = await context.RecurringExpenses
			.Include(re => re.User)
			.Where(re => re.NextDueDate <= today)
			.ToListAsync();

		foreach (var recurringExpense in recurringExpenses)
		{
			await SendNotificationAsync(
				recurringExpense.User,
				"Recurring Expense Due",
				$"Your recurring expense {recurringExpense.Name} is due today."
			);

			recurringExpense.NextDueDate = recurringExpense.NextDueDate.AddMonths(1); // or other frequency
		}

		await context.SaveChangesAsync();
	}

	private async void CheckSavingGoals(MilaContext context)
	{
		var nearEndGoals = await context.SavingGoals
			.Include(sg => sg.User)
			.Where(sg => sg.Deadline <= DateTime.Today.AddDays(7)) // Notify if goal ends within a week
			.ToListAsync();

		foreach (var goal in nearEndGoals)
		{
			await SendNotificationAsync(
				goal.User,
				"Saving Goal Approaching Deadline",
				$"Your saving goal {goal.Name} is nearing its deadline."
			);
		}

		await context.SaveChangesAsync();
	}

	private async void CheckBudgets(MilaContext context)
	{
		var budgets = await context.Budgets
			.Include(b => b.User)
			.Where(b => b.CurrentAmount >= b.Limit) // Check if the budget limit is reached
			.ToListAsync();

		foreach (var budget in budgets)
		{
			await SendNotificationAsync(
				budget.User,
				"Budget Limit Reached",
				$"Your budget {budget.Name} has reached its limit."
			);
		}

		await context.SaveChangesAsync();
	}

	public async Task SendNotificationAsync(User user, string title, string message)
	{
		if (user == null)
		{
			throw new ArgumentNullException(nameof(user), "User cannot be null when sending a notification.");
		}

		if (string.IsNullOrWhiteSpace(user.Email))
		{
			throw new ArgumentException("User must have a valid email address to send a notification.");
		}

		// Simulate sending notification (email, SMS, etc.)
		Console.WriteLine($"Sending notification to {user.Email}: Title: {title}, Message: {message}");

		await Task.CompletedTask;
	}

	public void Dispose()
	{
		_timer?.Dispose();
	}
}
