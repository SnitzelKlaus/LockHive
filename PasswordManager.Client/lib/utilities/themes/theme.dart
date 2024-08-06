import 'package:flutter/material.dart';

class CustomTheme {
  static ThemeData get lightTheme {
    return ThemeData(
      primarySwatch: Colors.amber,
      appBarTheme: AppBarTheme(backgroundColor: Colors.amber),
      primaryColor: Colors.purple,
      scaffoldBackgroundColor: Color.fromARGB(255, 255, 255, 255),
      fontFamily: 'Raleway',
      cardColor: Color.fromARGB(255, 230, 230, 230),
      elevatedButtonTheme: ElevatedButtonThemeData(
          style: ButtonStyle(
              backgroundColor: MaterialStateProperty.all(Colors.amber),
              foregroundColor: MaterialStateProperty.all(Colors.black))),
      floatingActionButtonTheme: const FloatingActionButtonThemeData(
          backgroundColor: Colors.amber, foregroundColor: Colors.black),
      buttonTheme: ButtonThemeData(
        shape:
            RoundedRectangleBorder(borderRadius: BorderRadius.circular(18.0)),
        buttonColor: Colors.purple,
      ),
      inputDecorationTheme: InputDecorationTheme(
        border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(8),
        ),
      ),
      outlinedButtonTheme: OutlinedButtonThemeData(
        style: ButtonStyle(
          padding: MaterialStateProperty.all<EdgeInsets>(
            const EdgeInsets.all(24),
          ),
          backgroundColor: MaterialStateProperty.all<Color>(Colors.amber),
          foregroundColor: MaterialStateProperty.all<Color>(Colors.black),
        ),
      ),
    );
  }
}
