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
        var decrypted = await jsRuntime.InvokeAsync<byte[]>("decryptAsync", cipherBytes, keyBytes);
        return Encoding.UTF8.GetString(decrypted);
    }
}