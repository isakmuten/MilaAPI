class RecurringExpense {
  final String id;
  final double amount;
  final String description;
  final DateTime nextDueDate;
  final String userId;
  final String categoryId;

  RecurringExpense({
    required this.id,
    required this.amount,
    required this.description,
    required this.nextDueDate,
    required this.userId,
    required this.categoryId,
  });

  factory RecurringExpense.fromJson(Map<String, dynamic> json) {
    return RecurringExpense(
      id: json['id'],
      amount: json['amount'],
      description: json['description'],
      nextDueDate: DateTime.parse(json['nextDueDate']),
      userId: json['userId'],
      categoryId: json['categoryId'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'amount': amount,
      'description': description,
      'nextDueDate': nextDueDate.toIso8601String(),
      'userId': userId,
      'categoryId': categoryId,
    };
  }
}
