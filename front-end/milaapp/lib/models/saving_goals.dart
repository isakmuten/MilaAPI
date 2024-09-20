class SavingGoal {
  final String id;
  final double targetAmount;
  final double currentAmount;
  final String description;
  final DateTime deadline;
  final String userId;

  SavingGoal({
    required this.id,
    required this.targetAmount,
    required this.currentAmount,
    required this.description,
    required this.deadline,
    required this.userId,
  });

  factory SavingGoal.fromJson(Map<String, dynamic> json) {
    return SavingGoal(
      id: json['id'],
      targetAmount: json['targetAmount'],
      currentAmount: json['currentAmount'],
      description: json['description'],
      deadline: DateTime.parse(json['deadline']),
      userId: json['userId'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'targetAmount': targetAmount,
      'currentAmount': currentAmount,
      'description': description,
      'deadline': deadline.toIso8601String(),
      'userId': userId,
    };
  }
}
