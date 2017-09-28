using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageServer.Core.Model;

namespace ImageServer.Core.Helpers.FileAccess
{
    public static class FileAccessHelper
    {
        public static async Task<byte[]> GetFileAsync(string slug, string file, List<HostConfig> hosts)
        {
            var host = hosts.Find(x => x.Slug == slug);

            if (host == null)
            {
                //todo log: warning, null host request.
                return null;
            }

            var access = GetAccess(host.Type);

            return await access.GetFileAsync(host, file);
        }

        private static IFileAccessStrategy GetAccess(HostType hostType)
        {
            switch (hostType)
            {
                case HostType.FileSystem:
                    return new FileSystemAccess();
                case HostType.MongoGridFs:
                    return new GridFsAccess();
                case HostType.RemoteUrl:
                    return new WebAccess();
                default:
                    //todo log: error!, null host type.
                    throw new NotImplementedException(hostType.ToString());
            }
        }
    }
}