using System.Collections.Generic;
using System.Threading.Tasks;
using ImageServer.Core.Model;

namespace ImageServer.Core.Helpers
{
    public static class FileHelper
    {
        public static async Task<byte[]> GetFileAsync(string slug, string id, List<HostConfig> hosts)
        {
            var host = hosts.Find(x => x.Slug == slug);

            if (host == null)
            {
                //todo log: warning, null host request.
                return null;
            }

            switch (host.Type)
            {
                case HostType.FileSystem:
                    return await FileSystemHelper.GetFileAsync(host, id);

                case HostType.MongoGridFs:
                    return await DatabaseHelper.GetFileAsync(host, id);
                    
                case HostType.RemoteUrl:
                    return await WebclientHelper.GetFileAsync(host, id);                    
            }

            //todo log: error!, null host type.
            return null;
        }
    }
}