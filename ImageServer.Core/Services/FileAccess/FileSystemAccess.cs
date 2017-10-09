using System.IO;
using System.Threading.Tasks;
using ImageServer.Core.Model;

namespace ImageServer.Core.Services.FileAccess
{
    public class FileSystemAccess : IFileAccessStrategy
    {
        public async Task<byte[]> GetFileAsync(HostConfig host, string file)
        {
            var filepath = host.Path + file;
            var b = await File.ReadAllBytesAsync(filepath);
            return b;
        }
    }
}