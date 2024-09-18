using Microsoft.EntityFrameworkCore;
using MilaAPI.Models;

public class MilaContext : DbContext
{
	public MilaContext(DbContextOptions<MilaContext> options) : base(options) { }

	public DbSet<User> Users { get; set; }
	public DbSet<Expense> Expenses { get; set; }
	public DbSet<Category> Categories { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Specify precision and scale for the Amount property
		modelBuilder.Entity<Expense>()
			.Property(e => e.Amount)
			.HasPrecision(18, 2); // 18 digits total, 2 digits after decimal point
	}
}
