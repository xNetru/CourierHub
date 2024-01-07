using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CourierHub.Shared.Abstractions;
using System.IO.Compression;
using System.Text;

namespace CourierHub.Shared.Data;
/// <summary>
/// Implements <see cref="ICloudStorage"/> interface and provides methods for Azure Blob Storage operations.
/// </summary>
public class AzureStorage : ICloudStorage {
    private readonly string _azure;
    private readonly string _sas;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureStorage"/> class with Azure SAS token.
    /// </summary>
    /// <param name="azure">Azure storage account connection string.</param>
    /// <param name="signature">Base64 encoded shared access signature used to authenticate with the Azure service.</param>
    public AzureStorage(string azure, string signature) {
        _azure = azure;
        _sas = Encoding.UTF8.GetString(Convert.FromBase64String(signature));
    }

    /// <inheritdoc/>
    public async Task<bool> CheckBlobAsync(string path, string container) {
        BlobClient blobClient = new BlobContainerClient(new Uri(_azure + container + _sas)).GetBlobClient(path);
        return await blobClient.ExistsAsync();
    }

    /// <inheritdoc/>
    public async Task<string> GetBlobAsync(string path, string container) {
        BlobClient blobClient = new BlobContainerClient(new Uri(_azure + container + _sas)).GetBlobClient(path);
        BlobDownloadResult result = await blobClient.DownloadContentAsync();
        return result.Content.ToString();
    }

    /// <inheritdoc/>
    public async Task PutBlobAsync(string path, string container, string blob, bool gzip) {
        BlobClient blobClient = new BlobContainerClient(new Uri(_azure + container + _sas)).GetBlobClient(path + (gzip ? ".gz" : ""));
        using var ms = new MemoryStream();
        if (gzip) {
            using var compressor = new GZipStream(ms, CompressionMode.Compress);
            await compressor.WriteAsync(Encoding.UTF8.GetBytes(blob));
            await compressor.FlushAsync();
            ms.Position = 0;
            await blobClient.UploadAsync(ms, true);
        } else {
            await blobClient.UploadAsync(new BinaryData(blob), true);
        }
    }
}

