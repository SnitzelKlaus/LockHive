import 'package:logger/logger.dart';

import '../../../models/dto_models/password.dart';
import '../../backend_services/api_utilities/api_exception.dart';
import '../../backend_services/password_api_service/password_api_service.dart';

class PasswordServiceManager {
  PasswordApiService passwordApiService = PasswordApiService();
  Logger logger = Logger();

  Future<List<Password>> getPasswords() async {
    logger.d("Caling PasswordServiceManager.getPasswords");
    try{
      return passwordApiService.getPasswords();

    }
    on ApiException catch (e){
      logger.e("Error in PasswordServiceManager.getPasswords", error: e);
      rethrow;
    }
  }
  Future<bool> createPassword(Password password) async {
    logger.d("Caling PasswordServiceManager.craetePassword");

    if(password.passwordId != null){
      logger.e("Id should be null");
      throw Exception("Id should be null");
    }
    if(password.friendlyName == null || password.friendlyName!.isEmpty){
      logger.e("Friendly name should not be empty");
      throw Exception("Friendly name should not be empty");
    }
    if(password.password == null || password.password!.isEmpty){
      logger.e("Password should not be empty");
      throw Exception("Password should not be empty");
    }
    if(password.url == null || password.url!.isEmpty){
      logger.e("Url should not be empty");
      throw Exception("Url should not be empty");
    }
    if(password.username == null || password.username!.isEmpty){
      logger.e("Username should not be empty");
      throw Exception("Username should not be empty");
    }

    try{
      return passwordApiService.createPassword(password);

    }
    on ApiException catch (e){
      logger.e("Error in PasswordServiceManager.createPassword", error: e);
      rethrow;
    }
  }
  
  Future<String> generatePassword(double length) async {
    logger.d("Caling PasswordServiceManager.generatePassword");

    if(length <= 8 || length > 128){
      logger.e("Password length should be between 8 and 128");
      throw Exception("Password length should be between 8 and 128");
    }

    try{
      return passwordApiService.generatePassword(length);

    }
    on ApiException catch (e) {
      logger.e("Error in PasswordServiceManager.generatePassword", error: e);
      rethrow;
    }
  }

  Future<bool> updatePassword(Password password) async {
    logger.d("Caling PasswordServiceManager.updatePassword");

    if(password.passwordId == null){
      logger.e("Id should not be null");
      throw Exception("Id should not be null");
    }
    if(password.friendlyName == null || password.friendlyName!.isEmpty){
      logger.e("Friendly name should not be empty");
      throw Exception("Friendly name should not be empty");
    }
    if(password.password == null || password.password!.isEmpty){
      logger.e("Password should not be empty");
      throw Exception("Password should not be empty");
    }
    if(password.url == null || password.url!.isEmpty){
      logger.e("Url should not be empty");
      throw Exception("Url should not be empty");
    }
    if(password.username == null || password.username!.isEmpty){
      logger.e("Username should not be empty");
      throw Exception("Username should not be empty");
    }

    try{
      return passwordApiService.updatePassword(password);

    }
    on ApiException catch (e){
      logger.e("Error in PasswordServiceManager.updatePassword", error: e);
      rethrow;
    }
  }

  Future<bool> deletePassword(String passwordId) async {
    logger.d("Caling PasswordServiceManager.deletePassword");
    try{
      return passwordApiService.deletePassword(passwordId);

    }
    on ApiException catch (e){
      logger.e("Error in PasswordServiceManager.updatePassword", error: e);
      return false;
    }
  }
}
