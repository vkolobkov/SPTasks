using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace SP_Task1.ServiceInterface
{
    [Table("businessPartners")]
    [BsonIgnoreExtraElements]
    public class BusinessPartnerModel : ModelBase
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("url")]
        public string Url { get; set; }

        [BsonElement("phone")]
        public string Telephone { get; set; }

        [BsonElement("fax")]
        public string Fax { get; set; }

        [BsonElement("address")]
        public AddressModel Address { get; set; }
    }
}
