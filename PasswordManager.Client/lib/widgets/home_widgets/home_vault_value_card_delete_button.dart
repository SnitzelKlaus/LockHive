import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:password_manager_client/models/blocs/vault_bloc/bloc/vault_bloc.dart';
import 'package:password_manager_client/models/interfaces/i_vault_value.dart';

class HomeVaultValueCardDeleteButton extends StatelessWidget {
  const HomeVaultValueCardDeleteButton({super.key, required this.vaultValue});
  final IVaultValue vaultValue;
  @override
  Widget build(BuildContext context) {
    return Container(
      alignment: Alignment.bottomRight,
      child: ElevatedButton(
          style: ElevatedButton.styleFrom(
              backgroundColor: Color.fromARGB(141, 202, 202, 202)),
          onPressed: () async {
            BlocProvider.of<VaultBloc>(context)
                .eventSink
                .add(DeleteVaultValue(vaultValue));
          },
          child: const Text(
            "Delete",
            style: TextStyle(
              fontSize: 12,
            ),
          )),
    );
  }
}
