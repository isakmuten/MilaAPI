class Expense {
  final String id;
  final double amount;
  final DateTime date;
  final String description;
  final String categoryId;
  final String userId;

  Expense({
    required this.id,
    required this.amount,
    required this.date,
    required this.description,
    required this.categoryId,
    required this.userId,
  });

  factory Expense.fromJson(Map<String, dynamic> json) {
    return Expense(
      id: json['id'],
      amount: json['amount'],
      date: DateTime.parse(json['date']),
      description: json['description'],
      categoryId: json['categoryId'],
      userId: json['userId'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'amount': amount,
      'date': date.toIso8601String(),
      'description': description,
      'categoryId': categoryId,
      'userId': userId,
    };
  }
}
