using Funq;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using ServiceStack.Validation;
using SP_Task1.ServiceInterface;
using SP_Task1.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Task1.Tests
{
    public class BusinessPartnerServiceIntegrationTests
    {
        const string BaseUri = "http://localhost:2000/";
        private readonly ServiceStackHost appHost;

        class AppHost : AppSelfHostBase
        {
            public AppHost() : base(nameof(BusinessPartnerServiceIntegrationTests), typeof(BusinessPartnerService).Assembly) { }

            public override void Configure(Container container)
            {
                LogManager.LogFactory = new NLogFactory();

                Plugins.Add(new ValidationFeature());

                string mongoDbConnection = System.Configuration.ConfigurationManager.ConnectionStrings["mongoDb"].ConnectionString;

                container.Register<MongoDbContext>(i => new MongoDbContext(mongoDbConnection));
                container.RegisterAutoWiredAs<CollectionRepository<BusinessPartnerModel>, ICollectionRepository<BusinessPartnerModel>>();
                container.RegisterAutoWiredAs<CollectionRepository<InvoiceModel>, ICollectionRepository<InvoiceModel>>();
                container.RegisterAutoWiredAs<CollectionRepository<PurchaseOrderModel>, ICollectionRepository<PurchaseOrderModel>>();

                container.RegisterValidators(typeof(GetPurchaseOrderValidator).Assembly);
            }
        }

        public BusinessPartnerServiceIntegrationTests()
        {
            appHost = new AppHost()
                .Init()
                .Start(BaseUri);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        public IServiceClient CreateClient() => new JsonServiceClient(BaseUri);


        [Test]
        public void GetBP_EmptyId_ExceptionRaised()
        {
            var client = CreateClient();
            var getBP = new GetBusinessPartner
            {
                Id = Guid.Empty
            };
            var ex = Assert.Throws<WebServiceException>(() => client.Get<BusinessPartnerResponse>(getBP));
            Assert.IsTrue(ex.ErrorCode == "NotEqual");
            Assert.IsTrue(ex.ErrorMessage == "Specify nonempty Id for BusinessPartner.");
        }

        [Test]
        public void GetPO_NonExistingId_ExceptionRaised()
        {
            var client = CreateClient();
            Guid nonExistingId = Guid.NewGuid();
            var getBP = new GetBusinessPartner
            {
                Id = nonExistingId
            };
            var ex = Assert.Throws<WebServiceException>(() => client.Get<BusinessPartnerResponse>(getBP));
            Assert.IsTrue(ex.ErrorCode == "NotFound");
            Assert.IsTrue(ex.ErrorMessage == $"There is no BusinessPartner with id : {nonExistingId}");
        }

        [Test]
        public void CreateBP_IncorrectBP_ExceptionRaised()
        {
            var createBP = new CreateBusinessPartner
            {
                Url = "https://starbucks.com",
                Fax = "(206) 447-1575",
                Address = new Address
                {
                    Street2 = "Suite 800",
                    State = "WA"
                }
            };

            var client = CreateClient();
            var ex = Assert.Throws<WebServiceException>(() => client.Post<BusinessPartnerResponse>(createBP));
            Assert.IsTrue(ex.ResponseStatus.Errors.Count == 7);
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify Name for BusinessPartner."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify Email for BusinessPartner."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify Telephone for BusinessPartner."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify Street1 of Address."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify City of Address."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify ZipCode of Address."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify Country of Address."));
        }

        [Test]
        public void UpdateBP_IncorrectBP_ExceptionRaised()
        {
            var updateBP = new UpdateBusinessPartner
            {
                Url = "https://starbucks.com",
                Fax = "(206) 447-1575",
                Address = new Address
                {
                    Street2 = "Suite 800",
                    State = "WA"
                }
            };

            var client = CreateClient();
            var ex = Assert.Throws<WebServiceException>(() => client.Put<BusinessPartnerResponse>(updateBP));
            Assert.IsTrue(ex.ResponseStatus.Errors.Count == 8);
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify Id for BusinessPartner."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify Name for BusinessPartner."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify Email for BusinessPartner."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify Telephone for BusinessPartner."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify Street1 of Address."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify City of Address."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify ZipCode of Address."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify Country of Address."));
        }

        //[Test]
        //public void CreatePO_POWithOutLines_ExceptionRaised()
        //{
        //    var createPO = new CreatePurchaseOrder
        //    {
        //        Number = string.Empty,
        //        CustomerId = Guid.Empty,
        //        Lines = new List<PurchaseOrderLine>()
        //    };

        //    var client = CreateClient();
        //    var ex = Assert.Throws<WebServiceException>(() => client.Post<PurchaseOrderResponse>(createPO));
        //    Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify at least one Line for PurchaseOrder."));
        //}
    }
}
