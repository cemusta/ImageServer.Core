using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageServer.Core.Model
{
    public class AzureBlobContainer
    {
        public string ContainerClientName { get; }
        public string BlobClientName { get; }
        public AzureBlobContainer(HostConfig host, string fullQualifiedName)
        {
            if (string.IsNullOrEmpty(fullQualifiedName))
            {
                throw new ArgumentNullException(nameof(fullQualifiedName));
            }

            var splitted = fullQualifiedName.Split("/");
            if (splitted == null || !splitted.Any() || splitted.Length < 2)
            {
                throw new Exception("Fully qualified name should have at least two parts. For instance; /container/client-path");
            }

            ContainerClientName = string.IsNullOrWhiteSpace(host.Backend) ? splitted.FirstOrDefault() : host.Backend;
            BlobClientName = string.Join("/", string.IsNullOrWhiteSpace(host.Backend) ? splitted.Skip(1) : splitted);
        }
    }
}