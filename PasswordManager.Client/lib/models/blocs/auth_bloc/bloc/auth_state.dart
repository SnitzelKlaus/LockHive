part of 'auth_bloc.dart';

sealed class AuthState {
  late User? user;
}

final class AuthInitial extends AuthState {}
