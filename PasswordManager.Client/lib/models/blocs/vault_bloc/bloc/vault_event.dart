part of 'vault_bloc.dart';

abstract class VaultEvent {
  var logger = Logger();
  execute(VaultState state);
}

class GetVaultValues extends VaultEvent {

  PasswordServiceManager passwordServiceManager = PasswordServiceManager();

  @override
  execute(VaultState state) async {

    var passwordsFuture = passwordServiceManager.getPasswords();
    var passwords = await passwordsFuture;

    state.vaultValue = passwords;

    return state;
  }
}

class DeleteVaultValue extends VaultEvent{
  final IVaultValue _vaultValue;
  DeleteVaultValue(this._vaultValue);

  PasswordServiceManager passwordServiceManager = PasswordServiceManager();

  @override
  execute(VaultState state) async {
    if(_vaultValue.type == VaultValueType.password){
      var result = await passwordServiceManager.deletePassword((_vaultValue as Password).passwordId!);
      if(result == true){
        state.vaultValue = state.vaultValue.where((element) => (element as Password).passwordId != (_vaultValue as Password).passwordId).toList();
      }
      else{
        state.error = true;
      }

    }

    return state;
  }
}