using Microsoft.AspNetCore.Identity;
using MilaAPI.Models;

public class DataSeeder
{
	public static void SeedUsers(MilaContext context)
	{
		if (!context.Users.Any())
		{
			var passwordHasher = new PasswordHasher<User>();

			var user = new User
			{
				FirstName = "Isak",
				LastName = "Muten",
				Email = "isak@muten.com",
				IsActive = true,
				DateCreated = DateTime.Now
			};

			user.PasswordHash = passwordHasher.HashPassword(user, "isakisak123");

			context.Users.Add(user);
			context.SaveChanges();
		}
	}
}
