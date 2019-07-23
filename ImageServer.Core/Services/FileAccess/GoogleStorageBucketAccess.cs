using System;
using System.IO;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using ImageServer.Core.Model;

namespace ImageServer.Core.Services.FileAccess
{
    public class GoogleStorageBucketAccess : IFileAccessStrategy
    {
        public async Task<byte[]> GetFileAsync(HostConfig host, string file)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    // Instantiates a client.
                    var storage = StorageClient.Create();

                    // The name for the new bucket.
                    var bucketName = host.Backend;

                    await storage.DownloadObjectAsync(bucketName, file, stream as Stream);

                    return stream.GetBuffer();
                }
            }
            catch (Google.Apis.Auth.OAuth2.Responses.TokenResponseException ex)
            {
                throw new UnauthorizedAccessException($"Google Storage Bucket ({host.Slug}|{host.Backend}): {ex.Message}");
            }
            catch (Google.GoogleApiException ex)
            {
                if (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new FileNotFoundException(ex.Message, file);
                }
                throw ex;
            }
        }
    }
}
