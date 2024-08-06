import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:password_manager_client/models/blocs/auth_bloc/bloc/auth_bloc.dart';
import 'package:password_manager_client/widgets/shared/cards/themed_card.dart';
import 'package:password_manager_client/widgets/shared/progress_indicators/circular_generic_progress_indicator.dart';

class OpenUserButton extends StatelessWidget {
  const OpenUserButton({super.key});

  @override
  Widget build(BuildContext context) {
    return ThemedCard(
      child: StreamBuilder<AuthState>(
        initialData: BlocProvider.of<AuthBloc>(context).state,
        stream: BlocProvider.of<AuthBloc>(context).authState,
        builder: (context, AsyncSnapshot<AuthState?> snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting && snapshot.data == null) {
            return const Align(
                alignment: Alignment.topCenter, child: CircularGenericProgessIndicator());
          } else {
            return GestureDetector(
              onTap: () {
                Navigator.pushNamed(context, "/user");
              },
              child: Row(
                mainAxisAlignment: MainAxisAlignment.start,
                children: [
                  CircleAvatar(
                    backgroundImage: NetworkImage(snapshot
                            .data?.user?.photoURL ??
                        "https://robohash.org/${snapshot.data?.user?.email}.png?set=set5"),
                  ),
                  const Padding(padding: EdgeInsets.only(left: 8.0)),
                  Text(
                    (snapshot.data!.user!.email!),
                    style: const TextStyle(
                      fontWeight: FontWeight.bold,
                      fontSize: 18,
                    ),
                  ),
                  const Padding(padding: EdgeInsets.only(left: 8.0)),
                  const Icon(
                    Icons.arrow_forward_outlined,
                    color: Colors.black,
                  ),
                ],
              ),
            );
          }
        },
      ),
    );
  }
}