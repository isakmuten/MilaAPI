using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilaAPI.Models
{
	public class RecurringExpense
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int UserId { get; set; }  // Related to a User
		public User User { get; set; }

		[Required]
		public int CategoryId { get; set; }  // Related to a Category
		public Category Category { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Amount { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }  // Optional if recurring expense ends at some point

		public RecurrenceType Recurrence { get; set; }  // Daily, Weekly, Monthly, Yearly

		public bool IsActive { get; set; }  // Determines if the recurring expense is active
		public DateTime NextDueDate { get; set; }
		public string Name { get; set; }
	}

	public enum RecurrenceType
	{
		Daily,
		Weekly,
		Monthly,
		Yearly
	}
}
