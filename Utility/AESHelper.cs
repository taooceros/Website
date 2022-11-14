using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using static System.Security.Cryptography.Aes;

namespace Website.Utility;

public class AesHelper
{
    public static async Task<string> SubtleCryptoDecryptAsync(IJSRuntime jsRuntime, string key, string cipherText)
    {
        var keyBytes = Convert.FromBase64String(key);
        var cipherBytes = Convert.FromBase64String(cipherText);
        var iv = new Byte[16];
        var decrypted = await jsRuntime.InvokeAsync<byte[]>("decryptAsync", cipherBytes, keyBytes, iv);
        var decryptedText = Convert.FromBase64CharArray(decrypted.Select(b => (char)b).ToArray(), 0, decrypted.Length);
        return Encoding.UTF8.GetString(decryptedText);
    }
}