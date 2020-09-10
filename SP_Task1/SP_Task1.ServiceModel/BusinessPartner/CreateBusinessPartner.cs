namespace SP_Task1.ServiceModel
{
    public class CreateBusinessPartner
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public Address Address { get; set; }
    }
}
