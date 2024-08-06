part of 'edit_vault_value_bloc.dart';

sealed class EditVaultValueState {
  late VaultValueType valueType = VaultValueType.password;
  late IVaultValue vaultValue = Password();
  late bool isSaveDone = false;
  late double generatePasswordLength = 64;
  late bool error = false;
}

final class EditVaultValueInitial extends EditVaultValueState {}
