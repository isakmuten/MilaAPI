import 'package:auto_route/annotations.dart';
import 'package:flutter/material.dart';

@RoutePage()
class BudgetScreen extends StatelessWidget {
  const BudgetScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Budgets')),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Text(
              'Your Monthly Budget',
              style: Theme.of(context).textTheme.titleLarge,
            ),
            Text(
              '\$5000',
              style: Theme.of(context).textTheme.headlineMedium,
            ),
            const SizedBox(height: 20),
            const Text('You have spent \$3000'),
            const LinearProgressIndicator(
              value: 3000 / 5000,
            ),
          ],
        ),
      ),
    );
  }
}
