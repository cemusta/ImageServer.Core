using System.Collections.Generic;
using System.Threading.Tasks;
using ImageServer.Core.Model;

namespace ImageServer.Core.Services
{
    public interface IFileAccessService
    {
        Task<byte[]> GetFileAsync(string slug, string file, List<HostConfig> hosts);
    }
}