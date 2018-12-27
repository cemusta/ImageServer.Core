using System.Collections.Generic;

namespace ImageServer.Core.Model
{
    public class HostConfig
    {
        public string Slug { get; set; }

        public HostType Type { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string Backend { get; set; }

        public string Path { get; set; }

        public List<string> WhiteList { get; set; }

        public string FallbackImage { get; set; }
    }

    public enum HostType
    {
        FileSystem = 0,
        GridFs = 1,
        Web = 2,
        GoogleBucket = 3,
    }

}