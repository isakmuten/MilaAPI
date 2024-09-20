import 'dart:convert';
import 'package:http/http.dart' as http;

class ApiService {
  // Use the correct base URL depending on the platform.
  final String baseUrl = 'http://192.168.158.24:7199';
  Future<Map<String, dynamic>> signupUser(String email, String password) async {
    final response = await http.post(
      Uri.parse('$baseUrl/api/user/signup'),
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({
        'email': email,
        'password': password,
      }),
    );

    if (response.statusCode == 200 || response.statusCode == 201) {
      return jsonDecode(response.body);
    } else {
      throw Exception('Failed to sign up user');
    }
  }

  Future<Map<String, dynamic>> loginUser(String email, String password) async {
    final response = await http.post(
      Uri.parse('$baseUrl/api/user/login'),
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({
        'email': email,
        'password': password,
      }),
    );

    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    } else {
      throw Exception('Failed to log in');
    }
  }
}
