namespace SP_Task1.ServiceModel
{
    public class PurchaseOrderLine
    {
        public string Description { get; set; }
        public double? Quantity { get; set; }
        public bool IsPaired { get; set; }
    }
}
