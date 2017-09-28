using System.Threading.Tasks;
using ImageServer.Core.Model;

namespace ImageServer.Core.Helpers.FileAccess
{
    public interface IFileAccessStrategy
    {
        Task<byte[]> GetFileAsync(HostConfig host, string file);
    }
}
