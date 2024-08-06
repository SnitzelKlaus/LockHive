import 'package:json_annotation/json_annotation.dart';
import 'package:password_manager_client/models/enums/vault_value_type.dart';
import 'package:password_manager_client/models/interfaces/i_vault_value.dart';
// dart run build_runner build

part 'password.g.dart';

@JsonSerializable(explicitToJson: true)
class Password implements IVaultValue{
  String? passwordId;
  @override
  @JsonKey(includeToJson: false, includeFromJson: false)
  String? title, subTitle;
  @override
  @JsonKey(includeToJson: false, includeFromJson: false)
  VaultValueType type = VaultValueType.password;
  @JsonKey(includeToJson: false, includeFromJson: false)
  bool hidePasswordInput = true;

  late String? url, password, username, friendlyName;

  Password({this.passwordId, this.url, this.password, this.username, this.friendlyName}){
    title = friendlyName;
    subTitle = url;
  }

  factory Password.fromJson(Map<String, dynamic> json) =>
      _$PasswordFromJson(json);

  Map<String, dynamic> toJson() => _$PasswordToJson(this);
}
