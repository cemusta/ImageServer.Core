using System;
using System.Threading.Tasks;
using ImageServer.Core.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace ImageServer.Core.Services.FileAccess
{
    public class GridFsAccess: IFileAccessStrategy
    {

        public async Task<byte[]> GetFileAsync(HostConfig host, string file)
        {
            int index = file.LastIndexOf('.');
            file = index == -1 ? file : file.Substring(0, index); //remove extension if any

            GridFSBucket bucket = GetBucket(host);
            byte[] bytes;
            try
            {
                var ob = new ObjectId(file);
                if (ob == ObjectId.Empty)
                    return null;
                bytes = await bucket.DownloadAsBytesAsync(ob);
            }
            catch (ArgumentException ex)
            {
                throw new GridFsObjectIdException(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex is GridFSFileNotFoundException || ex is IndexOutOfRangeException || ex is ArgumentNullException)
                {
                    return null;
                }

                //log other errors....
                throw;
            }

            return bytes;
        }

        private GridFSBucket GetBucket(HostConfig host)
        {
            var client = new MongoClient(host.ConnectionString);

            var database = client.GetDatabase(host.DatabaseName);           

            var bucket = new GridFSBucket(database);
            return bucket;
        }
    }
}
