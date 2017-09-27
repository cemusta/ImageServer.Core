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

    }

    public enum HostType
    {
        FileSystem = 0,
        MongoGridFs = 1,
        RemoteUrl = 2
    }

}