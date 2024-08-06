import 'package:flutter/material.dart';
import 'package:password_manager_client/widgets/shared/animations/animated_hive_background.dart';

import '../widgets/create_new_vault_values_widgets.dart/create_vault_value_container.dart';

class CreateVaultValueScreen extends StatelessWidget {
  const CreateVaultValueScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text("Create new vault value")),
      body: const AnimatedHiveBackground(child: CreateVaultValueContainer()),
    );
  }
}
