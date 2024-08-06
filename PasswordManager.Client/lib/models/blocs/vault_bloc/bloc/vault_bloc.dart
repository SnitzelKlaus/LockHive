import 'package:bloc/bloc.dart';
import 'package:logger/src/logger.dart';
import 'package:password_manager_client/models/dto_models/password.dart';
import 'package:password_manager_client/models/enums/vault_value_type.dart';
import 'package:password_manager_client/models/interfaces/i_vault_value.dart';
import 'dart:async';

import '../../../../services/service_managers/password_service/password_service_manager.dart';

part 'vault_event.dart';
part 'vault_state.dart';

class VaultBloc extends Bloc<VaultEvent, VaultState> {
  final _vaultStateController = StreamController<VaultState>.broadcast();

  StreamSink<VaultState> get _currentVaultSink => _vaultStateController.sink;

  Stream<VaultState> get vaultState => _vaultStateController.stream;

  final _eventStreamController = StreamController<VaultEvent>();

  StreamSink<VaultEvent> get eventSink => _eventStreamController.sink;

  Stream<VaultEvent?> get eventStream => _eventStreamController.stream;

  VaultBloc() : super(VaultInitial()) {
    _eventStreamController.stream.listen(_mapEventToState);
    _currentVaultSink.add(state);
  }
  Future<void> _mapEventToState(VaultEvent event) async {
    await event.execute(state);
    _currentVaultSink.add(state);
  }
}
