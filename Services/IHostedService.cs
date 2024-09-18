using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MilaAPI.Models;
using MilaAPI.DTOs;
using MilaAPI.Controllers;

namespace MilaAPI.Services
{
	public class NotificationService : IHostedService, IDisposable
	{
		private readonly IServiceProvider _serviceProvider;
		private Timer _timer;

		public NotificationService(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			// Set up the timer to trigger notifications every hour or at a desired interval
			_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));
			return Task.CompletedTask;
		}

		private void DoWork(object state)
		{
			using (var scope = _serviceProvider.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<MilaContext>();

				// Example 1: Trigger notifications for recurring expenses
				CheckRecurringExpenses(context);

				// Example 2: Trigger notifications for nearing saving goals
				CheckSavingGoals(context);

				// Example 3: Trigger notifications for budget limits
				CheckBudgets(context);
			}
		}

		private void CheckRecurringExpenses(MilaContext context)
		{
			var today = DateTime.Today;
			var recurringExpenses = context.RecurringExpenses
				.Include(re => re.User)
				.Where(re => re.NextDueDate <= today)
				.ToList();

			foreach (var recurringExpense in recurringExpenses)
			{
				// Send notification to the user
				context.Notifications.Add(new Notification
				{
					UserId = recurringExpense.UserId,
					Message = $"Your recurring expense {recurringExpense.Name} is due.",
					DateCreated = DateTime.Now,
					IsRead = false,
					Type = "RecurringExpense"
				});

				// Update next due date for the recurring expense
				recurringExpense.NextDueDate = recurringExpense.NextDueDate.AddMonths(1); // or other frequency
			}

			context.SaveChanges();
		}

		private void CheckSavingGoals(MilaContext context)
		{
			var nearEndGoals = context.SavingGoals
				.Include(sg => sg.User)
				.Where(sg => sg.Deadline <= DateTime.Today.AddDays(7)) // Notify if goal ends within a week
				.ToList();

			foreach (var goal in nearEndGoals)
			{
				context.Notifications.Add(new Notification
				{
					UserId = goal.UserId,
					Message = $"Your saving goal {goal.Name} is nearing its end date.",
					DateCreated = DateTime.Now,
					IsRead = false,
					Type = "SavingGoal"
				});
			}

			context.SaveChanges();
		}

		private void CheckBudgets(MilaContext context)
		{
			var budgets = context.Budgets
				.Include(b => b.User)
				.Where(b => b.CurrentAmount >= b.Limit) // Check if the budget limit is reached
				.ToList();

			foreach (var budget in budgets)
			{
				context.Notifications.Add(new Notification
				{
					UserId = budget.UserId,
					Message = $"Your budget {budget.Name} has reached its limit.",
					DateCreated = DateTime.Now,
					IsRead = false,
					Type = "BudgetLimit"
				});
			}

			context.SaveChanges();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_timer?.Change(Timeout.Infinite, 0);
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}
