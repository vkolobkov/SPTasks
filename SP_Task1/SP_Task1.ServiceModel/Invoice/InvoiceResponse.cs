using ServiceStack;
using System;
using System.Collections.Generic;

namespace SP_Task1.ServiceModel
{
    public class InvoiceResponse
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid VendorId { get; set; }
        public Guid CustomerId { get; set; }
        public double? TaxAmount { get; set; }
        public double? NetAmount { get; set; }
        public double? TotalPrice { get; set; }
        public List<InvoiceLine> Lines { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
