using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ImageServer.Core.Model;

namespace ImageServer.Core.Services.FileAccess
{
    public class WebAccess : IFileAccessStrategy
    {
        public async Task<byte[]> GetFileAsync(HostConfig host, string file)
        {
            var url = host.Backend + '/' + file;

            using (var stream = new MemoryStream())
            {

                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                using (var contentStream = await (await client.SendAsync(request)).Content.ReadAsStreamAsync())
                {
                    await contentStream.CopyToAsync(stream);
                }

                return stream.TryGetBuffer(out ArraySegment<byte> data) ? data.Array : null;
            }
        }
    }
}
