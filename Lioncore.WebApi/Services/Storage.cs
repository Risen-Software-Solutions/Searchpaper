using Amazon.S3;
using Microsoft.Extensions.Options;

namespace Lioncore.WebApi.Services;

public class Storage
{
    public const string BucketSystem = "system";
    public const string BucketFiles = "virtual";
    public StorageOptions Options { get; }
    public IAmazonS3 S3Client { get; }

    public Storage(IOptions<StorageOptions> optionsAccessor)
    {
        Options = optionsAccessor.Value;

        S3Client = new AmazonS3Client(
            Options.AccessKey,
            Options.SecretKey,
            new AmazonS3Config
            {
                ServiceURL = Options.ServiceURL,
                AuthenticationRegion = Options.Region,
                ForcePathStyle = Options.ForcePathStyle,
            }
        );
    }

    public static void Initialize(IServiceProvider services)
    {
        var storage = services.GetRequiredService<Storage>();

        storage.S3Client.EnsureBucketExistsAsync(BucketSystem).Wait();
        storage.S3Client.EnsureBucketExistsAsync(BucketFiles).Wait();
    }
}
