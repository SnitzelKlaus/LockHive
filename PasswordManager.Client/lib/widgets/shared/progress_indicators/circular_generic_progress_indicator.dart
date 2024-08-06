import 'package:flutter/material.dart';

class CircularGenericProgessIndicator extends StatelessWidget {
  const CircularGenericProgessIndicator({super.key});

  @override
  Widget build(BuildContext context) {
    return const Align(
        alignment: Alignment.topCenter, child: CircularProgressIndicator());
  }
}
