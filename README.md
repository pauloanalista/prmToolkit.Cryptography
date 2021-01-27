# prmToolkit.Cryptography
Uma biblioteca para fornecer um servi�o de criptografia simples.

Atualmente suporta apenas criptografia AES de 256 bits.

## Instala��o

Voc� pode instalar a partir do NuGet usando o seguinte comando:

`Install-Package prmToolkit.Cryptography`

Ou por meio do gerenciador de pacotes do Visual Studio.

## Configura��o

Se voc� deseja usar inje��o de depend�ncia, � necess�rio registrar o servi�o de criptografia na cole��o de servi�os (geralmente no arquivo _Startup.cs_):

```csharp
using prmToolkit.Cryptography;
using prmToolkit.Cryptography.Interfaces;

public void ConfigureServices (servi�os IServiceCollection)
{
    services.AddSingleton <ICrypto, Crypto> ();
}
```

Se voc� quiser usar manualmente, basta fazer:

```csharp
using prmToolkit.Cryptography;
using prmToolkit.Cryptography.Interfaces;

Public static async Task Main (string [] args)
{
    ICrypto Crypto = new Crypto();
}
```

## Uso
Se voc� usar inje��o de depend�ncia, injete `ICrypto` em sua classe:

```csharp
using prmToolkit.Cryptography;
using prmToolkit.Cryptography.Interfaces;

public class MyClass {

    private readonly ICrypto _crypto;

    public MyClass (ICrypto crypto)
    {
        _crypto = crypto;
    }

    public void MyMethod ()
    {
        // A chave de criptografia privada deve ter 256 bits (32 caracteres)
        string _privateEncryptionKey = "X5hHQvWWkUZKvKLgmbBimuiKruCW5qH7";

        var encryptedContent = _crypto.Encrypt(_contentToBeEncrypted, _privateEncryptionKey);
        var decryptedContent = _crypto.Decrypt(encryptedContent, _privateEncryptionKey);
    }
}
```

Se voc� usar manualmente:

```csharp
using prmToolkit.Cryptography;
using prmToolkit.Cryptography.Interfaces;

public class MyClass {

    public void MyMethod ()
    {
        ICrypto crypto = new Crypto();
        
        // A chave de criptografia privada deve ter 256 bits (32 caracteres)
        string _privateEncryptionKey = "X5hHQvWWkUZKvKLgmbBimuiKruCW5qH7";

        var encryptedContent = _crypto.Encrypt(_contentToBeEncrypted, _privateEncryptionKey);
        var decryptedContent = _crypto.Decrypt(encryptedContent, _privateEncryptionKey);
    }
}
```
