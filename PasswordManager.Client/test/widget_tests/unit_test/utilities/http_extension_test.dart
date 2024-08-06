import 'package:flutter_test/flutter_test.dart';
import 'package:http/http.dart' as http;
import 'package:password_manager_client/services/backend_services/api_utilities/http_extension.dart';

void main(){
  test("HttpExtension generates expected result", () {
    http.Response response = http.Response("body", 200);
    expect(response.isOk, true);
    response = http.Response("body", 201);
    expect(response.isOk, true);
    response = http.Response("body", 400);
    expect(response.isOk, false);
    response = http.Response("body", 401);
    expect(response.isOk, false);
    response = http.Response("body", 404);
    expect(response.isOk, false);
    response = http.Response("body", 500);
    expect(response.isOk, false);
  });
}