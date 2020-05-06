using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Google.Cloud.Storage.V1;
using ImageServer.Core.Model;
using Microsoft.Extensions.Configuration;

namespace ImageServer.Core.Services.FileAccess
{
    public class AzureBlobStorageAccess : IFileAccessStrategy
    {
        public string ConnectionString { get; }
        public AzureBlobStorageAccess(IConfiguration configuration)
        {
            this.ConnectionString = configuration.GetValue<string>("AzureConnectionString");
        }

        public async Task<byte[]> GetFileAsync(HostConfig host, string file)
        {
            try
            {
                AzureBlobContainer container = new AzureBlobContainer(host, file);

                BlobServiceClient serviceClient = new BlobServiceClient(ConnectionString);
                var containerClient = serviceClient.GetBlobContainerClient(container.ContainerClientName);
                if (!await containerClient.ExistsAsync())
                {
                    throw new FileNotFoundException(file);
                }

                var blobClient = containerClient.GetBlobClient(container.BlobClientName);
                if (!await blobClient.ExistsAsync())
                {
                    throw new FileNotFoundException(file);
                }

                using var stream = new MemoryStream();
                var download = await blobClient.DownloadAsync();
                await download.Value.Content.CopyToAsync(stream);
                return stream.GetBuffer();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
