using ServiceStack;
using System;
using System.Collections.Generic;

namespace SP_Task1.ServiceModel
{
    public class PurchaseOrderResponse
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public Guid CustomerId { get; set; }
        public List<PurchaseOrderLine> Lines { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
