part of 'edit_vault_value_bloc.dart';

abstract class EditVaultValueEvent {
  final Logger logger = Logger();
  execute(EditVaultValueState state);
}

class SetVaultValueType extends EditVaultValueEvent {

  SetVaultValueType(this._type);
  final VaultValueType _type;

  @override
  execute(EditVaultValueState state) {
    state.valueType = _type;
    return state;
  }
  
}

class SaveVaultValue extends EditVaultValueEvent {

  SaveVaultValue(this._vaultValue);
  final IVaultValue _vaultValue;

  PasswordServiceManager passwordServiceManager = PasswordServiceManager();
  @override
  execute(EditVaultValueState state) async {
    try{

    if (_vaultValue.type == VaultValueType.password) {
      if((_vaultValue as Password).passwordId != null){
        await passwordServiceManager.updatePassword(_vaultValue as Password);
      }
      else{
        await passwordServiceManager.createPassword(_vaultValue as Password);
      } 
    }
    }
    on ApiException catch (e) {
      state.error = true;
      logger.e(e);
      return state;
    }

    state.isSaveDone = true;
    return state;
  }
}

class SetVaultValue extends EditVaultValueEvent {

  SetVaultValue(this._vaultValue);
  final IVaultValue _vaultValue;

  @override
  execute(EditVaultValueState state) {
    state.vaultValue = _vaultValue;
    return state;
  }
  
}
class GeneratePassword extends EditVaultValueEvent {

  GeneratePassword(this._tempPassword, this._length);
  final double _length;
  final Password _tempPassword;
  PasswordServiceManager passwordServiceManager = PasswordServiceManager();

  @override
  execute(EditVaultValueState state) async {
    if(state.vaultValue.type != VaultValueType.password){
      throw Exception("Invalid VaultValueType to generate password.");
    }
    try{
      state.vaultValue = _tempPassword;
      (state.vaultValue as Password).password = await passwordServiceManager.generatePassword(_length);

    } 
    on ApiException catch (e) {
      state.error = true;
      logger.e(e);
      return state;
    }

    return state;
  }
  
}
class SetGeneratePasswordLength extends EditVaultValueEvent {

  SetGeneratePasswordLength(this._length);
  final double _length;

  @override
  execute(EditVaultValueState state) {
    state.generatePasswordLength = _length;
    return state;
  }
  
}

class NotifiBloc extends EditVaultValueEvent{
  @override
  execute(EditVaultValueState state) {
    return state;
  }
}

