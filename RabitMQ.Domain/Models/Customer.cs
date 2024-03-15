using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domains.Models
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("contact")]
        public string Contact { get; set; }
        [BsonElement("country")]
        public string Country { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }
    }
}
