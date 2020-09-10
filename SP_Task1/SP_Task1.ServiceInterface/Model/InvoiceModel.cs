using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace SP_Task1.ServiceInterface
{
    [Table("invoices")]
    [BsonIgnoreExtraElements]
    public class InvoiceModel : ModelBase
    {
        [BsonElement("number")]
        public string Number { get; set; }

        [BsonElement("createdDate")]
        public DateTime? CreatedDate { get; set; }

        [BsonElement("dueDate")]
        public DateTime? DueDate { get; set; }

        [BsonElement("vendorId")]
        public Guid VendorId { get; set; }

        [BsonElement("customerId")]
        public Guid CustomerId { get; set; }

        [BsonElement("taxAmount")]
        public double? TaxAmount { get; set; }

        [BsonElement("netAmount")]
        public double? NetAmount { get; set; }

        [BsonElement("totalPrice")]
        public double? TotalPrice { get; set; }

        [BsonElement("lines")]
        public List<InvoiceLineModel> Lines { get; set; }
    }
}
