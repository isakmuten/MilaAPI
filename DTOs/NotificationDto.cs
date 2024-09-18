namespace MilaAPI.DTOs
{
	public class NotificationDto
	{
		public int UserId { get; set; }
		public string Message { get; set; }
		public DateTime DateCreated { get; set; }
		public bool IsRead { get; set; }
		public string Type { get; set; }
	}
}
