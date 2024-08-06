part of 'vault_bloc.dart';

sealed class VaultState {
  late List<IVaultValue> vaultValue = <IVaultValue>[];
  late bool error = false;
}

final class VaultInitial extends VaultState {}
