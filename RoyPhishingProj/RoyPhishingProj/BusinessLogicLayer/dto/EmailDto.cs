using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RoyPhishingProj.BusinessLogicLayer.dto
{
    public class EmailDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}
