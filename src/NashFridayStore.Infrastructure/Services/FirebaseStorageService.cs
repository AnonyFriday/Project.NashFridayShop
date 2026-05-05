using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using NashFridayStore.Infrastructure.AppOptions;
using NashFridayStore.Infrastructure.Interfaces;

namespace NashFridayStore.Infrastructure.Services;

public class FirebaseStorageService : IStorageService
{
    private readonly FirebaseOptions _options;
    private readonly StorageClient _storageClient;

    public FirebaseStorageService(IOptions<FirebaseOptions> options, StorageClient storageClient)
    {
        _options = options.Value;
        _storageClient = storageClient;
    }

    public async Task<string> UploadFileAsync(Stream stream, string fileName, string? folderPath, string contentType, CancellationToken ct)
    {        
        if(!string.IsNullOrEmpty(folderPath))
        {
            fileName = $"{folderPath}/{fileName}";
        }

        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        _ = await _storageClient.UploadObjectAsync(
            _options.Bucket,
            fileName,
            contentType,
            stream,
            cancellationToken: ct);

        return  $"https://storage.googleapis.com/{_options.Bucket}/{fileName}";
    }

    public async Task<bool> DeleteFileAsync(string fileName, string? folderPath, CancellationToken ct)
    {
        try
        {
            if(!string.IsNullOrEmpty(folderPath))
            {
                fileName = $"{folderPath}/{fileName}";
            }

            await _storageClient.DeleteObjectAsync(_options.Bucket, fileName, cancellationToken: ct);
            return true;
        }
        catch (Google.GoogleApiException ex) when (ex.Error.Code == 404)
        {
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
