using System;
using MongoDB.Bson.Serialization.Attributes;

namespace SP_Task1.ServiceInterface
{
    public class ModelBase
    {
        [BsonId]
        public Guid Id { get; set; }
    }
}
