using System.Threading.Tasks;
using ImageServer.Core.Model;

namespace ImageServer.Core.Services
{
    public interface IFileAccessService
    {
        HostConfig GetHostConfig(string slug);

        Task<byte[]> GetFileAsync(HostConfig host, string file);
    }
}