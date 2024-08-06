import 'package:password_manager_client/models/blocs/vault_bloc/bloc/vault_bloc.dart';
import 'package:password_manager_client/models/dto_models/password.dart';
import 'package:password_manager_client/models/interfaces/i_vault_value.dart';
import 'package:password_manager_client/services/service_managers/password_service/password_service_manager.dart';
import 'package:test/test.dart';
import 'package:mocktail/mocktail.dart';

class McokPasswordServiceManager extends Mock implements PasswordServiceManager {

}
void main() {
  
  test('GetVaultValues bloc event', () async {
    List<Password> passwords = [Password(passwordId: "233", friendlyName: "3", password: "password3", url: "url3", username: "username3"), Password(passwordId: "1", friendlyName: "name", password: "password", url: "url", username: "username"), Password(passwordId: "323", friendlyName: "name2", password: "password2", url: "url2", username: "username2")];
    
    McokPasswordServiceManager serviceManager = McokPasswordServiceManager();

    when(() => serviceManager.getPasswords()).thenAnswer((_) async => passwords);

    GetVaultValues event = GetVaultValues();
    event.passwordServiceManager = serviceManager;
    VaultState state = VaultInitial();

    state = await event.execute(state);


    List<IVaultValue> vaultValue = [];
    vaultValue.addAll(passwords);

    expect(state.vaultValue, vaultValue);
  });

  test("DeleteVaultValue bloc event successful", () async {
    Password password = Password(passwordId: "1", friendlyName: "name", password: "password", url: "url", username: "username");
    McokPasswordServiceManager serviceManager = McokPasswordServiceManager();

    when(() => serviceManager.deletePassword(password.passwordId!)).thenAnswer((_) async => true);

    DeleteVaultValue event = DeleteVaultValue(password);
    event.passwordServiceManager = serviceManager;
    VaultState state = VaultInitial();
    state.vaultValue.add(password);

    state = await event.execute(state);

    expect(state.vaultValue.length, 0);
  });

  test("DeleteVaultValue bloc event fail", () async {
    Password password = Password(passwordId: "1", friendlyName: "name", password: "password", url: "url", username: "username");
    McokPasswordServiceManager serviceManager = McokPasswordServiceManager();

    when(() => serviceManager.deletePassword(password.passwordId!)).thenAnswer((_) async => false);

    DeleteVaultValue event = DeleteVaultValue(password);
    event.passwordServiceManager = serviceManager;
    VaultState state = VaultInitial();
    state.vaultValue.add(password);

    state = await event.execute(state);

    expect(state.vaultValue.length, 1);
    expect(state.error, true);
  });
}