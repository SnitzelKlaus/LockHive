import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:password_manager_client/models/blocs/create_vault_value_bloc/bloc/edit_vault_value_bloc.dart';

import '../../../models/dto_models/password.dart';

class EditPasswordVaultValueInputContainer extends StatefulWidget {
  const EditPasswordVaultValueInputContainer(
      {super.key, required this.newPassword, required this.generatePasswordLength});
  final Password newPassword;
  final double generatePasswordLength;

  @override
  State<EditPasswordVaultValueInputContainer> createState() => _EditPasswordVaultValueInputContainerState();
}

class _EditPasswordVaultValueInputContainerState extends State<EditPasswordVaultValueInputContainer> {
 final _formKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    return Form(
      key: _formKey,
      child: Column(
        children: [
          TextFormField(
            controller: TextEditingController(text: widget.newPassword.friendlyName),
            decoration: const InputDecoration(
              border: UnderlineInputBorder(),
              labelText: 'Name',
            ),
            onChanged: (value) {
              widget.newPassword.friendlyName = value;
            },
            validator: (value) {
              if (value == null || value.isEmpty) {
                return 'Please enter a name';
              }
              return null;
            },
          ),
          TextFormField(
            controller: TextEditingController(text: widget.newPassword.url),
            decoration: const InputDecoration(
              border: UnderlineInputBorder(),
              labelText: 'Url',
            ),
            onChanged: (value) {
              widget.newPassword.url = value;
            },
            validator: (value) {
              if (value == null || value.isEmpty) {
                return 'Please enter a url';
              }
              return null;
            },
          ),
          TextFormField(
            controller: TextEditingController(text: widget.newPassword.username),
            decoration: const InputDecoration(
              border: UnderlineInputBorder(),
              labelText: 'Username',
            ),
            onChanged: (value) {
              widget.newPassword.username = value;
            },
            validator: (value) {
              if (value == null || value.isEmpty) {
                return 'Please enter a username';
              }
              return null;
            },
          ),
          Row(
            children: [
              Expanded(
                child: TextFormField(
                  controller: TextEditingController(text: widget.newPassword.password),
                  decoration: InputDecoration(
                    border: UnderlineInputBorder(),
                    labelText: 'Password',
                    suffixIcon:  IconButton(
                  icon: Icon(
                     widget.newPassword.hidePasswordInput
                     ? Icons.visibility
                     : Icons.visibility_off,
                     color: Theme.of(context).primaryColorDark,
                     ),
                  onPressed: () {
                    widget.newPassword.hidePasswordInput = !widget.newPassword.hidePasswordInput;
                    BlocProvider.of<EditVaultValueBloc>(context)
                      .eventSink
                      .add(SetVaultValue(widget.newPassword));
                   },
                  ),
                  ),
                  onChanged: (value) {
                    widget.newPassword.password = value;
                  },
                  validator: (value) {
                    if (value == null || value.isEmpty || value.length < 8) {
                      return 'Please enter a password that is longer than 8 characters';
                    }
                    return null;
                  },
                  obscureText: widget.newPassword.hidePasswordInput,
                  enableSuggestions: false,
                  autocorrect: false,
                ),
              ),
              Container(
                child:IconButton(
                    icon: Icon(
                       Icons.lock_reset,
                       color: Theme.of(context).primaryColorDark,
                       ),
                    onPressed: () {
                      
                      BlocProvider.of<EditVaultValueBloc>(context)
                        .eventSink
                        .add(GeneratePassword(widget.newPassword,  widget.generatePasswordLength));
                     },
                    ),
              ),
              
            ],
          ),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
              Padding(
                padding: const EdgeInsets.all(8.0),
                child: Text(widget.generatePasswordLength.round().toString(),),
              ),
              
              SizedBox(
                width: MediaQuery.of(context).size.width * 0.8,

                child: Slider(
                      value: widget.generatePasswordLength,
                      divisions: 120,
                      label: widget.generatePasswordLength.round().toString(),
                      onChanged: (double value) {
                          BlocProvider.of<EditVaultValueBloc>(context)
                        .eventSink
                        .add(SetGeneratePasswordLength(value));
                      },
                      min: 9,
                      max: 128,
                    ),
              ),
            ],
            ),
          ElevatedButton(
            onPressed: () {
              if (_formKey.currentState!.validate()) {
              BlocProvider.of<EditVaultValueBloc>(context)
                  .eventSink
                  .add(SaveVaultValue(widget.newPassword));
                Navigator.pop(context);
                  }
                

            },
            child: Text("Save"),
          )
        ],
      ),
    );
  }
}
