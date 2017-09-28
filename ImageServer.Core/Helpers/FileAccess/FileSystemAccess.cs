using System;
using System.IO;
using System.Threading.Tasks;
using ImageServer.Core.Model;

namespace ImageServer.Core.Helpers.FileAccess
{
    public class FileSystemAccess: IFileAccessStrategy
    {
        public async Task<byte[]> GetFileAsync(HostConfig host, string file)
        {
            try
            {
                var filepath = host.Path + file;
                var b = await File.ReadAllBytesAsync(filepath);
                return b;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //return null;
                throw;
            }
        }
    }
}