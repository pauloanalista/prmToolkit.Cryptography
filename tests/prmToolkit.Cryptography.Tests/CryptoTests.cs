using prmToolkit.Cryptography.Interfaces;
using System;
using Xunit;

namespace prmToolkit.Cryptography.Tests
{
    public class CryptoTests
    {
        private readonly ICrypto _crypto;
        private readonly string _privateEncryptionKey;
        private readonly string _contentToBeEncrypted;

        public CryptoTests()
        {
            _crypto = new Crypto();
            _privateEncryptionKey = "C5hHQvWWkUZNvKLgmbBimuiKruCW5qHp";
            _contentToBeEncrypted = "Paulo";
        }
        
        [Fact]
        public void Encrypt_And_Decrypt_Success()
        {
            var encryptedContent = _crypto.Encrypt(_contentToBeEncrypted, _privateEncryptionKey);
            var decryptedContent = _crypto.Decrypt(encryptedContent, _privateEncryptionKey);
            
            Assert.Equal(_contentToBeEncrypted, decryptedContent);
        }

        [Fact]
        public void Encrypt_Without_Content_Must_Fail()
        {
            var exception = Record.Exception(()=> _crypto.Encrypt(null, _privateEncryptionKey));
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Encrypt_Without_Key_Must_Fail()
        {
            var exception = Record.Exception(() => _crypto.Encrypt(_contentToBeEncrypted, null));
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Encrypt_With_Invalid_Key_Must_Fail()
        {
            var exception = Record.Exception(() => _crypto.Encrypt(_contentToBeEncrypted, "123"));
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }

        [Fact]
        public void Decrypt_Without_Content_Must_Fail()
        {
            var exception = Record.Exception(() => _crypto.Decrypt(null, _privateEncryptionKey));
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Decrypt_Without_Key_Must_Fail()
        {
            var exception = Record.Exception(() => _crypto.Decrypt(_contentToBeEncrypted, null));
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Decrypt_With_Invalid_Key_Must_Fail()
        {
            var exception = Record.Exception(() => _crypto.Decrypt(_contentToBeEncrypted, "123"));
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }


    }
}