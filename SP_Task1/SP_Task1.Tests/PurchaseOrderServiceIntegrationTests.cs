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

namespace SP_Task1.Tests
{
    public class PurchaseOrderServiceIntegrationTests
    {
        const string BaseUri = "http://localhost:2000/";
        private readonly ServiceStackHost appHost;

        class AppHost : AppSelfHostBase
        {
            public AppHost() : base(nameof(PurchaseOrderServiceIntegrationTests), typeof(PurchaseOrderService).Assembly) { }

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

        public PurchaseOrderServiceIntegrationTests()
        {
            appHost = new AppHost()
                .Init()
                .Start(BaseUri);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        public IServiceClient CreateClient() => new JsonServiceClient(BaseUri);


        [Test]
        public void GetPO_EmptyId_ExceptionRaised()
        {
            var client = CreateClient();
            var getPO = new GetPurchaseOrder
            {
                Id = Guid.Empty
            };
            var ex = Assert.Throws<WebServiceException>(() => client.Get<PurchaseOrderResponse>(getPO));
            Assert.IsTrue(ex.ErrorCode == "NotEqual");
            Assert.IsTrue(ex.ErrorMessage == "Specify nonempty Id for PurchaseOrder.");
        }

        [Test]
        public void GetPO_NonExistingId_ExceptionRaised()
        {
            var client = CreateClient();
            Guid nonExistingId = Guid.NewGuid();
            var getPO = new GetPurchaseOrder
            {
                Id = nonExistingId
            };
            var ex = Assert.Throws<WebServiceException>(() => client.Get<PurchaseOrderResponse>(getPO));
            Assert.IsTrue(ex.ErrorCode == "NotFound");
            Assert.IsTrue(ex.ErrorMessage == $"There is no PurchaseOrder with Id:{nonExistingId}");
        }

        [Test]
        public void CreatePO_IncorrectPO_ExceptionRaised()
        {
            var createPO = new CreatePurchaseOrder
            {
                Number = string.Empty,
                CustomerId = Guid.Empty,
                Lines = new List<PurchaseOrderLine>()
                {
                    new PurchaseOrderLine
                    {
                        Description = string.Empty,
                        Quantity = -50,
                        IsPaired = true
                    },
                    new PurchaseOrderLine
                    {
                        Description = "Robusta beans",
                        Quantity = 700,
                        IsPaired = false
                    }
                }
            };

            var client = CreateClient();
            var ex = Assert.Throws<WebServiceException>(() => client.Post<PurchaseOrderResponse>(createPO));
            Assert.IsTrue(ex.ResponseStatus.Errors.Count == 5);
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify Number for PurchaseOrder."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEqual" && e.Message == "Specify nonempty CustomerId for PurchaseOrder."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Each line in PurchaseOrder should have Description."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "GreaterThanOrEqual" && e.Message == "PurchaseOrderLine quantity should be positive or zero."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "Equal" && e.Message == "Initial IsPaired state should be false."));
        }

        [Test]
        public void CreatePO_POWithOutLines_ExceptionRaised()
        {
            var createPO = new CreatePurchaseOrder
            {
                Number = string.Empty,
                CustomerId = Guid.Empty,
                Lines = new List<PurchaseOrderLine>()
            };

            var client = CreateClient();
            var ex = Assert.Throws<WebServiceException>(() => client.Post<PurchaseOrderResponse>(createPO));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify at least one Line for PurchaseOrder."));
        }
    }
}
