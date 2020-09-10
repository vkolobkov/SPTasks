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
    public class InvoiceServiceIntegrationTests
    {
        const string BaseUri = "http://localhost:2000/";
        private readonly ServiceStackHost appHost;

        class AppHost : AppSelfHostBase
        {
            public AppHost() : base(nameof(PurchaseOrderServiceIntegrationTests), typeof(InvoiceService).Assembly) { }

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

        public InvoiceServiceIntegrationTests()
        {
            appHost = new AppHost()
                .Init()
                .Start(BaseUri);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        public IServiceClient CreateClient() => new JsonServiceClient(BaseUri);


        [Test]
        public void GetInvoice_EmptyId_ExceptionRaised()
        {
            var client = CreateClient();
            var getInvoice = new GetInvoice
            {
                Id = Guid.Empty
            };
            var ex = Assert.Throws<WebServiceException>(() => client.Get<InvoiceResponse>(getInvoice));
            Assert.IsTrue(ex.ErrorCode == "NotEqual");
            Assert.IsTrue(ex.ErrorMessage == "Specify nonempty Id for Invoice.");
        }

        [Test]
        public void PatchInvoice_EmptyId_ExceptionRaised()
        {
            var client = CreateClient();
            var patchInvoice = new PatchInvoice
            {
                Id = Guid.Empty
            };
            var ex = Assert.Throws<WebServiceException>(() => client.Patch<InvoiceResponse>(patchInvoice));
            Assert.IsTrue(ex.ErrorCode == "NotEqual");
            Assert.IsTrue(ex.ErrorMessage == "Specify nonempty Id for Invoice.");
        }

        [Test]
        public void GetInvoice_NonExistingId_ExceptionRaised()
        {
            var client = CreateClient();
            Guid nonExistingId = Guid.NewGuid();
            var getInvoice = new GetInvoice
            {
                Id = nonExistingId
            };
            var ex = Assert.Throws<WebServiceException>(() => client.Get<InvoiceResponse>(getInvoice));
            Assert.IsTrue(ex.ErrorCode == "NotFound");
            Assert.IsTrue(ex.ErrorMessage == $"There is no Invoice with id : {nonExistingId}");
        }

        [Test]
        public void PatchInvoice_NonExistingId_ExceptionRaised()
        {
            var client = CreateClient();
            Guid nonExistingId = Guid.NewGuid();
            var patchInvoice = new PatchInvoice
            {
                Id = nonExistingId
            };
            var ex = Assert.Throws<WebServiceException>(() => client.Patch<InvoiceResponse>(patchInvoice));
            Assert.IsTrue(ex.ErrorCode == "NotFound");
            Assert.IsTrue(ex.ErrorMessage == $"There is no Invoice with id : {nonExistingId}");
        }

        [Test]
        public void CreateInvoice_IncorrectInvoice_ExceptionRaised()
        {
            var createInvoice = new CreateInvoice
            {
                Number = string.Empty,
                DueDate = DateTime.Now.Date.AddDays(-30),
                TaxAmount = -420,
                NetAmount = -2100,
                TotalPrice = -25200,
                Lines = new List<InvoiceLine>()
                {
                    new InvoiceLine
                    {
                        Description = string.Empty,
                        Quantity = -10,
                        UnitPrice = -130,
                        TaxRate = -2000,
                        TaxAmount = -260,
                        TotalPrice = -15600,
                        PoNumber = string.Empty,
                        IsPaired = true
                    },
                    new InvoiceLine
                    {
                        Description = "Office",
                        Quantity = 20,
                        UnitPrice = 40,
                        TaxRate = 20,
                        TaxAmount = 160,
                        TotalPrice = 960,
                        PoNumber = "PO000000",
                        IsPaired = false
                    }
                }
            };

            var client = CreateClient();
            var ex = Assert.Throws<WebServiceException>(() => client.Post<InvoiceResponse>(createInvoice));
            Assert.IsTrue(ex.ResponseStatus.Errors.Count == 19);
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify Number for Invoice."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify CreatedDate for Invoice."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "GreaterThan" && e.Message == "DueDate of Invoice should be greater than today."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEqual" && e.Message == "Specify nonempty VendorId for Invoice."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEqual" && e.Message == "Specify nonempty CustomerId for Invoice."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "GreaterThanOrEqual" && e.Message == "TaxAmount of Invoice should be greater than or equal to 0."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "GreaterThanOrEqual" && e.Message == "NetAmount of Invoice should be greater than or equal to 0."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "GreaterThanOrEqual" && e.Message == "TotalPrice of Invoice should be greater than or equal to 0."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "Predicate" && e.Message == "TotalPrice should be equals to NetAmount+TaxAmount with tolerance:0.5."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Each line in Invoice should have Description."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "GreaterThanOrEqual" && e.Message == "Invoice quantity should be positive or zero."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "GreaterThanOrEqual" && e.Message == "Invoice UnitPrice should be positive or zero."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "GreaterThanOrEqual" && e.Message == "Invoice TaxRate should be positive or zero."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "GreaterThanOrEqual" && e.Message == "Invoice TaxAmount should be positive or zero."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "GreaterThanOrEqual" && e.Message == "Invoice TotalPrice should be positive or zero."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "PurchaseOrder number should be specified for each line in Invoice."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "Equal" && e.Message == "Initial IsPaired state should be false."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "Predicate" && e.Message == "TaxAmount doesn't corresponds to TaxRate with tolerance:0.5."));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "Predicate" && e.Message == "Total of line should be equal to UnitPrice*Quantity+TaxAmount with tolerance:0.5."));
        }

        [Test]
        public void CreateInvoice_InvoiceWithOutLines_ExceptionRaised()
        {
            var createInvoice = new CreateInvoice
            {
                Number = "INV00900109",
                CreatedDate = DateTime.Now,
                DueDate = DateTime.Now.Date.AddDays(30),
                TaxAmount = 420,
                NetAmount = 2100,
                TotalPrice = 2520,
                Lines = new List<InvoiceLine>()
            };

            var client = CreateClient();
            var ex = Assert.Throws<WebServiceException>(() => client.Post<InvoiceResponse>(createInvoice));
            Assert.IsTrue(ex.ResponseStatus.Errors.Any(e => e.ErrorCode == "NotEmpty" && e.Message == "Specify at least one Line for Invoice."));
        }
    }
}
