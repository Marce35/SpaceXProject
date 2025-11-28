namespace SpaceXProject.api.Configuration;
public class IdentityEncryptionKeyConfig
{
    public Dictionary<string, string> Keys { get; set; } = new();

    public string CurrentIdentityKeyId { get; set; } = string.Empty;
}
