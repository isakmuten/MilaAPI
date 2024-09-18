using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilaAPI.Models
{
	public class Budget
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int UserId { get; set; }
		public User User { get; set; }  // Relation with User

		[Required]
		public int CategoryId { get; set; }
		public Category Category { get; set; }  // Relation with Category

		[Column(TypeName = "decimal(18,2)")]
		public decimal Amount { get; set; }  // Budget amount
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public bool IsActive { get; set; }
		
		[Column(TypeName = "decimal(18,2)")]
		public decimal CurrentAmount { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Limit { get; set; }
		public string Name { get; set; }
	}
}
