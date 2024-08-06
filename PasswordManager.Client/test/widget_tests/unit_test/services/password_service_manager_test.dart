import 'package:mocktail/mocktail.dart';
import 'package:password_manager_client/models/dto_models/password.dart';
import 'package:password_manager_client/services/backend_services/api_utilities/api_exception.dart';
import 'package:password_manager_client/services/backend_services/password_api_service/password_api_service.dart';
import 'package:password_manager_client/services/service_managers/password_service/password_service_manager.dart';
import 'package:test/test.dart';
class McokPasswordApiService extends Mock implements PasswordApiService {}
void main(){ 
  PasswordServiceManager serviceManager = PasswordServiceManager();
  test("getPasswrods successfull", () async {

    List<Password> passwords = [Password(passwordId: "1", friendlyName: "name", password: "password", url: "url", username: "username"), Password(passwordId: "2", friendlyName: "name2", password: "password2", url: "url2", username: "username2"), Password(passwordId: "3", friendlyName: "name3", password: "password3", url: "url3", username: "username3")];
    McokPasswordApiService service = McokPasswordApiService();
    when(() => service.getPasswords()).thenAnswer((_) async => passwords);
    serviceManager.passwordApiService = service;

    List<Password> result = await serviceManager.getPasswords();

    expect(result, passwords);
  });
  test("getPasswrods rethows apiException", () async {

    McokPasswordApiService service = McokPasswordApiService();
    when(() => service.getPasswords()).thenThrow(ApiException(400, "From unittest getPasswrods"));
    serviceManager.passwordApiService = service;

    expect(() => serviceManager.getPasswords(), throwsA(isA<ApiException>()));
  });
  test("createPassword successfull", () async {

    Password password = Password(friendlyName: "name", password: "password", url: "url", username: "username");
    McokPasswordApiService service = McokPasswordApiService();
    when(() => service.createPassword(password)).thenAnswer((_) async => true);
    serviceManager.passwordApiService = service;

    bool result = await serviceManager.createPassword(password);

    expect(result, true);
  });
  test("createPassword rethows apiException", () async {

    Password password = Password(friendlyName: "name", password: "password", url: "url", username: "username");
    McokPasswordApiService service = McokPasswordApiService();
    when(() => service.createPassword(password)).thenThrow(ApiException(400, "From unittest createPassword"));
    serviceManager.passwordApiService = service;

    expect(() => serviceManager.createPassword(password), throwsA(isA<ApiException>()));
  });
  test("createPassword throws exception on validation error", () async {

    Password passwordWithId = Password(passwordId: "1", friendlyName: "name", password: "password", url: "url", username: "username");
    Password passwordWithoutFriendlyName = Password(password: "password", url: "url", username: "username");
    Password passwordWithoutPassword = Password(friendlyName: "name", url: "url", username: "username");
    Password passwordWithoutUrl = Password(friendlyName: "name", password: "password", username: "username");
    Password passwordWithoutUsername = Password(friendlyName: "name", password: "password", url: "url",);

    expect(() => serviceManager.createPassword(passwordWithId), throwsA(isA<Exception>()));
    expect(() => serviceManager.createPassword(passwordWithoutFriendlyName), throwsA(isA<Exception>()));
    expect(() => serviceManager.createPassword(passwordWithoutPassword), throwsA(isA<Exception>()));
    expect(() => serviceManager.createPassword(passwordWithoutUrl), throwsA(isA<Exception>()));
    expect(() => serviceManager.createPassword(passwordWithoutUsername), throwsA(isA<Exception>()));
  });
  test("generatePassword successfull", () async {

    McokPasswordApiService service = McokPasswordApiService();
    when(() => service.generatePassword(10)).thenAnswer((_) async => "password10");
    serviceManager.passwordApiService = service;

    String result = await serviceManager.generatePassword(10);

    expect(result, "password10");
  });
  test("generatePassword rethows apiException", () async {

    McokPasswordApiService service = McokPasswordApiService();
    when(() => service.generatePassword(10)).thenThrow(ApiException(400, "From unittest generatePassword"));
    serviceManager.passwordApiService = service;

    expect(() => serviceManager.generatePassword(10), throwsA(isA<ApiException>()));
  });
  test("generatePassword thwors exception on illegal length", () async {
    expect(() => serviceManager.generatePassword(8), throwsA(isA<Exception>()));
    expect(() => serviceManager.generatePassword(129), throwsA(isA<Exception>()));
    expect(() => serviceManager.generatePassword(-1), throwsA(isA<Exception>()));
  });
  test("updatePassword successfull", () async {

    Password password = Password(passwordId: "1", friendlyName: "name", password: "password", url: "url", username: "username");
    McokPasswordApiService service = McokPasswordApiService();
    when(() => service.updatePassword(password)).thenAnswer((_) async => true);
    serviceManager.passwordApiService = service;

    bool result = await serviceManager.updatePassword(password);

    expect(result, true);
  });
  test("updatePassword rethows apiException", () async {
    Password password = Password(passwordId: "1", friendlyName: "name", password: "password", url: "url", username: "username");

    McokPasswordApiService service = McokPasswordApiService();
    when(() => service.updatePassword(password)).thenThrow(ApiException(400, "From unittest updatePassword"));
    serviceManager.passwordApiService = service;

    expect(() => serviceManager.updatePassword(password), throwsA(isA<ApiException>()));
  });
  test("updatePassword throws exception on validation error", () async {

    Password passwordWithoutId = Password(friendlyName: "name", password: "password", url: "url", username: "username");
    Password passwordWithoutFriendlyName = Password(passwordId: "1", password: "password", url: "url", username: "username");
    Password passwordWithoutPassword = Password(passwordId: "1", friendlyName: "name", url: "url", username: "username");
    Password passwordWithoutUrl = Password(passwordId: "1", friendlyName: "name", password: "password", username: "username");
    Password passwordWithoutUsername = Password(passwordId: "1", friendlyName: "name", password: "password", url: "url",);

    expect(() => serviceManager.updatePassword(passwordWithoutId), throwsA(isA<Exception>()));
    expect(() => serviceManager.updatePassword(passwordWithoutFriendlyName), throwsA(isA<Exception>()));
    expect(() => serviceManager.updatePassword(passwordWithoutPassword), throwsA(isA<Exception>()));
    expect(() => serviceManager.updatePassword(passwordWithoutUrl), throwsA(isA<Exception>()));
    expect(() => serviceManager.updatePassword(passwordWithoutUsername), throwsA(isA<Exception>()));
  });
  test("deletePassword successful", () async {

    McokPasswordApiService service = McokPasswordApiService();
    when(() => service.deletePassword("1")).thenAnswer((_) async => true);
    serviceManager.passwordApiService = service;

    bool result = await serviceManager.deletePassword("1");

    expect(result, true);
  });
  test("deletePassword on throws api-exception", () async {
      
    McokPasswordApiService service = McokPasswordApiService();
    when(() => service.deletePassword("1")).thenThrow(ApiException(400, "From unittest deletePassword"));
    serviceManager.passwordApiService = service;
    
    bool result = await serviceManager.deletePassword("1");

    expect(result, false);
  });
}