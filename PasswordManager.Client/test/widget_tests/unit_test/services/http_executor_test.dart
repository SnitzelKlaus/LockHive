import 'package:flutter_test/flutter_test.dart';
import 'package:http/http.dart' as http;
import 'package:http/http.dart';
import 'package:mocktail/mocktail.dart';
import 'package:password_manager_client/services/http_executor/firebase_token_manager.dart';
import 'package:password_manager_client/services/http_executor/http_executor.dart';


class HttpMockClient extends Mock implements Client{}
class MockFirebaseTokenManager extends Mock implements FirebaseTokenManager{}
class FakeUri extends Fake implements Uri {}

void main(){
  setUpAll(() {
    registerFallbackValue(FakeUri());
  });
  
  test("HttpExecetor.get successfull without specific headers", () async {
    http.Response response = http.Response("[]", 200);
    MockFirebaseTokenManager firebaseTokenManager = MockFirebaseTokenManager();
    when(() => firebaseTokenManager.getUserToken()).thenAnswer((_) async => "token");
    var headers = <String, String>{};
    headers["Authorization"] = "Bearer token";
    
    HttpMockClient httpMockClient = HttpMockClient();
    when(() => httpMockClient.get(Uri.parse("http://localhost:8080/passwords"), headers: headers)).thenAnswer((_) async => Response("[]", 200));
    
    HttpExecutor httpExecutor = HttpExecutor();
    httpExecutor.httpClient = httpMockClient;
    httpExecutor.firebaseTokenManager = firebaseTokenManager;
    
    http.Response result = await httpExecutor.get(Uri.parse("http://localhost:8080/passwords"));
   
    expect(result.body, response.body);
    expect(result.statusCode, response.statusCode);
  });
  test("HttpExecetor.get successfull with specific headers", () async {
    http.Response response = http.Response("[]", 200);
    MockFirebaseTokenManager firebaseTokenManager = MockFirebaseTokenManager();
    when(() => firebaseTokenManager.getUserToken()).thenAnswer((_) async => "token");
    
    var specificHeaders = <String, String>{};
    specificHeaders["specific"] = "header";
    var headers = specificHeaders;
    headers["Authorization"] = "Bearer token";
    HttpMockClient httpMockClient = HttpMockClient();
    when(() => httpMockClient.get(Uri.parse("http://localhost:8080/passwords"), headers: headers)).thenAnswer((_) async => response);
    
    HttpExecutor httpExecutor = HttpExecutor();
    httpExecutor.httpClient = httpMockClient;
    httpExecutor.firebaseTokenManager = firebaseTokenManager;
    
    http.Response result = await httpExecutor.get(Uri.parse("http://localhost:8080/passwords"), headers: specificHeaders);
   
    expect(result.body, response.body);
    expect(result.statusCode, response.statusCode);
  });
  test("HttpExecetor.post successfull without specific headers", () async {
    http.Response response = http.Response("[]", 200);
    MockFirebaseTokenManager firebaseTokenManager = MockFirebaseTokenManager();
    when(() => firebaseTokenManager.getUserToken()).thenAnswer((_) async => "token");
    var headers = <String, String>{};
    headers["Authorization"] = "Bearer token";
    headers["Content-Type"] = "application/json";
    
    HttpMockClient httpMockClient = HttpMockClient();
    when(() => httpMockClient.post(Uri.parse("http://localhost:8080/passwords"), body: any(named: "body"), headers: headers, )).thenAnswer((_) async => response);
    
    HttpExecutor httpExecutor = HttpExecutor();
    httpExecutor.httpClient = httpMockClient;
    httpExecutor.firebaseTokenManager = firebaseTokenManager;
    
    http.Response result = await httpExecutor.post(Uri.parse("http://localhost:8080/passwords"));
   
    expect(result.body, response.body);
    expect(result.statusCode, response.statusCode);
  });
  test("HttpExecetor.post successfull with specific headers", () async {
    http.Response response = http.Response("[]", 200);
    MockFirebaseTokenManager firebaseTokenManager = MockFirebaseTokenManager();
    when(() => firebaseTokenManager.getUserToken()).thenAnswer((_) async => "token");
    
    var specificHeaders = <String, String>{};
    specificHeaders["specific"] = "header";
    var headers = specificHeaders;
    headers["Authorization"] = "Bearer token";
    HttpMockClient httpMockClient = HttpMockClient();
    when(() => httpMockClient.post(Uri.parse("http://localhost:8080/passwords"), body: any(named: "body"), headers: headers)).thenAnswer((_) async => response);
    
    HttpExecutor httpExecutor = HttpExecutor();
    httpExecutor.httpClient = httpMockClient;
    httpExecutor.firebaseTokenManager = firebaseTokenManager;
    
    http.Response result = await httpExecutor.post(Uri.parse("http://localhost:8080/passwords"), headers: specificHeaders);
   
    expect(result.body, response.body);
    expect(result.statusCode, response.statusCode);
  });
  
  test("HttpExecetor.put successfull with specific headers", () async {
    http.Response response = http.Response("[]", 200);
    MockFirebaseTokenManager firebaseTokenManager = MockFirebaseTokenManager();
    when(() => firebaseTokenManager.getUserToken()).thenAnswer((_) async => "token");
    
    var specificHeaders = <String, String>{};
    specificHeaders["specific"] = "header";
    var headers = specificHeaders;
    headers["Authorization"] = "Bearer token";
    HttpMockClient httpMockClient = HttpMockClient();
    when(() => httpMockClient.put(Uri.parse("http://localhost:8080/passwords"), body: any(named: "body"), headers: headers)).thenAnswer((_) async => response);
    
    HttpExecutor httpExecutor = HttpExecutor();
    httpExecutor.httpClient = httpMockClient;
    httpExecutor.firebaseTokenManager = firebaseTokenManager;
    
    http.Response result = await httpExecutor.put(Uri.parse("http://localhost:8080/passwords"), headers: specificHeaders);
   
    expect(result.body, response.body);
    expect(result.statusCode, response.statusCode);
  });
  test("HttpExecetor.put successfull without specific headers", () async {
    http.Response response = http.Response("[]", 200);
    MockFirebaseTokenManager firebaseTokenManager = MockFirebaseTokenManager();
    when(() => firebaseTokenManager.getUserToken()).thenAnswer((_) async => "token");
    var headers = <String, String>{};
    headers["Authorization"] = "Bearer token";
    headers["Content-Type"] = "application/json";
    
    HttpMockClient httpMockClient = HttpMockClient();
    when(() => httpMockClient.put(Uri.parse("http://localhost:8080/passwords"), body: any(named: "body"), headers: headers, )).thenAnswer((_) async => response);
    
    HttpExecutor httpExecutor = HttpExecutor();
    httpExecutor.httpClient = httpMockClient;
    httpExecutor.firebaseTokenManager = firebaseTokenManager;
    
    http.Response result = await httpExecutor.put(Uri.parse("http://localhost:8080/passwords"));
   
    expect(result.body, response.body);
    expect(result.statusCode, response.statusCode);
  });
  test("HttpExecetor.delete successfull with specific headers", () async {
    http.Response response = http.Response("[]", 200);
    MockFirebaseTokenManager firebaseTokenManager = MockFirebaseTokenManager();
    when(() => firebaseTokenManager.getUserToken()).thenAnswer((_) async => "token");
    
    var specificHeaders = <String, String>{};
    specificHeaders["specific"] = "header";
    var headers = specificHeaders;
    headers["Authorization"] = "Bearer token";
    HttpMockClient httpMockClient = HttpMockClient();
    when(() => httpMockClient.delete(Uri.parse("http://localhost:8080/passwords"), body: any(named: "body"), headers: headers)).thenAnswer((_) async => response);
    
    HttpExecutor httpExecutor = HttpExecutor();
    httpExecutor.httpClient = httpMockClient;
    httpExecutor.firebaseTokenManager = firebaseTokenManager;
    
    http.Response result = await httpExecutor.delete(Uri.parse("http://localhost:8080/passwords"), headers: specificHeaders);
   
    expect(result.body, response.body);
    expect(result.statusCode, response.statusCode);
  });
  test("HttpExecetor.delete successfull without specific headers", () async {
    http.Response response = http.Response("[]", 200);
    MockFirebaseTokenManager firebaseTokenManager = MockFirebaseTokenManager();
    when(() => firebaseTokenManager.getUserToken()).thenAnswer((_) async => "token");
    var headers = <String, String>{};
    headers["Authorization"] = "Bearer token";
    headers["Content-Type"] = "application/json";
    
    HttpMockClient httpMockClient = HttpMockClient();
    when(() => httpMockClient.delete(Uri.parse("http://localhost:8080/passwords"), body: any(named: "body"), headers: headers)).thenAnswer((_) async => response);
    
    HttpExecutor httpExecutor = HttpExecutor();
    httpExecutor.httpClient = httpMockClient;
    httpExecutor.firebaseTokenManager = firebaseTokenManager;
    
    http.Response result = await httpExecutor.delete(Uri.parse("http://localhost:8080/passwords", ));
   
    expect(result.body, response.body);
    expect(result.statusCode, response.statusCode);
  });
}