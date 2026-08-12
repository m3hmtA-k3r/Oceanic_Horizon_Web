using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Oceanic_Horizon_Travel.Entities.Common
{
    public abstract class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }   // Kaydın oluşturulma tarihi
    }
}
