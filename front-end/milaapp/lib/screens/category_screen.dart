import 'package:auto_route/annotations.dart';
import 'package:flutter/material.dart';

@RoutePage()
class CategoryScreen extends StatelessWidget {
  const CategoryScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Categories')),
      floatingActionButton: FloatingActionButton(
        onPressed: () {
          // Add logic to create a new category
        },
        child: const Icon(Icons.add),
      ),
      body: ListView.builder(
        itemCount: 5, // Replace with dynamic count
        itemBuilder: (context, index) {
          return Card(
            margin: const EdgeInsets.symmetric(vertical: 8, horizontal: 16),
            child: ListTile(
              leading: const Icon(Icons.category),
              title: Text('Category $index'),
              subtitle: Text('Details about category $index'),
            ),
          );
        },
      ),
    );
  }
}
