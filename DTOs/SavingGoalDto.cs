namespace MilaAPI.DTOs
{
	public class SavingGoalDto
	{
		public int UserId { get; set; }
		public string Name { get; set; }
		public decimal TargetAmount { get; set; }
		public decimal CurrentAmount { get; set; }
		public DateTime Deadline { get; set; }
		public bool IsAchieved { get; set; }
	}
}
