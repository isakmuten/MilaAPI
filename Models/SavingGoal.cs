using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilaAPI.Models
{
	public class SavingGoal
	{
		[Key]
		public int Id { get; set; }
		
		[Required]
		public int UserId { get; set; }  // The user to whom the goal belongs
		public User User { get; set; }

		public string Name { get; set; }  // Goal name, e.g., "Buy a car"
		
		[Column(TypeName = "decimal(18, 2)")]
		public decimal TargetAmount { get; set; }  // Amount the user is aiming to save
		[Column(TypeName = "decimal(18, 2)")]
		public decimal CurrentAmount { get; set; }  // Amount saved so far
		
		public DateTime Deadline { get; set; }  // The deadline for achieving the goal
		public bool IsAchieved { get; set; }  // Whether the goal has been reached
	}
}
