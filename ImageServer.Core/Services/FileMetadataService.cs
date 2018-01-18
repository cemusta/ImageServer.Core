using System;
using System.Threading.Tasks;
using ImageServer.Core.Model;
using ImageServer.Core.Services.FileMetadata;

namespace ImageServer.Core.Services
{
    public class FileMetadataService : IFileMetadataService
    {
        public Task<Model.FileMetadata> GetFileMetadataAsync(HostConfig host, string file)
        {
            var access = GetMetadata(host.Type);

            return access.GetFileMetadata(host, file);
        }

        private static IFileMetadataStrategy GetMetadata(HostType hostType)
        {
            switch (hostType)
            {
                case HostType.GridFs:
                    return new GridFsMetadata();
                default:
                    throw new NotImplementedException(hostType.ToString());
            }
        }
    }
}