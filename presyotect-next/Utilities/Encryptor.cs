using NETCore.Encrypt;

namespace Presyotect.Utilities;

public static class Encryptor
{
    public static string Hash(this string source)
    {
        return EncryptProvider.Sha512(source);
    }
    
    public static string Encrypt(this string source, IConfiguration configuration)
    {
        return EncryptProvider.AESEncrypt(source, configuration["Encryption:Key"]);
    }

    public static string Decrypt(this string source, IConfiguration configuration)
    {
        return EncryptProvider.AESDecrypt(source, configuration["Encryption:Key"]);
    }
}