using Microsoft.EntityFrameworkCore;
using MilaAPI.Models;

public class MilaContext : DbContext
{
	public MilaContext(DbContextOptions<MilaContext> options) : base(options) { }

	public DbSet<User> Users { get; set; }
	public DbSet<Expense> Expenses { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<Transaction> Transactions { get; set; }
	public DbSet<Budget> Budgets { get; set; }
	public DbSet<RecurringExpense> RecurringExpenses { get; set; }
	public DbSet<SavingGoal> SavingGoals { get; set; }
	public DbSet<Notification> Notifications { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Specify precision and scale for the Amount property
		modelBuilder.Entity<Expense>()
			.Property(e => e.Amount)
			.HasPrecision(18, 2); // 18 digits total, 2 digits after decimal point

		modelBuilder.Entity<Transaction>()
		.Property(t => t.Amount)
		.HasColumnType("decimal(18, 2)");

		modelBuilder.Entity<Transaction>()
			.Property(t => t.Balance)
			.HasColumnType("decimal(18, 2)");

		modelBuilder.Entity<Budget>()
			.Property(t => t.Amount)
			.HasColumnType("decimal(18, 2)");

		modelBuilder.Entity<Budget>()
			.Property(t => t.CurrentAmount)
			.HasColumnType("decimal(18, 2)");

		modelBuilder.Entity<Budget>()
			.Property(t => t.Limit)
			.HasColumnType("decimal(18, 2)");

		modelBuilder.Entity<RecurringExpense>()
			.Property(t => t.Amount)
			.HasColumnType("decimal(18, 2)");

		modelBuilder.Entity<SavingGoal>()
			.Property(t => t.TargetAmount)
			.HasColumnType("decimal(18, 2)");

		modelBuilder.Entity<SavingGoal>()
			.Property(t => t.CurrentAmount)
			.HasColumnType("decimal(18, 2)");

		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Category>().HasData(
			new Category { Id = 1, Name = "Food", Description = "Expenses for food and groceries" },
			new Category { Id = 2, Name = "Rent", Description = "Monthly rent payments" },
			new Category { Id = 3, Name = "Entertainment", Description = "Movies, concerts, and other fun activities" }
		);
	}

}
