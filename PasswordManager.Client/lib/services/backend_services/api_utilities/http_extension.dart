import 'package:http/http.dart' as http;

extension IsOk on http.Response {
  /// Returns true if the status code is in the 200 range
  bool get isOk {
    return (statusCode ~/ 100) == 2;
  }
}