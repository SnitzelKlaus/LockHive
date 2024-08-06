import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:password_manager_client/models/blocs/auth_bloc/bloc/auth_bloc.dart';

class AuthScreen extends StatelessWidget {
  const AuthScreen({super.key});

  @override
  Widget build(BuildContext context) {
    FirebaseAuth.instance.authStateChanges().listen((User? user) {
      if (user == null) {
        print('User is currently signed out!');
        Navigator.of(context).pushNamedAndRemoveUntil('/sign-in', (route) => route.isFirst);
      } else {
        print('User is signed in!');

        BlocProvider.of<AuthBloc>(context)
            .eventSink
            .add(LoginEvent(user));
        Navigator.of(context).pushNamedAndRemoveUntil('/home', (route) => route.isFirst);
      }
    });
    return Container();
  }
}
