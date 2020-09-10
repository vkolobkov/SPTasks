using MongoDB.Bson.Serialization.Attributes;

namespace SP_Task1.ServiceInterface
{
    public class PurchaseOrderLineModel
    {
        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("quantity")]
        public double? Quantity { get; set; }

        [BsonElement("isPaired")]
        public bool IsPaired { get; set; }
    }
}
