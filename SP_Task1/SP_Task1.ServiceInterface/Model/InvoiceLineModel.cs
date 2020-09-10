using MongoDB.Bson.Serialization.Attributes;

namespace SP_Task1.ServiceInterface
{
    public class InvoiceLineModel
    {

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("quantity")]
        public double? Quantity { get; set; }

        [BsonElement("unitPrice")]
        public double? UnitPrice { get; set; }

        [BsonElement("taxRate")]
        public double? TaxRate { get; set; }

        [BsonElement("taxAmount")]
        public double? TaxAmount { get; set; }

        [BsonElement("totalPrice")]
        public double? TotalPrice { get; set; }

        [BsonElement("poNumber")]
        public string PoNumber { get; set; }

        [BsonElement("isPaired")]
        public bool IsPaired { get; set; }

    }
}
