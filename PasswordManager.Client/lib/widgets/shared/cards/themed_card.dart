import 'dart:ui';

import 'package:flutter/material.dart';

class ThemedCard extends StatelessWidget {
  const ThemedCard({super.key, required this.child});
  final Widget child;

  @override
  Widget build(BuildContext context) {
    return ClipRect(
      child: BackdropFilter(
        filter: ImageFilter.blur(sigmaX: 1, sigmaY: 1),
        child: Container(
          decoration: BoxDecoration(
              borderRadius: BorderRadius.circular(4),
              color: Theme.of(context).cardColor),
          alignment: Alignment.topLeft,
          child: child,
        ),
      ),
    );
  }
}
