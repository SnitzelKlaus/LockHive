part of 'auth_bloc.dart';


abstract class AuthEvent {
  var log = Logger();
  execute(AuthState state);
}

class LoginEvent extends AuthEvent {
  LoginEvent(this._user);
  final User _user;
  @override
  execute(AuthState state) async {
    log.t("Calling LoginEvent");
    state.user = _user;
  }
}
class NotifyStream extends AuthEvent {
  @override
  execute(AuthState state) async {
    log.t("Calling NotifyStream");
  }
}