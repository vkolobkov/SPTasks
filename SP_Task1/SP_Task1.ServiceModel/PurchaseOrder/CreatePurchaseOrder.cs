using System;
using System.Collections.Generic;

namespace SP_Task1.ServiceModel
{
    public class CreatePurchaseOrder
    {
        public string Number { get; set; }
        public Guid CustomerId { get; set; }
        public List<PurchaseOrderLine> Lines { get; set; }
    }
}
