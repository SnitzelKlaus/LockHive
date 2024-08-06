import 'package:firebase_ui_auth/firebase_ui_auth.dart';
import 'package:password_manager_client/screens/create_vautl_value_screen.dart';
import 'package:password_manager_client/screens/home_screen.dart';
import 'package:password_manager_client/screens/user_screen.dart';
import 'package:password_manager_client/screens/vault_value_screen.dart';

var appRoutes = {
  '/home': (context) => const HomeScreen(),
  '/vaultValueScreen': (context) => const VaultValueScreen(),
  '/createVaultValue': (context) => const CreateVaultValueScreen(),
  '/user': (context) => const UserScreen(),

  //Firebase UI Auth
  '/sign-in': (context) =>  SignInScreen(
    providers: [
      EmailAuthProvider(),
      ],
  )
};
