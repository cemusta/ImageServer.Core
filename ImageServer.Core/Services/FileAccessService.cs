using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageServer.Core.Model;
using ImageServer.Core.Services.FileAccess;
using Microsoft.Extensions.Options;

namespace ImageServer.Core.Services
{
    public class FileAccessService : IFileAccessService
    {
        private readonly List<HostConfig> _hosts;

        public FileAccessService(IOptions<List<HostConfig>> hosts)
        {
            _hosts = hosts?.Value ?? 
                     throw new ArgumentNullException(nameof(hosts));
        }

        public HostConfig GetHostConfig(string slug)
        {
            var host = _hosts.Find(x => x.Slug == slug);

            if (host == null)
            {
                throw new SlugNotFoundException($"Unknown host slug requested: {slug}");
            }

            return host;
        }

        public async Task<byte[]> GetFileAsync(HostConfig hostConfig, string file)
        {
            var access = GetAccess(hostConfig.Type);

            return await access.GetFileAsync(hostConfig, file);
        }

        private IFileAccessStrategy GetAccess(HostType hostType)
        {
            switch (hostType)
            {
                case HostType.FileSystem:
                    return new FileSystemAccess();
                case HostType.GridFs:
                    return new GridFsAccess();
                case HostType.Web:
                    return new WebAccess();
                default:
                    throw new NotImplementedException(hostType.ToString());
            }
        }
    }
}