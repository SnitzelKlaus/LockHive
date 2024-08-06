import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:password_manager_client/models/dto_models/password.dart';
import 'package:password_manager_client/models/enums/vault_value_type.dart';
import 'package:password_manager_client/widgets/create_new_vault_values_widgets.dart/password/edit_password_vault_value_input_container.dart';
import 'package:password_manager_client/widgets/create_new_vault_values_widgets.dart/select_vault_value_type_section.dart';
import 'package:password_manager_client/widgets/shared/cards/themed_card.dart';

import '../../models/blocs/create_vault_value_bloc/bloc/edit_vault_value_bloc.dart';
import '../shared/progress_indicators/circular_generic_progress_indicator.dart';

class CreateVaultValueContainer extends StatefulWidget {
  const CreateVaultValueContainer({super.key});

  @override
  State<CreateVaultValueContainer> createState() =>
      _CreateVaultValueContainerState();
}

class _CreateVaultValueContainerState extends State<CreateVaultValueContainer> {
  @override
  Widget build(BuildContext context) {
    return ThemedCard(
      child: StreamBuilder<EditVaultValueState>(
          stream: BlocProvider.of<EditVaultValueBloc>(context)
              .editVaultValueState,
          initialData: EditVaultValueInitial(),
          builder: (context, snapshot) {
            if (!snapshot.hasData) {
              return const CircularGenericProgessIndicator();
            } else {
              return SingleChildScrollView(
                child: Column(
                  children: [
                    SelectVaultValueTypeSection(
                        value: snapshot.data!.valueType),
                    if (snapshot.data!.valueType == VaultValueType.password)
                      EditPasswordVaultValueInputContainer(
                        newPassword: snapshot.data!.vaultValue as Password,
                        generatePasswordLength:
                            snapshot.data!.generatePasswordLength,
                      ),
                    if (snapshot.data!.valueType == VaultValueType.creditCard) Placeholder(),
                  ],
                ),
              );
            }
          }),
    );
  }
}
