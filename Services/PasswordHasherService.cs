using Microsoft.AspNetCore.Identity;
using MilaAPI.Models;

public class PasswordHasherService
{
	private readonly PasswordHasher<User> _passwordHasher;

	public PasswordHasherService()
	{
		_passwordHasher = new PasswordHasher<User>();
	}

	public string HashPassword(User user, string password)
	{
		return _passwordHasher.HashPassword(user, password);
	}

	public PasswordVerificationResult VerifyPassword(User user, string password)
	{
		return _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
	}
}
