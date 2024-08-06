import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:password_manager_client/models/blocs/create_vault_value_bloc/bloc/edit_vault_value_bloc.dart';

import '../../models/enums/vault_value_type.dart';

class SelectVaultValueTypeSection extends StatelessWidget {
  const SelectVaultValueTypeSection({super.key, required this.value});
  final VaultValueType value;

  @override
  Widget build(BuildContext context) {
    return DropdownMenu(
      dropdownMenuEntries: const <DropdownMenuEntry<dynamic>>[
        DropdownMenuEntry(value: VaultValueType.password, label: "Password"),
        DropdownMenuEntry(value: VaultValueType.creditCard, label: "Credit card"),
      ],
      initialSelection: value,
      onSelected: (value) {
        BlocProvider.of<EditVaultValueBloc>(context)
            .eventSink
            .add(SetVaultValueType(value));
      },
    );
  }
}
