using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilaAPI.Models;
using MilaAPI.DTOs;
using MilaAPI.Services;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
	private readonly MilaContext _context;
	private readonly JwtService _jwtService;

	public UsersController(MilaContext context, JwtService jwtService)
	{
		_context = context;
		_jwtService = jwtService;
	}

	// POST: api/Users/register
	[HttpPost("register")]
	public async Task<ActionResult<User>> RegisterUser(UserRegistrationDto userDto)
	{
		// Kontrollera om e-post redan används
		if (_context.Users.Any(u => u.Email == userDto.Email))
		{
			return BadRequest("Email is already in use");
		}

		var user = new User
		{
			FirstName = userDto.FirstName,
			LastName = userDto.LastName,

			Email = userDto.Email,
			DateCreated = DateTime.Now,
			IsActive = true
		};

		// Hasha lösenordet
		var passwordHasher = new PasswordHasher<User>();
		user.PasswordHash = passwordHasher.HashPassword(user, userDto.Password);

		// Spara användaren i databasen
		_context.Users.Add(user);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
	}

	// POST: api/Users/login
	[HttpPost("login")]
	public async Task<ActionResult<string>> LoginUser(UserLoginDto userDto)
	{
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
		if (user == null)
		{
			Console.WriteLine("Email not found.");
			return Unauthorized("Invalid email or password");
		}

		var passwordHasher = new PasswordHasher<User>();
		var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userDto.Password);

		if (passwordVerificationResult == PasswordVerificationResult.Failed)
		{
			Console.WriteLine("Password verification failed.");
			return Unauthorized("Invalid email or password");
		}

		// Generera JWT-token efter lyckad inloggning
		var token = _jwtService.GenerateToken(user);

		return Ok(new
		{
			access_token = token,
			token_type = "bearer"
		});
	}

	// GET: api/Users/{id}
	[HttpGet("{id}")]
	[Authorize]
	public async Task<ActionResult<User>> GetUser(int id)
	{
		var user = await _context.Users.FindAsync(id);
		if (user == null)
		{
			return NotFound();
		}
		return Ok(user);
	}

	// GET: api/Users
	[HttpGet]
	[Authorize]// Skyddad endpoint - kräver JWT
	public async Task<ActionResult<IEnumerable<User>>> GetUsers()
	{
		return await _context.Users.ToListAsync();
	}

	// PUT: api/Users/{id}
	[HttpPut("{id}")]
	[Authorize]// Skyddad endpoint - kräver JWT
	public async Task<IActionResult> UpdateUser(int id, User user)
	{
		if (id != user.Id)
		{
			return BadRequest();
		}

		_context.Entry(user).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!UserExists(id))
			{
				return NotFound();
			}
			else
			{
				throw;
			}
		}

		return NoContent();
	}

	// DELETE: api/Users/{id}
	[HttpDelete("{id}")]
	[Authorize]// Skyddad endpoint - kräver JWT
	public async Task<IActionResult> DeleteUser(int id)
	{
		var user = await _context.Users.FindAsync(id);
		if (user == null)
		{
			return NotFound();
		}

		_context.Users.Remove(user);
		await _context.SaveChangesAsync();

		return NoContent();
	}

	private bool UserExists(int id)
	{
		return _context.Users.Any(e => e.Id == id);
	}
}
