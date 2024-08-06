using NUnit.Framework;
using PasswordManager.Password.ApplicationServices.PasswordGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Password.Tests.GeneratePassword
{
    [TestFixture]
    public class GeneratePasswordServiceTests
    {
        private GenerateSecureKeyService _generatePasswordService;

        [SetUp]
        public void Setup()
        {
            _generatePasswordService = new GenerateSecureKeyService();
        }

        [Test]
        public void GeneratePassword_ThrowsArgumentException_WhenLengthIsLessThanMinimum()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _generatePasswordService.GenerateKey(7));
        }

        [Test]
        public async Task GeneratePassword_ReturnsPasswordOfRequestedLength()
        {
            var passwordLength = 10;
            var password = await _generatePasswordService.GenerateKey(passwordLength);
            Assert.That(password, Has.Length.EqualTo(passwordLength));
        }

        [Test]
        public async Task GeneratePassword_IncludesAllCharacterTypes()
        {
            var password = await _generatePasswordService.GenerateKey(12);
            Assert.Multiple(() =>
            {
                Assert.That(password.Any(char.IsUpper), Is.True, "Password does not contain an uppercase letter.");
                Assert.That(password.Any(char.IsLower), Is.True, "Password does not contain a lowercase letter.");
                Assert.That(password.Any(char.IsDigit), Is.True, "Password does not contain a digit.");
                Assert.That(password.Any(ch => "!@#$%^&*()_+-=[]{}|;:'\",.<>/?`~".Contains(ch)), Is.True, "Password does not contain a special character.");
            });
        }

        [Test]
        public async Task GeneratePassword_GeneratesRandomPasswords()
        {
            var password1 = await _generatePasswordService.GenerateKey(12);
            var password2 = await _generatePasswordService.GenerateKey(12);
            Assert.That(password2, Is.Not.EqualTo(password1), "Generated passwords are not random.");
        }
    }
}
