import 'package:flutter/material.dart';
import 'package:password_manager_client/widgets/shared/animations/animated_hive_background.dart';
import 'package:password_manager_client/widgets/user_screen/user_screen_container.dart';

class UserScreen extends StatelessWidget {
  const UserScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text("My user")),
      body: const AnimatedHiveBackground(child: UserScreenContainer()),
    );
  }
}