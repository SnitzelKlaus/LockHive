import 'dart:async';

import 'package:bloc/bloc.dart';
import 'package:logger/logger.dart';
import 'package:password_manager_client/models/dto_models/password.dart';
import 'package:password_manager_client/models/interfaces/i_vault_value.dart';
import 'package:password_manager_client/services/backend_services/api_utilities/api_exception.dart';
import 'package:password_manager_client/services/service_managers/password_service/password_service_manager.dart';

import '../../../enums/vault_value_type.dart';


part 'edit_vault_value_event.dart';
part 'edit_vault_value_state.dart';

class EditVaultValueBloc
    extends Bloc<EditVaultValueEvent, EditVaultValueState> {
  final _editVaultValueStateController =
      StreamController<EditVaultValueState>.broadcast();

  StreamSink<EditVaultValueState> get _currentVaultSink =>
      _editVaultValueStateController.sink;

  Stream<EditVaultValueState> get editVaultValueState =>
      _editVaultValueStateController.stream;

  final _eventStreamController = StreamController<EditVaultValueEvent>();

  StreamSink<EditVaultValueEvent> get eventSink =>
      _eventStreamController.sink;

  Stream<EditVaultValueEvent?> get eventStream =>
      _eventStreamController.stream;

  EditVaultValueBloc() : super(EditVaultValueInitial()) {
    _eventStreamController.stream.listen(_mapEventToState);
    _currentVaultSink.add(state);
  }
  Future<void> _mapEventToState(EditVaultValueEvent event) async {
    await event.execute(state);
    _currentVaultSink.add(state);
  }
}
