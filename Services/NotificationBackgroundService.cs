using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MilaAPI.Models;

namespace MilaAPI.Services
{
	public class NotificationBackgroundService : IHostedService, IDisposable
	{
		private readonly IServiceProvider _serviceProvider; // Holds the service provider for scoped services
		private Timer _timer;

		public NotificationBackgroundService(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			// Set up the timer to trigger notifications at desired intervals, e.g., every hour
			_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));
			return Task.CompletedTask;
		}

		private void DoWork(object state)
		{
			// Create a new scope to ensure we can use scoped services like MilaContext
			using (var scope = _serviceProvider.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<MilaContext>();
				var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();

				// Execute different notification checks
				CheckRecurringExpenses(context, notificationService).Wait();
				CheckSavingGoals(context, notificationService).Wait();
				CheckBudgets(context, notificationService).Wait();
			}
		}

		private async Task CheckRecurringExpenses(MilaContext context, NotificationService notificationService)
		{
			var today = DateTime.Today;
			var recurringExpenses = await context.RecurringExpenses
				.Include(re => re.User)
				.Where(re => re.NextDueDate <= today)
				.ToListAsync();

			foreach (var recurringExpense in recurringExpenses)
			{
				// Send notifications using NotificationService
				await notificationService.SendNotificationAsync(
					recurringExpense.User,
					"Recurring Expense Due",
					$"Your recurring expense {recurringExpense.Name} is due today."
				);

				// Update next due date for recurring expenses
				recurringExpense.NextDueDate = recurringExpense.NextDueDate.AddMonths(1);
			}

			await context.SaveChangesAsync();
		}

		private async Task CheckSavingGoals(MilaContext context, NotificationService notificationService)
		{
			var nearEndGoals = await context.SavingGoals
				.Include(sg => sg.User)
				.Where(sg => sg.Deadline <= DateTime.Today.AddDays(7))
				.ToListAsync();

			foreach (var goal in nearEndGoals)
			{
				await notificationService.SendNotificationAsync(
					goal.User,
					"Saving Goal Approaching Deadline",
					$"Your saving goal {goal.Name} is nearing its deadline."
				);
			}

			await context.SaveChangesAsync();
		}

		private async Task CheckBudgets(MilaContext context, NotificationService notificationService)
		{
			var budgets = await context.Budgets
				.Include(b => b.User)
				.Where(b => b.CurrentAmount >= b.Limit)
				.ToListAsync();

			foreach (var budget in budgets)
			{
				await notificationService.SendNotificationAsync(
					budget.User,
					"Budget Limit Reached",
					$"Your budget {budget.Name} has reached its limit."
				);
			}

			await context.SaveChangesAsync();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_timer?.Change(Timeout.Infinite, 0); // Stop the timer
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose(); // Clean up resources
		}
	}
}
