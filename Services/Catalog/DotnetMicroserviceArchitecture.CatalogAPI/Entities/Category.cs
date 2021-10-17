using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DotnetMicroserviceArchitecture.CatalogAPI.Entities
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
