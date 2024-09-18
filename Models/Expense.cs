using System.ComponentModel.DataAnnotations.Schema;

namespace MilaAPI.Models
{
	public class Expense
	{
		public int Id { get; set; }

		[Column(TypeName = "decimal(18,2)")] // This ensures the SQL column is created with the correct precision and scale
		public decimal Amount { get; set; }
		public DateTime Date { get; set; } // Date the expense was made
		public string Description { get; set; } // Optional description of the expense

		// Foreign key to the User entity
		public int UserId { get; set; }
		public User User { get; set; } // Navigation property

		// Foreign key to Category entity
		public int CategoryId { get; set; }
		public Category Category { get; set; } // Navigation property
	}
}
