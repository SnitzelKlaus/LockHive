import 'package:password_manager_client/models/blocs/create_vault_value_bloc/bloc/edit_vault_value_bloc.dart';
import 'package:password_manager_client/models/dto_models/password.dart';
import 'package:password_manager_client/models/enums/vault_value_type.dart';
import 'package:password_manager_client/models/interfaces/i_vault_value.dart';
import 'package:password_manager_client/services/backend_services/api_utilities/api_exception.dart';
import 'package:password_manager_client/services/service_managers/password_service/password_service_manager.dart';
import 'package:test/test.dart';
import 'package:mocktail/mocktail.dart';

class McokPasswordServiceManager extends Mock implements PasswordServiceManager {}
void main() {
  
  test('SetVaultValueType bloc event on passwordType', () async {

    EditVaultValueEvent event = SetVaultValueType(VaultValueType.password);
    EditVaultValueState state = EditVaultValueInitial();


    state = event.execute(state);

    expect(state.valueType, VaultValueType.password);
  });
  test('SetVaultValueType bloc event on paymentCardType', () async {

    EditVaultValueEvent event = SetVaultValueType(VaultValueType.creditCard);
    EditVaultValueState state = EditVaultValueInitial();


    state = event.execute(state);

    expect(state.valueType, VaultValueType.creditCard);
  });
  test('SaveVaultValue bloc event on Password create', () async {
    Password password = Password(friendlyName: "name", password: "password", url: "url", username: "username");
  
    McokPasswordServiceManager serviceManager = McokPasswordServiceManager();
    when(() => serviceManager.createPassword(password)).thenAnswer((_) async => true);
  
    SaveVaultValue event = SaveVaultValue(password);
    event.passwordServiceManager = serviceManager;
    EditVaultValueState state = EditVaultValueInitial();

    state = await event.execute(state);

    expect(state.isSaveDone, true);
    expect(state.error, false);
  });
  test('SaveVaultValue bloc event on Password update', () async {
    Password password = Password(friendlyName: "name", password: "password", url: "url", username: "username", passwordId: "1");
  
    McokPasswordServiceManager serviceManager = McokPasswordServiceManager();
    when(() => serviceManager.updatePassword(password)).thenAnswer((_) async => true);
  
    SaveVaultValue event = SaveVaultValue(password);
    event.passwordServiceManager = serviceManager;
    EditVaultValueState state = EditVaultValueInitial();

    state = await event.execute(state);

    expect(state.isSaveDone, true);
    expect(state.error, false);
  });

  test('SaveVaultValue bloc event on Password update throw ApiException', () async {
    Password password = Password(friendlyName: "name", password: "password", url: "url", username: "username", passwordId: "1");
  
    McokPasswordServiceManager serviceManager = McokPasswordServiceManager();
    when(() => serviceManager.updatePassword(password)).thenThrow(ApiException(400, "Exception thrown from test"));
  
    SaveVaultValue event = SaveVaultValue(password);
    event.passwordServiceManager = serviceManager;
    EditVaultValueState state = EditVaultValueInitial();
    
    state = await event.execute(state);

    expect(state.error, true);
  });

  test('SetVaultValue bloc event', () async {
    Password password = Password(friendlyName: "name", password: "password", url: "url", username: "username", passwordId: "1");
  
    SetVaultValue event = SetVaultValue(password);
    EditVaultValueState state = EditVaultValueInitial();
    
    state = await event.execute(state);

    expect(state.vaultValue, password as IVaultValue);
  });

  test('GeneratePassword bloc event', () async {
    Password password = Password(friendlyName: "name", password: "password", url: "url", username: "username", passwordId: "1");
    GeneratePassword event = GeneratePassword(password, 10);
    EditVaultValueState state = EditVaultValueInitial();
    McokPasswordServiceManager serviceManager = McokPasswordServiceManager();
    when(() => serviceManager.generatePassword(10)).thenAnswer((_) async => "password");
    event.passwordServiceManager = serviceManager;
    

    state = await event.execute(state);

    expect((state.vaultValue as Password).password, "password");
  });

  test('GeneratePassword bloc event on ApiException', () async {
    Password password = Password(friendlyName: "name", password: "password", url: "url", username: "username", passwordId: "1");
    GeneratePassword event = GeneratePassword(password, 10);
    EditVaultValueState state = EditVaultValueInitial();
    McokPasswordServiceManager serviceManager = McokPasswordServiceManager();
    when(() => serviceManager.generatePassword(10)).thenThrow(ApiException(400, "Exception thrown from test"));
    event.passwordServiceManager = serviceManager;

    state = await event.execute(state);

    expect(state.error, true);
  });

  test('GeneratePassword bloc event on not allowed type', () async {
    Password password = Password(friendlyName: "name", password: "password", url: "url", username: "username", passwordId: "1");
    GeneratePassword event = GeneratePassword(password, 10);
    EditVaultValueState state = EditVaultValueInitial();
    state.vaultValue.type = VaultValueType.creditCard;

    expect(() => event.execute(state), throwsA(isA<Exception>()));
  });

  test('SetGeneratePasswordLength bloc event', () async {

    const double length = 100;
    SetGeneratePasswordLength event = SetGeneratePasswordLength(length);
    EditVaultValueState state = EditVaultValueInitial();

    state = await event.execute(state);

    expect(state.generatePasswordLength, length);
  });
}