using MilaAPI.Models;

public class Category
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }

	// Navigation property for the expenses under this category
	public ICollection<Expense> Expenses { get; set; }
	public bool IsExpenseCategory { get; set; }
}
