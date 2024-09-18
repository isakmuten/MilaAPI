using MilaAPI.Models;

namespace MilaAPI.DTOs
{
	public class RecurringExpenseDto
	{
		public int UserId { get; set; }
		public int CategoryId { get; set; }
		public decimal Amount { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public RecurrenceType Recurrence { get; set; }
		public bool IsActive { get; set; }
	}
}
