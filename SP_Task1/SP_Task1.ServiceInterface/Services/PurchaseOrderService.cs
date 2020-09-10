using NLog;
using ServiceStack;
using SP_Task1.ServiceModel;
using System;

namespace SP_Task1.ServiceInterface
{
    public class PurchaseOrderService : Service
    {
        private readonly ICollectionRepository<PurchaseOrderModel> purchaseOrderRepository;
        private static ILogger Log = LogManager.GetLogger("FileLogger");

        public PurchaseOrderService(ICollectionRepository<PurchaseOrderModel> poRepository)
        {
            purchaseOrderRepository = poRepository;
        }

        public PurchaseOrderResponse Get(GetPurchaseOrder request)
        {
            try
            {
                PurchaseOrderModel purchaseOrder = purchaseOrderRepository.Find(request.Id);
                if (purchaseOrder == default(PurchaseOrderModel))
                    throw HttpError.NotFound($"There is no PurchaseOrder with Id:{request.Id}");

                return purchaseOrder.ConvertTo<PurchaseOrderResponse>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong while getting PO.");
                throw;
            }
        }

        public PurchaseOrderResponse Post(CreatePurchaseOrder request)
        {
            try
            {
                PurchaseOrderModel result = request.ConvertTo<PurchaseOrderModel>();
                purchaseOrderRepository.Create(result);

                return result.ConvertTo<PurchaseOrderResponse>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong while posting PO.");
                throw;
            }
        }
    }
}
