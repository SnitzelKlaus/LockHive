

import 'dart:convert';

import 'package:flutter_test/flutter_test.dart';
import 'package:mocktail/mocktail.dart';
import 'package:password_manager_client/models/dto_models/password.dart';
import 'package:password_manager_client/services/backend_services/api_endpoints/api_endpoints.dart';
import 'package:password_manager_client/services/backend_services/api_utilities/api_exception.dart';
import 'package:password_manager_client/services/backend_services/password_api_service/password_api_service.dart';
import 'package:password_manager_client/services/http_executor/http_executor.dart';
import 'package:http/http.dart' as http;


class HttpExecutorMock extends Mock implements HttpExecutor{}
class FakeUri extends Fake implements Uri {}
void main(){
   setUpAll(() {
    registerFallbackValue(FakeUri());
  });
  PasswordApiService passwordApiService = PasswordApiService();
  test("PasswordApiService.getPasswrods successfull", () async {
    List<Password> passwords = [Password(passwordId: "1", friendlyName: "name", password: "password", url: "url", username: "username"), Password(passwordId: "2", friendlyName: "name2", password: "password2", url: "url2", username: "username2"), Password(passwordId: "3", friendlyName: "name3", password: "password3", url: "url3", username: "username3")];
    HttpExecutorMock httpExecutor = HttpExecutorMock();
    when(() => httpExecutor.get(ApiEndpoints().passwordsUri())).thenAnswer((_) async => http.Response(jsonEncode(passwords.map((e) => e.toJson()).toList()), 200));
    passwordApiService.httpExecutor = httpExecutor;

    List<Password> result = await passwordApiService.getPasswords();

    expect(result.toString(), passwords.toString());
  });
  test("PasswordApiService.getPasswrods throws apiException on non-sucessfull statuscode", () async {
    List<Password> passwords = [Password(passwordId: "1", friendlyName: "name", password: "password", url: "url", username: "username"), Password(passwordId: "2", friendlyName: "name2", password: "password2", url: "url2", username: "username2"), Password(passwordId: "3", friendlyName: "name3", password: "password3", url: "url3", username: "username3")];
    HttpExecutorMock httpExecutor = HttpExecutorMock();
    passwordApiService.httpExecutor = httpExecutor;

    when(() => httpExecutor.get(ApiEndpoints().passwordsUri())).thenAnswer((_) async => http.Response(jsonEncode(passwords.map((e) => e.toJson()).toList()), 400));
    expect(() => passwordApiService.getPasswords(), throwsA(isA<ApiException>()));

    when(() => httpExecutor.get(ApiEndpoints().passwordsUri())).thenAnswer((_) async => http.Response(jsonEncode(passwords.map((e) => e.toJson()).toList()), 500));
    expect(() => passwordApiService.getPasswords(), throwsA(isA<ApiException>()));
  });

  test("PasswordApiService.createPassword successfull", () async {
    Password password = Password(friendlyName: "name", password: "password", url: "url", username: "username");
    HttpExecutorMock httpExecutor = HttpExecutorMock();
    when(() => httpExecutor.post(any(), body: any(named: "body"))).thenAnswer((_) async => http.Response("", 201));
    passwordApiService.httpExecutor = httpExecutor;

    bool result = await passwordApiService.createPassword(password);

    expect(result, true);
  });

  test("PasswordApiService.createPassword throws apiException on non-sucessfull statuscode", () async {
    Password password = Password(friendlyName: "name", password: "password", url: "url", username: "username");
    HttpExecutorMock httpExecutor = HttpExecutorMock();
    passwordApiService.httpExecutor = httpExecutor;

    when(() => httpExecutor.post(any(), body: any(named: "body"))).thenAnswer((_) async => http.Response("", 400));
    expect(() => passwordApiService.createPassword(password), throwsA(isA<ApiException>()));

    when(() => httpExecutor.post(any(), body: any(named: "body"))).thenAnswer((_) async => http.Response("", 500));
    expect(() => passwordApiService.createPassword(password), throwsA(isA<ApiException>()));
  });

  test("PasswordApiService.updatePassword successfull", () async {
    Password password = Password(passwordId: "1", friendlyName: "name", password: "password", url: "url", username: "username");
    HttpExecutorMock httpExecutor = HttpExecutorMock();
    when(() => httpExecutor.put(any(), body: any(named: "body"))).thenAnswer((_) async => http.Response("", 201));
    passwordApiService.httpExecutor = httpExecutor;

    bool result = await passwordApiService.updatePassword(password);

    expect(result, true);
  });

  test("PasswordApiService.updatePassword throws apiException on non-sucessfull statuscode", () async {
    Password password = Password(passwordId: "1", friendlyName: "name", password: "password", url: "url", username: "username");
    HttpExecutorMock httpExecutor = HttpExecutorMock();
    passwordApiService.httpExecutor = httpExecutor;

    when(() => httpExecutor.put(any(), body: any(named: "body"))).thenAnswer((_) async => http.Response("", 400));
    expect(() => passwordApiService.updatePassword(password), throwsA(isA<ApiException>()));

    when(() => httpExecutor.put(any(), body: any(named: "body"))).thenAnswer((_) async => http.Response("", 500));
    expect(() => passwordApiService.updatePassword(password), throwsA(isA<ApiException>()));
  });

  test("PasswordApiService.generatePassword successfull", () async {
    HttpExecutorMock httpExecutor = HttpExecutorMock();
    when(() => httpExecutor.get(any(),)).thenAnswer((_) async => http.Response("{\"password\":\"password10\"}", 201));
    passwordApiService.httpExecutor = httpExecutor;

    String result = await passwordApiService.generatePassword(10);

    expect(result, "password10");
  });

  test("PasswordApiService.generatePassword throws apiException on non-sucessfull statuscode", () async {
    HttpExecutorMock httpExecutor = HttpExecutorMock();
    passwordApiService.httpExecutor = httpExecutor;

    when(() => httpExecutor.get(any(),)).thenAnswer((_) async => http.Response("", 400));
    expect(() => passwordApiService.generatePassword(10), throwsA(isA<ApiException>()));

    when(() => httpExecutor.get(any(),)).thenAnswer((_) async => http.Response("", 500));
    expect(() => passwordApiService.generatePassword(10), throwsA(isA<ApiException>()));
  });

  test("PasswordApiService.deletePassword successfull", () async {
    HttpExecutorMock httpExecutor = HttpExecutorMock();
    when(() => httpExecutor.delete(any())).thenAnswer((_) async => http.Response("", 201));
    passwordApiService.httpExecutor = httpExecutor;

    bool result = await passwordApiService.deletePassword("1");

    expect(result, true);
  });
  test("PasswordApiService.deletePassword throws apiException on non-sucessfull statuscode", () async {
    HttpExecutorMock httpExecutor = HttpExecutorMock();
    passwordApiService.httpExecutor = httpExecutor;

    when(() => httpExecutor.delete(any())).thenAnswer((_) async => http.Response("", 400));
    expect(() => passwordApiService.deletePassword("1"), throwsA(isA<ApiException>()));

    when(() => httpExecutor.delete(any())).thenAnswer((_) async => http.Response("", 500));
    expect(() => passwordApiService.deletePassword("1"), throwsA(isA<ApiException>()));
  });
}
