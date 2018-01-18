using System.Threading.Tasks;
using ImageServer.Core.Model;

namespace ImageServer.Core.Services.FileMetadata
{
    public interface IFileMetadataStrategy
    {
        Task<Model.FileMetadata> GetFileMetadata(HostConfig host, string file);
    }
}