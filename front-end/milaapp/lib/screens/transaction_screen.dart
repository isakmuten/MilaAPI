import 'package:auto_route/annotations.dart';
import 'package:flutter/material.dart';

@RoutePage()
class TransactionScreen extends StatelessWidget {
  const TransactionScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Transactions')),
      floatingActionButton: FloatingActionButton(
        onPressed: () {
          // Add logic to create new transaction
        },
        child: const Icon(Icons.add),
      ),
      body: ListView.builder(
        itemCount: 10, // Replace with dynamic count
        itemBuilder: (context, index) {
          return Card(
            margin: const EdgeInsets.symmetric(vertical: 8, horizontal: 16),
            child: ListTile(
              leading: const Icon(Icons.monetization_on),
              title: Text('Transaction $index'),
              subtitle: Text('Details about transaction $index'),
              trailing: const Text('-\$100'),
            ),
          );
        },
      ),
    );
  }
}
