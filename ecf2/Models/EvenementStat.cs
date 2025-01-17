using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ecf2.Models
{
    public class EvenementStat
    {
        [BsonId]
        public ObjectId Id { get; set; } 

        [BsonElement("EvenementId")]
        public int EvenementId { get; set; }

        [BsonElement("NombreParticipants")]
        public int NombreParticipants { get; set; }

    }
}
