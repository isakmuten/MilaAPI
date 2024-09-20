import 'package:flutter/material.dart';

class UserProvider with ChangeNotifier {
  String? _token;
  bool _isAuthenticated = false;

  bool get isAuthenticated => _isAuthenticated;
  String? get token => _token;

  void login(String token) {
    _token = token;
    _isAuthenticated = true;
    notifyListeners();
  }

  void logout() {
    _token = null;
    _isAuthenticated = false;
    notifyListeners();
  }
}
