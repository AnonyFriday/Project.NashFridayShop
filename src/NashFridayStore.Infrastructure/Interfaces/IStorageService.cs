namespace NashFridayStore.Infrastructure.Interfaces;

public interface IStorageService
{
    Task<string> UploadFileAsync(Stream stream, string fileName, string? folderPath, string contentType, CancellationToken ct);
    Task<bool> DeleteFileAsync(string fileName, string? folderPath, CancellationToken ct);
}
