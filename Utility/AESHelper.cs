using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using static System.Security.Cryptography.Aes;

namespace Website.Utility;

public class AesHelper
{
    public static byte[] EncryptStringToBytes_Aes(string plainText, string key, byte[]? iv = null)
    {
        // Check arguments.
        if (plainText is not { Length: > 0 })
            throw new ArgumentNullException(nameof(plainText));
        if (key is not { Length: > 0 })
            throw new ArgumentNullException(nameof(key));

        // Create an Aes object
        // with the specified key and IV.
        using var aesAlg = Create();
        aesAlg.BlockSize = 128;
        aesAlg.KeySize = 256;
        aesAlg.Padding = PaddingMode.Zeros;
        aesAlg.Mode = CipherMode.CBC;
        
        aesAlg.Key = Convert.FromBase64String(key);

        if (iv is not null)
            aesAlg.IV = iv;

        // Create an encryptor to perform the stream transform.
        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        // Create the streams used for encryption.
        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        using var swEncrypt = new StreamWriter(csEncrypt);
        //Write all data to the stream.
        swEncrypt.Write(plainText);
        return msEncrypt.ToArray();
    }

    public static string DecryptStringFromBytes_Aes(string cipherText, string Key, byte[]? IV = null)
    {
        // Check arguments.
        if (cipherText is not { Length: > 0 })
            throw new ArgumentNullException(nameof(cipherText));
        if (Key is not { Length: > 0 })
            throw new ArgumentNullException(nameof(Key));
        // Declare the string used to hold
        // the decrypted text.

        // Create an Aes object
        // with the specified key and IV.
        using var aesAlg = Create();
        
        aesAlg.BlockSize = 128;
        aesAlg.KeySize = 256;
        aesAlg.Padding = PaddingMode.Zeros;
        aesAlg.Mode = CipherMode.CBC;
        
        var cipherByte = Convert.FromBase64String(cipherText);
        aesAlg.Key = Convert.FromBase64String(Key);
        IV ??= cipherByte[..16];

        aesAlg.IV = IV;

        // Create a decryptor to perform the stream transform.
        var decryptor = aesAlg.CreateDecryptor();

        var unencryptedData = decryptor.TransformFinalBlock(cipherByte, 16, cipherByte.Length - 16);

        // Read the decrypted bytes from the decrypting stream
        // and place them in a string.
        var plaintext = Encoding.UTF8.GetString(unencryptedData).Trim('\0');

        return plaintext;
    }
}