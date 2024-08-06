// ignore_for_file: prefer_interpolation_to_compose_strings

class ApiEndpoints{


  static const String _baseUserApiUrl = "https://10.0.2.2:50477/api/";
  static const String _basePasswordApiUrl = "https://10.0.2.2:50472/api/";

  Uri passwordsUri(){
    return Uri.parse(_baseUserApiUrl + "user/passwords");
  }
  Uri passwordsUriWithId(String passwordId){
    return Uri.parse(_baseUserApiUrl + "user/passwords/" + passwordId);
  }
  Uri passwordUriWithId(String passwordId){
    return Uri.parse(_baseUserApiUrl + "user/password/" + passwordId);
  }

  Uri generatePassword(Map<String, dynamic> queryparameters ){
    String queryString = Uri(queryParameters: queryparameters).query;
    return Uri.parse(_basePasswordApiUrl + "password/generate?" + queryString);

  }
}