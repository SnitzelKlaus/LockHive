import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:password_manager_client/models/blocs/create_vault_value_bloc/bloc/edit_vault_value_bloc.dart';
import 'package:password_manager_client/models/blocs/vault_bloc/bloc/vault_bloc.dart';
import 'package:password_manager_client/models/dto_models/password.dart';
import 'package:password_manager_client/widgets/home_widgets/home_container.dart';
import 'package:password_manager_client/widgets/shared/animations/animated_hive_background.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        floatingActionButton: FloatingActionButton(
          onPressed: () async {
            BlocProvider.of<EditVaultValueBloc>(context).eventSink.add(SetVaultValue(Password()));
            await Navigator.pushNamed(context, '/createVaultValue');
            await Future.delayed(Duration(seconds: 2));
            BlocProvider.of<VaultBloc>(context).eventSink.add(GetVaultValues());
          },
          child: Icon(Icons.add),
        ),
        body: const AnimatedHiveBackground(
          child: HomeContainer(),
        ));
  }
}
