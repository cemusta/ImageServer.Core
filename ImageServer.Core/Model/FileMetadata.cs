using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ImageServer.Core.Model
{
    [BsonIgnoreExtraElements]
    public class FileMetadata
    {
        [BsonElement("Title")]
        public string Title;

        [BsonElement("Description")]
        public string Description;

        [BsonElement("CustomRatio")]
        public List<CustomRatio> CustomRatio;
    }
}