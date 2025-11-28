using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SpaceXProject.api.Configuration;

namespace SpaceXProject.api.Data.Security;
public class IdentityLookupProtectorKeyRing : ILookupProtectorKeyRing
{
    private readonly IdentityEncryptionKeyConfig _encryptionKeyConfig;

    public IdentityLookupProtectorKeyRing(IOptions<IdentityEncryptionKeyConfig> config)
    {
        _encryptionKeyConfig = config.Value;
    }

    public IEnumerable<string> GetAllKeyIds()
    {
        return _encryptionKeyConfig.Keys.Keys;
    }

    public string CurrentKeyId => _encryptionKeyConfig.CurrentIdentityKeyId;

    public string this[string keyId] => _encryptionKeyConfig.Keys.ContainsKey(keyId) ? _encryptionKeyConfig.Keys[keyId] : throw new KeyNotFoundException($"KeyId {keyId} not found.");
}
