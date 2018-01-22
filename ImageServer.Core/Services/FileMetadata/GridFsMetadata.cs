using System;
using System.Threading.Tasks;
using ImageServer.Core.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace ImageServer.Core.Services.FileMetadata
{
    public class GridFsMetadata : IFileMetadataStrategy
    {
        public async Task<Model.FileMetadata> GetFileMetadata(HostConfig host, string file)
        {
            var index = file.LastIndexOf('.');
            file = index == -1 ? file : file.Substring(0, index); //remove extension if any

            IGridFSBucket bucket = GetBucket(host);

            try
            {
                var ob = new ObjectId(file);
                if (ob == ObjectId.Empty)
                    return null;

                var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", ob);
                var findOptions = new GridFSFindOptions
                {
                    Limit = 1
                };

                var fi = await bucket.FindAsync(filter, findOptions);

                var metadata = BsonSerializer.Deserialize<Model.FileMetadata>(fi.FirstOrDefault().Metadata);

                return metadata;
            }
            catch (ArgumentException ex)
            {
                throw new GridFsObjectIdException(ex.Message);
            }
            catch (FormatException ex)
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