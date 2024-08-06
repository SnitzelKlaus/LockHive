import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:password_manager_client/models/blocs/create_vault_value_bloc/bloc/edit_vault_value_bloc.dart';
import 'package:password_manager_client/models/blocs/vault_bloc/bloc/vault_bloc.dart';
import 'package:password_manager_client/models/interfaces/i_vault_value.dart';

class HomeVaultValueCardOpenButton extends StatelessWidget {
  const HomeVaultValueCardOpenButton({super.key, required this.vaultValue});
  final IVaultValue vaultValue;

  @override
  Widget build(BuildContext context) {
    return Container(
      alignment: Alignment.bottomRight,
      child: ElevatedButton(
          onPressed: () async {
            BlocProvider.of<EditVaultValueBloc>(context)
                .eventSink
                .add(SetVaultValue(vaultValue));
            await Navigator.pushNamed(
              context,
              '/vaultValueScreen',
              arguments: vaultValue,
            );
            await Future.delayed(Duration(seconds: 2));
            BlocProvider.of<VaultBloc>(context).eventSink.add(GetVaultValues());
          },
          child: const Text(
            "Open",
            style: TextStyle(
              fontSize: 12,
            ),
          )),
    );
  }
}
