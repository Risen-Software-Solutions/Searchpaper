namespace Lioncore.WebApi.Services;

public class StorageOptions
{
    public const string Storage = "Storage";

    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string ServiceURL { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public bool ForcePathStyle { get; set; } = true;
}
