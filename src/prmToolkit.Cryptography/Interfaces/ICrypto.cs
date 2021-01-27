namespace prmToolkit.Cryptography.Interfaces
{
    public interface ICrypto
    {
        string Encrypt(string contentToBeEncrypted, string privateEncryptionKey);
        string Decrypt(string contentToBeDecrypted, string privateEncryptionKey);
    }
}