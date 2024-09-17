using Microsoft.EntityFrameworkCore;
using MilaAPI.Models;

public class MilaContext : DbContext
{
	public MilaContext(DbContextOptions<MilaContext> options) : base(options) { }

	public DbSet<User> Users { get; set; }
}
