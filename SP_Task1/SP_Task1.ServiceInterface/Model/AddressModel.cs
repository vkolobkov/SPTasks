using MongoDB.Bson.Serialization.Attributes;

namespace SP_Task1.ServiceInterface
{
    public class AddressModel
    {
        [BsonElement("street1")]
        public string Street1 { get; set; }

        [BsonElement("street2")]
        public string Street2 { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("zipCode")]
        public string ZipCode { get; set; }

        [BsonElement("state")]
        public string State { get; set; }

        [BsonElement("country")]
        public string Country { get; set; }
    }
}
