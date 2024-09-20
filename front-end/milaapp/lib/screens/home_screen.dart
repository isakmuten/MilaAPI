import 'package:auto_route/auto_route.dart';
import 'package:flutter/material.dart';
import 'package:milaapp/router.dart';

@RoutePage()
class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Dashboard')),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: [
            Expanded(
              child: GridView.count(
                crossAxisCount: 2,
                crossAxisSpacing: 10,
                mainAxisSpacing: 10,
                children: [
                  _buildDashboardCard(context, 'Transactions', TransactionRoute()),
                  _buildDashboardCard(context, 'Budgets', BudgetRoute()),
                  _buildDashboardCard(context, 'Categories', CategoryRoute()),
                  _buildDashboardCard(context, 'Login', LoginRoute()),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildDashboardCard(BuildContext context, String title, PageRouteInfo route) {
    return GestureDetector(
      onTap: () {
        // Use AutoRoute for navigation
        context.router.push(route);
      },
      child: Card(
        elevation: 4,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
        child: Center(
          child: Text(
            title,
            style: Theme.of(context).textTheme.titleLarge,
          ),
        ),
      ),
    );
  }
}
