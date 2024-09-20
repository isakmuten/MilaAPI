import 'package:flutter/material.dart';
import 'package:milaapp/services/api_service.dart';
import 'package:auto_route/auto_route.dart';
import 'package:milaapp/router.dart';

@RoutePage()
class SignupScreen extends StatelessWidget {
  const SignupScreen({super.key});

  @override
  Widget build(BuildContext context) {
    final emailController = TextEditingController();
    final passwordController = TextEditingController();
    final confirmPasswordController = TextEditingController();
    final apiService = ApiService(); // Initialize API service

    return Scaffold(
      appBar: AppBar(title: const Text('Sign Up')),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            TextFormField(
              controller: emailController,
              decoration: const InputDecoration(labelText: 'Email'),
            ),
            const SizedBox(height: 16),
            TextFormField(
              controller: passwordController,
              decoration: const InputDecoration(labelText: 'Password'),
              obscureText: true,
            ),
            const SizedBox(height: 16),
            TextFormField(
              controller: confirmPasswordController,
              decoration: const InputDecoration(labelText: 'Confirm Password'),
              obscureText: true,
            ),
            const SizedBox(height: 16),
            ElevatedButton(
              onPressed: () async {
                if (_validateSignup(emailController.text, passwordController.text, confirmPasswordController.text)) {
                  try {
                    final response = await apiService.signupUser(
                      emailController.text,
                      passwordController.text,
                    );
                    // Assuming the response contains a token or success message
                    ScaffoldMessenger.of(context).showSnackBar(
                      SnackBar(content: Text('Sign up successful: ${response['message']}')),
                    );
                    context.router.push(HomeRoute());
                  } catch (error) {
                    ScaffoldMessenger.of(context).showSnackBar(
                      SnackBar(content: Text('Error: $error')),
                    );
                  }
                } else {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(content: Text('Invalid signup details')),
                  );
                }
              },
              child: const Text('Sign Up'),
            ),
          ],
        ),
      ),
    );
  }

  bool _validateSignup(String email, String password, String confirmPassword) {
    if (email.isEmpty || !email.contains('@')) {
      return false; // Invalid email
    }
    if (password.isEmpty || confirmPassword.isEmpty || password != confirmPassword) {
      return false; // Passwords do not match or are empty
    }
    return true; // All validations pass
  }
}
