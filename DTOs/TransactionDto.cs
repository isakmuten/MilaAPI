﻿namespace MilaAPI.DTOs
{
	public class TransactionDto
	{
		public int Id { get; set; }
		public decimal Amount { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }
		public string Type { get; set; }
		public int CategoryId { get; set; }
		public int UserId { get; set; }
		public string PaymentMethod { get; set; }
		public decimal Balance { get; set; }

	}
}
