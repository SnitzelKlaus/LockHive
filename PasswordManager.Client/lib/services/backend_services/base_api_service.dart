import 'package:logger/logger.dart';
import 'package:password_manager_client/services/backend_services/api_endpoints/api_endpoints.dart';

import '../http_executor/http_executor.dart';

abstract class BaseApiService{
  Logger logger = Logger();
  ApiEndpoints apiEndpoints = ApiEndpoints();
  HttpExecutor httpExecutor = HttpExecutor();
}