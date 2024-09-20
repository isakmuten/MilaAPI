import 'package:flutter/foundation.dart';

// Enum for Transaction Type
enum TransactionType {
  income,
  expense,
}

// Enum for Payment Method
enum PaymentMethod {
  creditCard,
  debitCard,
  applePay,
  googlePay,
  bankTransfer,
  cash,
}

// Enum for Transaction Status
enum TransactionStatus {
  pending,
  completed,
  failed,
}

// The Transaction model
class Transaction {
  final String id; // Unique ID for the transaction
  final double amount; // Transaction amount
  final DateTime date; // Transaction date
  final String description; // Transaction description
  final String categoryId; // Category ID for the transaction
  final String userId; // User ID linked to the transaction
  final PaymentMethod paymentMethod; // Payment method used
  final TransactionStatus status; // Status of the transaction
  final TransactionType type; // Type (income or expense)

  // Constructor with named parameters
  Transaction({
    required this.id,
    required this.amount,
    required this.date,
    required this.description,
    required this.categoryId,
    required this.userId,
    required this.paymentMethod,
    required this.status,
    required this.type,
  });

  // Factory method to create a Transaction object from JSON (API response)
  factory Transaction.fromJson(Map<String, dynamic> json) {
    return Transaction(
      id: json['id'] as String,
      amount: json['amount'] as double,
      date: DateTime.parse(json['date'] as String),
      description: json['description'] as String,
      categoryId: json['categoryId'] as String,
      userId: json['userId'] as String,
      paymentMethod: PaymentMethod.values.firstWhere((e) => describeEnum(e) == json['paymentMethod']),
      status: TransactionStatus.values.firstWhere((e) => describeEnum(e) == json['status']),
      type: TransactionType.values.firstWhere((e) => describeEnum(e) == json['type']),
    );
  }

  // Convert Transaction object to JSON (for API requests)
  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'amount': amount,
      'date': date.toIso8601String(),
      'description': description,
      'categoryId': categoryId,
      'userId': userId,
      'paymentMethod': describeEnum(paymentMethod),
      'status': describeEnum(status),
      'type': describeEnum(type),
    };
  }

  // Validate the transaction (this could be expanded with more complex logic)
  bool validate() {
    if (amount <= 0) return false;
    if (description.isEmpty) return false;
    return true;
  }
}
