using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Task1.ServiceModel
{
    public class BusinessPartnerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public Address Address { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
