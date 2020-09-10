using ServiceStack;
using System.Collections.Generic;

namespace SP_Task1.ServiceModel
{
    public class BusinessPartnersResponse
    {
        public List<BusinessPartnerResponse> BusinessPartners { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
