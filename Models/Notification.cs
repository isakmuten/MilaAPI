using System;
using System.ComponentModel.DataAnnotations;

namespace MilaAPI.Models
{
	public class Notification
	{
		[Key]
		public int Id { get; set; }
		
		[Required]
		public int UserId { get; set; }
		public User User { get; set; }

		public string Message { get; set; }  // Notification message content
		public DateTime DateCreated { get; set; }  // When the notification was created
		public bool IsRead { get; set; }  // Whether the user has read the notification
		public string Type { get; set; }  // Type of notification (e.g., "ExpenseAlert", "GoalReminder")
	}
}
