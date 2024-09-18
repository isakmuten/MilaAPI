using System;
using System.ComponentModel.DataAnnotations;

namespace MilaAPI.Models
{
	public class Transaction
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public decimal Amount { get; set; }
		
		[Required]
		public decimal Balance { get; set; }

		[Required]
		public DateTime Date { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public TransactionType Type { get; set; } // Could be Enum (Expense, Income, Transfer)

		[Required]
		public int CategoryId { get; set; }
		public Category Category { get; set; } // Navigation Property to Category

		[Required]
		public int UserId { get; set; }
		public User User { get; set; } // Navigation Property to User

		public PaymentMethod PaymentMethod { get; set; } // Payment method (optional)

		// Any other fields like Merchant, ReferenceNumber, etc.
	}

	public enum TransactionType
	{
		Expense,
		Income,
		Transfer
	}

	public enum PaymentMethod
	{
		Cash,
		Card,
		BankTransfer,
		Other
	}
}
