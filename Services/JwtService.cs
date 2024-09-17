using Microsoft.IdentityModel.Tokens;
using MilaAPI.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace MilaAPI.Services
{

public class JwtService
{
	private readonly string _secretKey;
	private readonly string _issuer;
	private readonly string _audience;

	public JwtService(string secretKey, string issuer, string audience)
	{
		_secretKey = secretKey;
		_issuer = issuer;
		_audience = audience;
	}

	public string GenerateToken(User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(_secretKey);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email)
                // Lägg till andra claims om nödvändigt
            }),
			Expires = DateTime.UtcNow.AddHours(1),
			Issuer = _issuer,
			Audience = _audience,
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(token);
			Console.WriteLine($"Generated JWT: {tokenString}");
			return tokenHandler.WriteToken(token);
	}

	public ClaimsPrincipal ValidateToken(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(_secretKey);

		try
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidIssuer = _issuer,
				ValidAudience = _audience,
				ValidateLifetime = true // Kontrollerar om token har gått ut
			};

			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
			return principal;
		}
		catch
		{
			
			return null;
		}
	}
}
}
