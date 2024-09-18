using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MilaAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext for SQL Server
builder.Services.AddDbContext<MilaContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services.AddSingleton(new JwtService(
	jwtSettings["Key"],
	jwtSettings["Issuer"],
	jwtSettings["Audience"]
));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.Events = new JwtBearerEvents
		{
			OnAuthenticationFailed = context =>
			{
				Console.WriteLine("Authentication failed: " + context.Exception.Message);
				return Task.CompletedTask;
			},
			OnTokenValidated = context =>
			{
				Console.WriteLine("Token validated successfully");
				return Task.CompletedTask;
			}
		};

		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtSettings["Issuer"],
			ValidAudience = jwtSettings["Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
		};
	});

// Add NotificationService as a scoped service
builder.Services.AddScoped<NotificationService>();

// Add TransactionProcessingService as a scoped service
builder.Services.AddScoped<TransactionProcessingService>();

// Add NotificationBackgroundService as a hosted service
builder.Services.AddHostedService<NotificationBackgroundService>();

// Configure Controllers with JSON Options to handle Reference Cycles
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
		options.JsonSerializerOptions.MaxDepth = 64; // Adjust based on your needs
	});

// Configure Swagger for API documentation and JWT Authentication
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Description = "Please enter JWT with Bearer into the field",
		Name = "Authorization",
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
	});
	c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
	{
		{
			new Microsoft.OpenApi.Models.OpenApiSecurityScheme
			{
				Reference = new Microsoft.OpenApi.Models.OpenApiReference
				{
					Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] { }
		}
	});
});

var app = builder.Build();

// Apply migrations and seed the database
using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<MilaContext>();

	// Apply any pending migrations
	context.Database.Migrate();

	// Call the SeedUsers method to seed users
	DataSeeder.SeedUsers(context);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
