﻿using System;
using System.Threading.Tasks;
using ImageServer.Core.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace ImageServer.Core.Helpers
{
    public static class DatabaseHelper
    {

        public static async Task<byte[]> GetFileAsync(HostConfig host, string id)
        {
            GridFSBucket bucket = GetBucket(host);
            byte[] bytes;
            try
            {
                var ob = new ObjectId(id);
                if (ob == ObjectId.Empty)
                    return null;
                bytes = await bucket.DownloadAsBytesAsync(ob);
            }
            catch (Exception ex)
            {
                if (ex is GridFSFileNotFoundException || ex is IndexOutOfRangeException || ex is ArgumentNullException)
                {
                    return null;
                }

                //log other errors....
                Console.WriteLine(ex.Message);
                throw;
            }

            return bytes;
        }

        private static GridFSBucket GetBucket(HostConfig host)
        {
            var client = new MongoClient(host.ConnectionString);

            var database = client.GetDatabase(host.DatabaseName);           

            var bucket = new GridFSBucket(database);
            return bucket;
        }


    }
}
