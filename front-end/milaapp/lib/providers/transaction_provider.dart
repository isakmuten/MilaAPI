import 'package:flutter/material.dart';
import '../models/transaction.dart'; // Assuming you have a Transaction model

class TransactionProvider with ChangeNotifier {
  List<Transaction> _transactions = [];

  List<Transaction> get transactions => _transactions;

  void addTransaction(Transaction transaction) {
    _transactions.add(transaction);
    notifyListeners();
  }

  void setTransactions(List<Transaction> transactions) {
    _transactions = transactions;
    notifyListeners();
  }

  void clearTransactions() {
    _transactions = [];
    notifyListeners();
  }
}
