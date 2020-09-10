using NLog;
using ServiceStack;
using SP_Task1.ServiceModel;
using System;
using System.Linq;

namespace SP_Task1.ServiceInterface
{
    public class InvoiceService : Service
    {
        private readonly MongoDbContext mongoDbContext;
        private readonly ICollectionRepository<InvoiceModel> invoiceRepository;
        private readonly ICollectionRepository<PurchaseOrderModel> purchaseOrderRepository;
        private static ILogger Log = LogManager.GetLogger("FileLogger");

        public InvoiceService(MongoDbContext dbContext, ICollectionRepository<InvoiceModel> iRepository, ICollectionRepository<PurchaseOrderModel> poRepository)
        {
            mongoDbContext = dbContext;
            invoiceRepository = iRepository;
            purchaseOrderRepository = poRepository;
        }

        public InvoiceResponse Get(GetInvoice request)
        {
            try
            {
                InvoiceModel invoice = invoiceRepository.Find(request.Id);
                if (invoice == default(InvoiceModel))
                    throw HttpError.NotFound($"There is no Invoice with id : {request.Id}");

                return invoice.ConvertTo<InvoiceResponse>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong while getting Invoice.");
                throw;
            }
        }

        public InvoiceResponse Post(CreateInvoice request)
        {
            try
            {
                InvoiceModel result = request.ConvertTo<InvoiceModel>();
                invoiceRepository.Create(result);

                return result.ConvertTo<InvoiceResponse>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong while posting Invoice.");
                throw;
            }
        }

        public InvoiceResponse Patch(PatchInvoice request)
        {
            InvoiceModel invoice = null;
            try
            {
                using (var session = mongoDbContext.StartSession())
                {
                    session.StartTransaction();

                    try
                    {
                        invoice = invoiceRepository.Find(session, request.Id);
                        if (invoice == default(InvoiceModel))
                            throw HttpError.NotFound($"There is no Invoice with id : {request.Id}");
                        foreach (InvoiceLineModel invoiceLine in invoice.Lines)
                        {
                            if (invoiceLine.IsPaired) continue;
                            var purchaseOrder = purchaseOrderRepository.List(session, po => po.Number == invoiceLine.PoNumber && po.CustomerId == invoice.CustomerId).FirstOrDefault();
                            var pairedPOLine = purchaseOrder.Lines.Where(pol => pol.Description == invoiceLine.Description && pol.Quantity == invoiceLine.Quantity).FirstOrDefault();
                            pairedPOLine.IsPaired = true;
                            invoiceLine.IsPaired = true;
                            var poReplaceResult = purchaseOrderRepository.Update(session, purchaseOrder);
                        }
                        var invoiceReplaceResult = invoiceRepository.Update(session, invoice);

                        session.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        session.AbortTransaction();
                        Log.Error(ex, "Something went wrong with transaction while pairing Invoice to PO.");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something general went wrong while pairing Invoice to PO.");
                throw;
            }

            return invoice?.ConvertTo<InvoiceResponse>();
        }
    }
}
