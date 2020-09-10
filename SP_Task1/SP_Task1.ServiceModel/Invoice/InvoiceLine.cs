namespace SP_Task1.ServiceModel
{
    public class InvoiceLine
    {
        public string Description { get; set; }
        public double? Quantity { get; set; }
        public double? UnitPrice { get; set; }
        public double? TaxRate { get; set; }
        public double? TaxAmount { get; set; }
        public double? TotalPrice { get; set; }
        public string PoNumber { get; set; }
        public bool IsPaired { get; set; }
    }
}
