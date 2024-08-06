import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:password_manager_client/models/blocs/auth_bloc/bloc/auth_bloc.dart';
import 'package:password_manager_client/widgets/shared/progress_indicators/circular_generic_progress_indicator.dart';
import 'package:settings_ui/settings_ui.dart';

class UserScreenContainer extends StatefulWidget {
  const UserScreenContainer({super.key});

  @override
  State<UserScreenContainer> createState() => _UserScreenContainerState();
}

class _UserScreenContainerState extends State<UserScreenContainer> {
  @override
  Widget build(BuildContext context) {
    return StreamBuilder<AuthState>(
        stream: BlocProvider.of<AuthBloc>(context).authState,
        builder: (context, AsyncSnapshot<AuthState?> snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Align(
                alignment: Alignment.topCenter, child: CircularGenericProgessIndicator());
          } else {
            return SettingsList(
              sections: [
                SettingsSection(
                  title: Text("My user"),
                  tiles: <SettingsTile>[    
                    SettingsTile(title: const Text("E-mail"), description: Text(snapshot.data!.user!.email ?? "")),
                    SettingsTile(title: const Text("User id"), description: Text(snapshot.data!.user!.uid)),
                    SettingsTile(
                      title: const Text("Logout"),
                      leading: const Icon(Icons.logout),
                      onPressed: (BuildContext context) async {
                        await FirebaseAuth.instance.signOut();
                      },
                    ),
                  ],
                ),
              ],
            );
          }
        },
      );
  }

    @override
    void initState(){
      super.initState();
      BlocProvider.of<AuthBloc>(context).eventSink.add(NotifyStream());
    }
}