import 'package:flutter/material.dart';
import 'package:password_manager_client/widgets/shared/animations/animated_hive_background.dart';
import 'package:password_manager_client/widgets/vault_value_screen/vault_value_container.dart';


class VaultValueScreen extends StatelessWidget {
  const VaultValueScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Vault Value"),
      ),
      body: AnimatedHiveBackground(child: VaultValueContainer()),
    );
  }
}
