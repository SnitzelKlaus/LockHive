import 'package:password_manager_client/models/blocs/auth_bloc/bloc/auth_bloc.dart';
import 'package:test/test.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:firebase_auth_mocks/firebase_auth_mocks.dart';

void main() {
  
  test('Log login bloc event', () async {

    User user = await MockFirebaseAuth().createUserWithEmailAndPassword(
      email: "unittest@gmail.com", password: "123456").then((value) => value.user!);
    LoginEvent loginEvent = LoginEvent(user);
    AuthState state = AuthInitial();
    loginEvent.execute(state);


    AuthState expectedState = AuthInitial();
    expectedState.user = user;
    expect(state.user, expectedState.user,);
  });
}