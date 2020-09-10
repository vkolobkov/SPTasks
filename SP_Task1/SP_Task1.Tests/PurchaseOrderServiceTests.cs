using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using ServiceStack.Validation;
using SP_Task1.ServiceInterface;
using SP_Task1.ServiceModel;
using System;
using System.Collections.Generic;

namespace SP_Task1.Tests
{
    public class PurchaseOrderServiceTests
    {
        private readonly ServiceStackHost appHost;
        private BusinessPartnerResponse starbucksBPResponse = null;
        private CreateBusinessPartner starbucksBP = new CreateBusinessPartner
        {
            Name = "Starbucks Corporation",
            Email = "contact@starbucks.com",
            Url = "https://starbucks.com",
            Telephone = "(206) 447-1575",
            Fax = "(206) 447-1575",
            Address = new Address
            {
                Street1 = "2401 Utah Avenue, South",
                Street2 = "Suite 800",
                City = "Seattle",
                ZipCode = "98134",
                State = "WA",
                Country = "US"
            }
        };
        private PurchaseOrderResponse coffeePOResponse = null;
        private CreatePurchaseOrder coffeePO = new CreatePurchaseOrder
        {
            Number = "PO100024",
            Lines = new List<PurchaseOrderLine>()
            {
                new PurchaseOrderLine
                {
                    Description = "Arabica beans",
                    Quantity = 35,
                    IsPaired = false
                },
                new PurchaseOrderLine
                {
                    Description = "Liberica beans",
                    Quantity = 100,
                    IsPaired = false
                }
            }
        };

        public PurchaseOrderServiceTests()
        {
            string mongoDbConnection = System.Configuration.ConfigurationManager.ConnectionStrings["mongoDb"].ConnectionString;

            appHost = new BasicAppHost().Init();
            appHost.Container.AddTransient<InvoiceService>();
            appHost.Container.AddTransient<BusinessPartnerService>();
            appHost.Container.AddTransient<PurchaseOrderService>();

            appHost.Plugins.Add(new ValidationFeature());
            appHost.Container.RegisterValidator(typeof(GetPurchaseOrderValidator));

            appHost.Container.Register<MongoDbContext>(i => new MongoDbContext(mongoDbConnection));
            appHost.Container.RegisterAutoWiredAs<CollectionRepository<BusinessPartnerModel>, ICollectionRepository<BusinessPartnerModel>>();
            appHost.Container.RegisterAutoWiredAs<CollectionRepository<InvoiceModel>, ICollectionRepository<InvoiceModel>>();
            appHost.Container.RegisterAutoWiredAs<CollectionRepository<PurchaseOrderModel>, ICollectionRepository<PurchaseOrderModel>>();
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {

            var bpService = appHost.Container.Resolve<BusinessPartnerService>();
            starbucksBPResponse = bpService.Post(starbucksBP);

            var poService = appHost.Container.Resolve<PurchaseOrderService>();
            coffeePO.CustomerId = starbucksBPResponse.Id;
            coffeePOResponse = poService.Post(coffeePO);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            var bpService = appHost.Container.Resolve<BusinessPartnerService>();
            bpService.Delete(new DeleteBusinessPartner { Id = starbucksBPResponse.Id });
            appHost.Dispose();
        }

        [Test]
        public void CreatePO_CorrectPO_POCreated()
        {
            var createPO = new CreatePurchaseOrder
            {
                Number = "PO100025",
                CustomerId = starbucksBPResponse.Id,
                Lines = new List<PurchaseOrderLine>()
                {
                    new PurchaseOrderLine
                    {
                        Description = "Excelsa beans",
                        Quantity = 50,
                        IsPaired = false
                    },
                    new PurchaseOrderLine
                    {
                        Description = "Robusta beans",
                        Quantity = 700,
                        IsPaired = false
                    }
                }
            };

            var service = appHost.Container.Resolve<PurchaseOrderService>();
            var response = service.Post(createPO);
            Assert.IsTrue(response.Id != Guid.Empty);
        }

        [Test]
        public void GetPO_CorrectId_POReturned()
        {
            var getPO = new GetPurchaseOrder
            {
                Id = coffeePOResponse.Id
            };

            var service = appHost.Container.Resolve<PurchaseOrderService>();
            var response = service.Get(getPO);
            Assert.IsTrue(response.Id == coffeePOResponse.Id);
            Assert.IsTrue(response.Number == coffeePO.Number);
        }

    }
}
