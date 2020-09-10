using Funq;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using ServiceStack.Validation;
using SP_Task1.ServiceInterface;
using SP_Task1.ServiceModel;

namespace SP_Task1
{
    //VS.NET Template Info: https://servicestack.net/vs-templates/EmptyAspNet
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// </summary>
        public AppHost()
            : base("SP_Task1", typeof(BusinessPartnerService).Assembly)
        {
            Routes.Add<GetBusinessPartners>("/partners", ApplyTo.Get);
            Routes.Add<GetBusinessPartner>("/partners/{Id}", ApplyTo.Get);
            Routes.Add<CreateBusinessPartner>("/partners", ApplyTo.Post);
            Routes.Add<UpdateBusinessPartner>("/partners/{Id}", ApplyTo.Put);
            Routes.Add<DeleteBusinessPartner>("/partners/{Id}", ApplyTo.Delete);

            Routes.Add<GetInvoice>("/invoices/{Id}", ApplyTo.Get);
            Routes.Add<CreateInvoice>("/invoices", ApplyTo.Post);
            Routes.Add<PatchInvoice>("/invoices/{Id}", ApplyTo.Patch);

            Routes.Add<GetPurchaseOrder>("/pos/{Id}", ApplyTo.Get);
            Routes.Add<CreatePurchaseOrder>("/pos", ApplyTo.Post);
        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
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
}