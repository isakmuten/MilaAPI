using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilaAPI.Models;
using MilaAPI.DTOs;

namespace MilaAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class NotificationController : ControllerBase
	{
		private readonly MilaContext _context;

		public NotificationController(MilaContext context)
		{
			_context = context;
		}

		// GET: api/Notification
		[HttpGet]
		public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications()
		{
			var notifications = await _context.Notifications
				.Include(n => n.User)  // Load related User
				.Select(n => new NotificationDto
				{
					UserId = n.UserId,
					Message = n.Message,
					DateCreated = n.DateCreated,
					IsRead = n.IsRead,
					Type = n.Type
				})
				.ToListAsync();

			return Ok(notifications);
		}

		// GET: api/Notification/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<Notification>> GetNotification(int id)
		{
			var notification = await _context.Notifications
				.Include(n => n.User)
				.FirstOrDefaultAsync(n => n.Id == id);

			if (notification == null)
			{
				return NotFound();
			}

			return Ok(notification);
		}

		// POST: api/Notification
		[HttpPost]
		public async Task<ActionResult<Notification>> CreateNotification(NotificationDto notificationDto)
		{
			var user = await _context.Users.FindAsync(notificationDto.UserId);
			if (user == null)
			{
				return BadRequest("User not found");
			}

			var notification = new Notification
			{
				UserId = notificationDto.UserId,
				Message = notificationDto.Message,
				DateCreated = DateTime.Now,
				IsRead = notificationDto.IsRead,
				Type = notificationDto.Type
			};

			_context.Notifications.Add(notification);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetNotification), new { id = notification.Id }, notification);
		}

		// PUT: api/Notification/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateNotification(int id, NotificationDto notificationDto)
		{
			var notification = await _context.Notifications.FindAsync(id);
			if (notification == null)
			{
				return NotFound();
			}

			notification.Message = notificationDto.Message;
			notification.IsRead = notificationDto.IsRead;
			notification.Type = notificationDto.Type;

			_context.Entry(notification).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!NotificationExists(id))
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

		// DELETE: api/Notification/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteNotification(int id)
		{
			var notification = await _context.Notifications.FindAsync(id);
			if (notification == null)
			{
				return NotFound();
			}

			_context.Notifications.Remove(notification);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool NotificationExists(int id)
		{
			return _context.Notifications.Any(n => n.Id == id);
		}
	}
}
