using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageServer.Core.Model;
using ImageServer.Core.Services.FileAccess;
using Microsoft.Extensions.Options;

namespace ImageServer.Core.Services
{
    public class FileAccessService: IFileAccessService
    {
        private readonly List<HostConfig> _hosts;

        public FileAccessService(IOptions<List<HostConfig>> hosts)
        {
            _hosts = hosts.Value;
        }

        public async Task<byte[]> GetFileAsync(string slug, string file)
        {
            var host = _hosts.Find(x => x.Slug == slug);

            if (host == null)
            {
                throw new Exception($"Unknown host slug requested: {slug}");
            }

            var access = GetAccess(host.Type);

            return await access.GetFileAsync(host, file);
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
                    //todo log: error!, null host type.
                    throw new NotImplementedException(hostType.ToString());
            }
        }
    }
}