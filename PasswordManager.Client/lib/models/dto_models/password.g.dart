// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'password.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Password _$PasswordFromJson(Map<String, dynamic> json) => Password(
      passwordId: json['passwordId'] as String?,
      url: json['url'] as String?,
      password: json['password'] as String?,
      username: json['username'] as String?,
      friendlyName: json['friendlyName'] as String?,
    );

Map<String, dynamic> _$PasswordToJson(Password instance) => <String, dynamic>{
      'passwordId': instance.passwordId,
      'url': instance.url,
      'password': instance.password,
      'username': instance.username,
      'friendlyName': instance.friendlyName,
    };
