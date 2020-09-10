using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using SP_Task1.ServiceInterface;
using SP_Task1.ServiceModel;
using System;
using System.Linq;

namespace SP_Task1.Tests
{
    public class BusinessPartnerServiceTests
    {
        private readonly ServiceStackHost appHost;
        private Guid missingGuid = Guid.NewGuid();
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
        private BusinessPartnerResponse geBPResponse = null;
        private CreateBusinessPartner geBP = new CreateBusinessPartner
        {
            Name = "General Electric Company",
            Email = "contact@ge.com",
            Url = "https://www.ge.com",
            Telephone = "+001 877 394 9775",
            Fax = "+001 877 394 9775",
            Address = new Address
            {
                Street1 = "5 Necco Street",
                Street2 = string.Empty,
                City = "Boston",
                ZipCode = "02210",
                State = "MA",
                Country = "US"
            }
        };
        private BusinessPartnerResponse lufthansaBPResponse = null;
        private CreateBusinessPartner lufthansaBP = new CreateBusinessPartner
        {
            Name = "Deutsche Lufthansa AG",
            Email = "contact@lufthansa.com",
            Url = "https://www.lufthansa.com",
            Telephone = "+49 (0) 69 86 799 799",
            Fax = "+49 (0) 69 86 799 799",
            Address = new Address
            {
                Street1 = "Flughafen-Bereich West",
                Street2 = string.Empty,
                City = "Frankfurt/Main",
                ZipCode = "60546",
                State = string.Empty,
                Country = "DE"
            }
        };
        private BusinessPartnerResponse microsoftBPResponse = null;
        private CreateBusinessPartner microsoftBP = new CreateBusinessPartner
        {
            Name = "Microsoft Corporation",
            Email = "contact@microsoft.com",
            Url = "https://www.microsoft.com",
            Telephone = "(800) 426-9400",
            Fax = "(800) 426-9400",
            Address = new Address
            {
                Street1 = "One Microsoft Way",
                Street2 = string.Empty,
                City = "Redmond",
                ZipCode = "98052-6399",
                State = "WA",
                Country = "US"
            }
        };

        public BusinessPartnerServiceTests()
        {
            string mongoDbConnection = System.Configuration.ConfigurationManager.ConnectionStrings["mongoDb"].ConnectionString;
            appHost = new BasicAppHost().Init();
            appHost.Container.AddTransient<BusinessPartnerService>();
            appHost.Container.Register<MongoDbContext>(i => new MongoDbContext(mongoDbConnection));
            appHost.Container.RegisterAutoWiredAs<CollectionRepository<BusinessPartnerModel>, ICollectionRepository<BusinessPartnerModel>>();
            appHost.Container.RegisterAutoWiredAs<CollectionRepository<InvoiceModel>, ICollectionRepository<InvoiceModel>>();
            appHost.Container.RegisterAutoWiredAs<CollectionRepository<PurchaseOrderModel>, ICollectionRepository<PurchaseOrderModel>>();
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var service = appHost.Container.Resolve<BusinessPartnerService>();
            starbucksBPResponse = service.Post(starbucksBP);
            geBPResponse = service.Post(geBP);
            lufthansaBPResponse = service.Post(lufthansaBP);
            microsoftBPResponse = service.Post(microsoftBP);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            var service = appHost.Container.Resolve<BusinessPartnerService>();
            service.Delete(new DeleteBusinessPartner { Id = starbucksBPResponse.Id });
            service.Delete(new DeleteBusinessPartner { Id = geBPResponse.Id });
            service.Delete(new DeleteBusinessPartner { Id = lufthansaBPResponse.Id });
            service.Delete(new DeleteBusinessPartner { Id = missingGuid });
            appHost.Dispose();
        }

        [Test]
        public void CreateBP_CorrectBP_BPCreated()
        {
            var createBP = new CreateBusinessPartner
            {
                Name = "XYZ Organization",
                Email = "xyz@gmail.com",
                Url = "https://xyz.com",
                Telephone = "123-45-6789",
                Fax = "123-45-1234",
                Address = new Address
                {
                    Street1 = "Großer Hirschgraben 15",
                    Street2 = string.Empty,
                    City = "Frankfurt",
                    ZipCode = "60311",
                    State = string.Empty,
                    Country = "DE"
                }
            };

            var service = appHost.Container.Resolve<BusinessPartnerService>();
            var response = service.Post(createBP);
            Assert.IsTrue(response.Id != Guid.Empty);
        }

        [Test]
        public void GetBP_CorrectId_BPReturned()
        {
            var getBP = new GetBusinessPartner
            {
                Id = starbucksBPResponse.Id
            };

            var service = appHost.Container.Resolve<BusinessPartnerService>();
            var response = service.Get(getBP);
            Assert.IsTrue(response.Id == starbucksBPResponse.Id);
            Assert.IsTrue(response.Name == starbucksBP.Name);
        }

        [Test]
        public void GetBPs_WithoutFilter_AllBPsReturned()
        {
            var service = appHost.Container.Resolve<BusinessPartnerService>();
            var response = service.Get(new GetBusinessPartners());
            Assert.IsTrue(response.BusinessPartners.Count >= 3);
            Assert.IsTrue(response.BusinessPartners.Any(bp => bp.Name == starbucksBP.Name));
            Assert.IsTrue(response.BusinessPartners.Any(bp => bp.Name == geBP.Name));
            Assert.IsTrue(response.BusinessPartners.Any(bp => bp.Name == lufthansaBP.Name));
        }

        [Test]
        public void UpdateBP_CorrectBP_BPUpdated()
        {
            var updateBP = new UpdateBusinessPartner
            {
                Id = starbucksBPResponse.Id,
                Name = "Starbucks Corp.",
                Email = starbucksBP.Email,
                Url = starbucksBP.Url,
                Telephone = starbucksBP.Telephone,
                Fax = starbucksBP.Fax,
                Address = starbucksBP.Address
            };

            var service = appHost.Container.Resolve<BusinessPartnerService>();
            var response = service.Put(updateBP);
            Assert.IsTrue(response.Name.Equals("Starbucks Corp.", StringComparison.InvariantCulture));
        }

        [Test]
        public void UpdateBP_MissingBP_BPInserted()
        {
            var updateBP = new UpdateBusinessPartner
            {
                Id = missingGuid,
                Name = "New Starbucks Corp.",
                Email = starbucksBP.Email,
                Url = starbucksBP.Url,
                Telephone = starbucksBP.Telephone,
                Fax = starbucksBP.Fax,
                Address = starbucksBP.Address
            };

            var service = appHost.Container.Resolve<BusinessPartnerService>();
            var response = service.Put(updateBP);
            Assert.IsTrue(response.Name.Equals("New Starbucks Corp.", StringComparison.InvariantCulture));
            Assert.IsTrue(response.Id == missingGuid);
        }

        [Test]
        public void DeleteBP_ExistingBP_BPDeleted()
        {
            var service = appHost.Container.Resolve<BusinessPartnerService>();
            var response = service.Delete(new DeleteBusinessPartner { Id = microsoftBPResponse.Id });
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NoContent);
        }
    }
}
