using MongoDB.Bson.Serialization.Attributes;

namespace ImageServer.Core.Model
{
    [BsonIgnoreExtraElements]
    public class CustomRatio
    {
        [BsonElement("Hash")]
        public string Hash;

        [BsonElement("MinRatio")]
        public double MinRatio;

        [BsonElement("MaxRatio")]
        public double MaxRatio;

        [BsonElement("X1")]
        public int X1;

        [BsonElement("Y1")]
        public int Y1;

        [BsonElement("X2")]
        public int X2;

        [BsonElement("Y2")]
        public int Y2;
    }
}