using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace QuickOrderAtendimento.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class EntityMongoBase
    {
        public EntityMongoBase()
        {
            this.Id = ObjectId.GenerateNewId();
        }

        [BsonId]
        public ObjectId Id { get; set; }
    }
}
