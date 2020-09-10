using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using SP_Task1.ServiceInterface;
using SP_Task1.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SP_Task1.Tests
{
    public class InvoiceServiceTests
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
        private InvoiceResponse softwareInvoiceResponse = null;
        private CreateInvoice softwareInvoice = new CreateInvoice
        {
            Number = "INV00900107",
            CreatedDate = DateTime.Now,
            DueDate = DateTime.Now.Date.AddDays(30),
            TaxAmount = 420,
            NetAmount = 2100,
            TotalPrice = 2520,
            Lines = new List<InvoiceLine>()
            {
                new InvoiceLine
                {
                    Description = "Windows",
                    Quantity = 10,
                    UnitPrice = 130,
                    TaxRate = 20,
                    TaxAmount = 260,
                    TotalPrice = 1560,
                    PoNumber = "PO000000",
                    IsPaired = false
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

        public InvoiceServiceTests()
        {
            string mongoDbConnection = System.Configuration.ConfigurationManager.ConnectionStrings["mongoDb"].ConnectionString;

            appHost = new BasicAppHost().Init();
            appHost.Container.AddTransient<InvoiceService>();
            appHost.Container.AddTransient<BusinessPartnerService>();
            appHost.Container.AddTransient<PurchaseOrderService>();

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
            microsoftBPResponse = bpService.Post(microsoftBP);

            var invoiceService = appHost.Container.Resolve<InvoiceService>();
            softwareInvoice.CustomerId = starbucksBPResponse.Id;
            softwareInvoice.VendorId = microsoftBPResponse.Id;
            softwareInvoiceResponse = invoiceService.Post(softwareInvoice);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            var bpService = appHost.Container.Resolve<BusinessPartnerService>();
            bpService.Delete(new DeleteBusinessPartner { Id = starbucksBPResponse.Id });
            appHost.Dispose();
        }

        [Test]
        public void CreateInvoice_CorrectInvoice_InvoiceCreated()
        {
            var createInvoice = new CreateInvoice
            {
                Number = "INV00900299",
                CreatedDate = DateTime.Now,
                DueDate = DateTime.Now.Date.AddDays(30),
                CustomerId = starbucksBPResponse.Id,
                VendorId = microsoftBPResponse.Id,
                TaxAmount = 368.99,
                NetAmount = 1844.95,
                TotalPrice = 2213.94,
                Lines = new List<InvoiceLine>()
            {
                new InvoiceLine
                {
                    Description = "Xbox One S NBA 2K20 Bundle (1TB)",
                    Quantity = 5,
                    UnitPrice = 299,
                    TaxRate = 20,
                    TaxAmount = 299,
                    TotalPrice = 1794,
                    PoNumber = "PO000000",
                    IsPaired = false
                },
                new InvoiceLine
                {
                    Description = "WRC 9 Deluxe Edition FIA World Rally Championship",
                    Quantity = 5,
                    UnitPrice = 69.99,
                    TaxRate = 20,
                    TaxAmount = 69.99,
                    TotalPrice = 419.94,
                    PoNumber = "PO000000",
                    IsPaired = false
                }
            }
            };

            var service = appHost.Container.Resolve<InvoiceService>();
            var response = service.Post(createInvoice);
            Assert.IsTrue(response.Id != Guid.Empty);
        }

        [Test]
        public void GetInvoice_CorrectId_InvoiceReturned()
        {
            var getInvoice = new GetInvoice
            {
                Id = softwareInvoiceResponse.Id
            };

            var service = appHost.Container.Resolve<InvoiceService>();
            var response = service.Get(getInvoice);
            Assert.IsTrue(response.Id == softwareInvoiceResponse.Id);
            Assert.IsTrue(response.Number == softwareInvoiceResponse.Number);
        }

        [Test]
        public void PairInvoiceToPO_CanBePaired_PairedInvoiceReturned()
        {
            var createInvoice = new CreateInvoice
            {
                Number = "INV00900300",
                CreatedDate = DateTime.Now,
                DueDate = DateTime.Now.Date.AddDays(30),
                CustomerId = starbucksBPResponse.Id,
                VendorId = microsoftBPResponse.Id,
                TaxAmount = 368.99,
                NetAmount = 1844.95,
                TotalPrice = 2213.94,
                Lines = new List<InvoiceLine>()
                {
                    new InvoiceLine
                    {
                        Description = "Xbox One S NBA 2K20 Bundle (1TB)",
                        Quantity = 5,
                        UnitPrice = 299,
                        TaxRate = 20,
                        TaxAmount = 299,
                        TotalPrice = 1794,
                        PoNumber = "PO000100",
                        IsPaired = false
                    },
                    new InvoiceLine
                    {
                        Description = "WRC 9 Deluxe Edition FIA World Rally Championship",
                        Quantity = 5,
                        UnitPrice = 69.99,
                        TaxRate = 20,
                        TaxAmount = 69.99,
                        TotalPrice = 419.94,
                        PoNumber = "PO000101",
                        IsPaired = false
                    }
                }
            };

            var invoiceService = appHost.Container.Resolve<InvoiceService>();
            var invoiceToPair = invoiceService.Post(createInvoice);

            var createPO100 = new CreatePurchaseOrder
            {
                Number = "PO000100",
                CustomerId = starbucksBPResponse.Id,
                Lines = new List<PurchaseOrderLine>()
                {
                    new PurchaseOrderLine
                    {
                        Description = "Xbox One S NBA 2K20 Bundle (1TB)",
                        Quantity = 5,
                        IsPaired = false
                    },
                    new PurchaseOrderLine
                    {
                        Description = "Xbox One S Star Wars Jedi: Fallen Order Bundle (1TB)",
                        Quantity = 5,
                        IsPaired = false
                    }
                }
            };
            var createPO101 = new CreatePurchaseOrder
            {
                Number = "PO000101",
                CustomerId = starbucksBPResponse.Id,
                Lines = new List<PurchaseOrderLine>()
                {
                    new PurchaseOrderLine
                    {
                        Description = "WRC 9 Deluxe Edition FIA World Rally Championship",
                        Quantity = 5,
                        IsPaired = false
                    },
                    new PurchaseOrderLine
                    {
                        Description = "Tony Hawk’s Pro Skater 1 + 2",
                        Quantity = 5,
                        IsPaired = false
                    }
                }
            };

            var poService = appHost.Container.Resolve<PurchaseOrderService>();
            var poToPair100 = poService.Post(createPO100);
            var poToPair101 = poService.Post(createPO101);

            var patchInvoice = new PatchInvoice
            {
                Id = invoiceToPair.Id
            };

            var pairedInvoice = invoiceService.Patch(patchInvoice);
            Assert.IsTrue(pairedInvoice.Lines.Where(l => l.IsPaired).Count() == 2);

            var getPO = new GetPurchaseOrder
            {
                Id = poToPair100.Id
            };
            var partialyPairedPO100 = poService.Get(getPO);
            Assert.IsTrue(partialyPairedPO100.Lines.Where(l => l.IsPaired).Count() == 1);
            getPO.Id = poToPair101.Id;
            var partialyPairedPO101 = poService.Get(getPO);
            Assert.IsTrue(partialyPairedPO101.Lines.Where(l => l.IsPaired).Count() == 1);
        }

    }
}
