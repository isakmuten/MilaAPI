using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilaAPI.Models
{
	public class Transaction
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[Column(TypeName = "decimal(18, 2)")]
		public decimal Amount { get; set; }

		[Required]
		[Column(TypeName = "decimal(18, 2)")]
		public decimal Balance { get; set; }

		[Required]
		public DateTime Date { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public TransactionType Type { get; set; }


		[Required]
		public int CategoryId { get; set; }

		public Category Category { get; set; }


		[Required]
		public int UserId { get; set; }

		public User User { get; set; }


		public PaymentMethod PaymentMethod { get; set; }

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
