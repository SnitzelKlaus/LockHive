import 'package:flutter/material.dart';

class HomeHeaderText extends StatelessWidget {
  const HomeHeaderText({super.key});

  @override
  Widget build(BuildContext context) {
    return Text(
      "Saved Passwords",
      style: Theme.of(context).textTheme.headlineLarge,
    );
  }
}
