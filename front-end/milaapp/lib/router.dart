import 'package:auto_route/auto_route.dart';
import 'package:flutter/material.dart';
import 'package:milaapp/screens/budget_screen.dart';
import 'package:milaapp/screens/category_screen.dart';
import 'package:milaapp/screens/home_screen.dart';
import 'package:milaapp/screens/login_screen.dart';
import 'package:milaapp/screens/sign_up_screen.dart';
import 'package:milaapp/screens/transaction_screen.dart';

part 'router.gr.dart';

@AutoRouterConfig()
class AppRouter extends RootStackRouter {
  @override
  List<AutoRoute> get routes => [
        AutoRoute(page: HomeRoute.page, initial: true),
        AutoRoute(page: TransactionRoute.page),
        AutoRoute(page: LoginRoute.page),
        AutoRoute(page: BudgetRoute.page),
        AutoRoute(page: CategoryRoute.page),
        AutoRoute(page: SignupRoute.page),
      ];
}

class MyApp extends StatelessWidget {
  final _appRouter = AppRouter();

  @override
  Widget build(BuildContext context) {
    return MaterialApp.router(
      routerDelegate: _appRouter.delegate(),
      routeInformationParser: _appRouter.defaultRouteParser(),
    );
  }
}
