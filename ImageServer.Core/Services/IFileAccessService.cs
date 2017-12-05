using System.Threading.Tasks;

namespace ImageServer.Core.Services
{
    public interface IFileAccessService
    {
        Task<byte[]> GetFileAsync(string slug, string file);
    }
}