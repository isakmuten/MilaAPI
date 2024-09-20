class Budget {
  final String id;
  final double totalAmount;
  final double currentAmount;
  final DateTime startDate;
  final DateTime endDate;
  final String userId;

  Budget({
    required this.id,
    required this.totalAmount,
    required this.currentAmount,
    required this.startDate,
    required this.endDate,
    required this.userId,
  });

  factory Budget.fromJson(Map<String, dynamic> json) {
    return Budget(
      id: json['id'],
      totalAmount: json['totalAmount'],
      currentAmount: json['currentAmount'],
      startDate: DateTime.parse(json['startDate']),
      endDate: DateTime.parse(json['endDate']),
      userId: json['userId'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'totalAmount': totalAmount,
      'currentAmount': currentAmount,
      'startDate': startDate.toIso8601String(),
      'endDate': endDate.toIso8601String(),
      'userId': userId,
    };
  }
}
