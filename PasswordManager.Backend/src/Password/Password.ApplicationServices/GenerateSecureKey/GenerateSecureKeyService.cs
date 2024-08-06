using System.Security.Cryptography;

namespace PasswordManager.Password.ApplicationServices.PasswordGenerator
{
    public class GenerateSecureKeyService : IGenerateSecureKeyService
    {
        // The character sets used to generate the password
        private const string _upperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string _lowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private const string _digits = "0123456789";
        private const string _specialCharacters = "!@#$%^&*()_+-=[]{}|;:'\",.<>/?`~";

        private const string _allCharacters = _upperCaseLetters + _lowerCaseLetters + _digits + _specialCharacters;

        public Task<string> GenerateKey(int length)
        {
            // Ensures that the password length is at least 8 characters
            // Which is the minimum length required by the NIST
            if (length < 8)
            {
                throw new ArgumentException("The password length must be at least 8 characters.");
            }

            // Ensures that the password length is at most 128 characters
            // Which is to prevent the password from being too long causing
            // performance issues.
            if (length > 128)
            {
                throw new ArgumentException("The password length must be at most 128 characters.");
            }

            char[] password = new char[length];
            byte[] randomBytes = GenerateRandomBytes(length);

            // Fills the password with random characters from the allCharacters string
            for (int i = 0; i < length; i++)
            {
                password[i] = _allCharacters[randomBytes[i] % _allCharacters.Length];
            }

            randomBytes = GenerateRandomBytes(4);

            // Ensures that the password contains at least one character from each character set
            // placed at a random position.
            for (int i = 0; i < randomBytes.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        password[randomBytes[i] % password.Length] = _upperCaseLetters[randomBytes[i] % _upperCaseLetters.Length];
                        break;
                    case 1:
                        password[randomBytes[i] % password.Length] = _lowerCaseLetters[randomBytes[i] % _lowerCaseLetters.Length];
                        break;
                    case 2:
                        password[randomBytes[i] % password.Length] = _digits[randomBytes[i] % _digits.Length];
                        break;
                    case 3:
                        password[randomBytes[i] % password.Length] = _specialCharacters[randomBytes[i] % _specialCharacters.Length];
                        break;
                }
            }

            return Task.FromResult(string.Concat(password));
        }

        private byte[] GenerateRandomBytes(int length)
        {
            byte[] randomBytes = new byte[length];
            RandomNumberGenerator.Create().GetBytes(randomBytes);
            return randomBytes;
        }
    }
}
