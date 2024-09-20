class Notification {
  final String id;
  final String message;
  final DateTime dateCreated;
  final bool isRead;
  final String userId;

  Notification({
    required this.id,
    required this.message,
    required this.dateCreated,
    required this.isRead,
    required this.userId,
  });

  factory Notification.fromJson(Map<String, dynamic> json) {
    return Notification(
      id: json['id'],
      message: json['message'],
      dateCreated: DateTime.parse(json['dateCreated']),
      isRead: json['isRead'],
      userId: json['userId'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'message': message,
      'dateCreated': dateCreated.toIso8601String(),
      'isRead': isRead,
      'userId': userId,
    };
  }
}
