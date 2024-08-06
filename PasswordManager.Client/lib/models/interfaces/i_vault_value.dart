import 'package:password_manager_client/models/enums/vault_value_type.dart';

class IVaultValue {
  //Title is used to display the name of the value in the UI
  String? title;
  //SubTitle is used to display the description of the value in the UI
  String? subTitle;
  late VaultValueType type;
}
