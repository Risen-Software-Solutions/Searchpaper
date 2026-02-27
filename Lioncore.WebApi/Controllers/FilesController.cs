using Amazon.S3;
using Lioncore.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;

namespace Lioncore.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly Storage _storage;

    public FilesController(Storage storage)
    {
        _storage = storage;
    }

    [HttpGet("{*key:file}")]
    public async Task<IActionResult> Download(string key = "")
    {
        try
        {
            var response = await _storage.S3Client.GetObjectAsync(Storage.BucketSystem, key);

            return File(response.ResponseStream, "application/octet-stream");
        }
        catch (AmazonS3Exception ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            return Problem();
        }
    }

    [HttpGet("{*folder:nonfile}")]
    public async Task<IActionResult> List(string folder = "")
    {
        var response = await _storage.S3Client.ListObjectsV2Async(
            new Amazon.S3.Model.ListObjectsV2Request
            {
                BucketName = Storage.BucketSystem,
                Delimiter = "/",
                Prefix = folder,
            }
        );

        var files = response.S3Objects;

        return Ok(
            new
            {
                files = files ?? [],
                CommonPrefixes = response.CommonPrefixes ?? [],
                response.Prefix,
            }
        );
    }
}
