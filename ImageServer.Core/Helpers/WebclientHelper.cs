using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ImageServer.Core.Model;

namespace ImageServer.Core.Helpers
{
    public static class WebclientHelper
    {
        public static async Task<byte[]> GetFileAsync(HostConfig host, string id)
        {
            var url = host.Backend + '/' + id;

            MemoryStream stream = new MemoryStream();

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            using ( Stream contentStream = await(await client.SendAsync(request)).Content.ReadAsStreamAsync())
            {
                await contentStream.CopyToAsync(stream);
            }

            ArraySegment<byte> data;

            return stream.TryGetBuffer(out data) ? data.Array : null;
        }
    }
}