using System.Threading.Tasks;
using ImageServer.Core.Model;

namespace ImageServer.Core.Services
{
    public interface IFileMetadataService
    {
        Task<Model.FileMetadata> GetFileMetadataAsync(HostConfig host, string file);
    }
}