using System;
using System.IO;
using System.Threading.Tasks;
using ImageServer.Core.Model;

namespace ImageServer.Core.Helpers
{
    public static class FileSystemHelper
    {

        public static byte[] GetFile(HostConfig host, string file)
        {
            try
            {
                var filepath = host.Path + file;
                var b = File.ReadAllBytes(filepath);
                return b;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //return null;
                throw;
            }  
        }

        public static async Task<byte[]> GetFileAsync(HostConfig host, string file)
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