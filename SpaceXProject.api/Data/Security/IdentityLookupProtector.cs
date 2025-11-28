using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Aes = System.Security.Cryptography.Aes;

namespace SpaceXProject.api.Data.Security;
public class IdentityLookupProtector : ILookupProtector
{
    private readonly ILookupProtectorKeyRing _keyRing;

    public IdentityLookupProtector(ILookupProtectorKeyRing keyRing)
    {
        _keyRing = keyRing;
    }

    public string Protect(string keyId, string? data)
    {
        if (string.IsNullOrEmpty(data)) return data ?? string.Empty;

        var keyString = _keyRing[keyId];
        var key = Convert.FromBase64String(keyString);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = DeriveIvFromData(data, aes.BlockSize / 8);

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();

        ms.Write(aes.IV, 0, aes.IV.Length);

        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(data);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public string? Unprotect(string keyId, string? data)
    {
        if (string.IsNullOrEmpty(data)) return data ?? string.Empty;

        var keyString = _keyRing[keyId];
        var key = Convert.FromBase64String(keyString);
        var fullCipher = Convert.FromBase64String(data);

        using var aes = Aes.Create();
        aes.Key = key;

        var iv = new byte[16];

        if (fullCipher.Length < 16) throw new Exception("Invalid encrypted data");

        Array.Copy(fullCipher, 0, iv, 0, iv.Length);
        aes.IV = iv;


        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(fullCipher, 16, fullCipher.Length - 16);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }

    private byte[] DeriveIvFromData(string data, int ivSize)
    {
        using var sha = SHA256.Create();
        var dataBytes = Encoding.UTF8.GetBytes(data);
        var hash = sha.ComputeHash(dataBytes);

        var iv = new byte[ivSize];
        Array.Copy(hash, 0, iv, 0, ivSize);
        return iv;
    }
}
