import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:password_manager_client/models/blocs/create_vault_value_bloc/bloc/edit_vault_value_bloc.dart';
import 'package:password_manager_client/models/dto_models/password.dart';
import 'package:password_manager_client/models/enums/vault_value_type.dart';
import 'package:password_manager_client/widgets/create_new_vault_values_widgets.dart/password/edit_password_vault_value_input_container.dart';
import 'package:password_manager_client/widgets/shared/cards/themed_card.dart';
import 'package:password_manager_client/widgets/shared/progress_indicators/circular_generic_progress_indicator.dart';


class VaultValueContainer extends StatelessWidget {
  const VaultValueContainer({super.key, });
  @override
  Widget build(BuildContext context) {
    BlocProvider.of<EditVaultValueBloc>(context).eventSink.add(NotifiBloc());
    return Padding(
      padding: const EdgeInsets.all(16.0),
      child: ThemedCard(
        child: StreamBuilder<EditVaultValueState>(
          
            stream: BlocProvider.of<EditVaultValueBloc>(context)
                .editVaultValueState,
            builder: (context, snapshot) {
              if (!snapshot.hasData) {
                return const CircularGenericProgessIndicator();
              } else {
          return SingleChildScrollView(
            child: switch (snapshot.data!.vaultValue.type) {

              VaultValueType.password => EditPasswordVaultValueInputContainer(newPassword: snapshot.data!.vaultValue as Password, generatePasswordLength: snapshot.data!.generatePasswordLength,),
              VaultValueType.creditCard => Container(),
            },
            
          );
              }
            }
        ),
      ),
    );
  }
}
