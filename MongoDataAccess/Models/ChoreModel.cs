using MongoDB.Bson.Serialization.Attributes;

namespace MongoDataAccess.Models
{
    public class ChoreModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string ChoreText { get; set; }
        public int FrequencyInDays { get; set; }
        public UserModel? AssignedTo { get; set; }
        public DateTime? LastCompleted { get; set; }
    }
}