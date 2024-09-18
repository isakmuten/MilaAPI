namespace MilaAPI.DTOs
{
	public class ExpenseDto
	{
		public decimal Amount { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }
		public int CategoryId { get; set; }
		public int UserId { get; set; }
	}
}
