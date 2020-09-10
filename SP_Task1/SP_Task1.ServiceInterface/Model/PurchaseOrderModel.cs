using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace SP_Task1.ServiceInterface
{
    [Table("purchaseOrders")]
    [BsonIgnoreExtraElements]
    public class PurchaseOrderModel : ModelBase
    {
        [BsonElement("number")]
        public string Number { get; set; }

        [BsonElement("customerId")]
        public Guid CustomerId { get; set; }

        [BsonElement("lines")]
        public List<PurchaseOrderLineModel> Lines { get; set; }
    }
}
