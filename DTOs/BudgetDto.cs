namespace MilaAPI.DTOs
{
	public class BudgetDto
	{
		public int UserId { get; set; }
		public int CategoryId { get; set; }
		public decimal Amount { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public bool IsActive { get; set; }
		public decimal CurrentAmount { get; set; }
		public decimal Limit { get; set; }
		public string Name { get; set; }
	}
}
