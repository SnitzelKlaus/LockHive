import 'dart:convert';

import 'package:http/http.dart' as http;
import 'package:logger/logger.dart';
import 'package:password_manager_client/services/http_executor/firebase_token_manager.dart';

class HttpExecutor {
  Logger log = Logger();
  http.Client httpClient = http.Client();
  FirebaseTokenManager firebaseTokenManager = FirebaseTokenManager();

  Future<http.Response> get(
    Uri uri, {
    Map<String, String>? headers,
  }) async {
    log.t("Calling get with uri $uri");
    headers ??= <String, String>{};
    headers["Authorization"] = "Bearer ${await firebaseTokenManager.getUserToken()}";
    return await httpClient.get(uri, headers: headers);
  }

  Future<http.Response> post(Uri uri,
      {Map<String, String>? headers, Map<String, dynamic>? body}) async {
    log.t("Calling post with uri $uri and body $body");

    body ??= <String, dynamic>{};

    headers ??= <String, String>{};

    headers["Content-Type"] = "application/json";
    headers["Authorization"] = "Bearer ${await firebaseTokenManager.getUserToken()}";

    return await httpClient.post(uri, body: json.encode(body), headers: headers);
  }

  Future<http.Response> delete(Uri uri,
      {Map<String, String>? headers, Map<String, dynamic>? body}) async {
    log.t("Calling delete with uri $uri and body $body");

    body ??= <String, dynamic>{};

    headers ??= <String, String>{};

    headers["Content-Type"] = "application/json";
    headers["Authorization"] = "Bearer ${await firebaseTokenManager.getUserToken()}";


    return await httpClient.delete(uri, body: json.encode(body), headers: headers);
  }

  Future<http.Response> put(Uri uri,
      {Map<String, String>? headers, Map<String, dynamic>? body}) async {
    log.t("Calling put with uri $uri and body $body");
    
    body ??= <String, dynamic>{};

    headers ??= <String, String>{};

    headers["Content-Type"] = "application/json";
    headers["Authorization"] = "Bearer ${await firebaseTokenManager.getUserToken()}";

    return await httpClient.put(uri, body: json.encode(body), headers: headers);
  }
}
