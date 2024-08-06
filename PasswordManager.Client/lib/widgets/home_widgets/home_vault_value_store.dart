import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:password_manager_client/models/blocs/vault_bloc/bloc/vault_bloc.dart';

import '../shared/progress_indicators/circular_generic_progress_indicator.dart';
import 'home_vault_value_card.dart';

class HomeVaultValueStores extends StatefulWidget {
  const HomeVaultValueStores({super.key});

  @override
  State<HomeVaultValueStores> createState() => _HomeVaultValueStoresState();
}

class _HomeVaultValueStoresState extends State<HomeVaultValueStores> {
  @override
  Widget build(BuildContext context) {
    return StreamBuilder<VaultState>(
      stream: BlocProvider.of<VaultBloc>(context).vaultState,
      builder: (context, snapshot) {
        if (snapshot.connectionState == ConnectionState.waiting) {
          return const CircularGenericProgessIndicator();
        } else if (!snapshot.hasData) {
          return const Placeholder();
        } else {
          var list = List<HomeVaultValueCard>.generate(
            snapshot.data!.vaultValue.length,
            (i) => HomeVaultValueCard(vaultValue: snapshot.data!.vaultValue[i]),
          );

          return Flexible(
            child: GridView.count(
                crossAxisCount: 2,
                mainAxisSpacing: 16,
                crossAxisSpacing: 16,
                children: list),
          );
        }
      },
    );
  }

  @override
  void initState() {
    super.initState();
    BlocProvider.of<VaultBloc>(context).eventSink.add(GetVaultValues());
  }
}
