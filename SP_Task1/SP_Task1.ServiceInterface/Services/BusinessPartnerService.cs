using MongoDB.Driver;
using NLog;
using ServiceStack;
using SP_Task1.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SP_Task1.ServiceInterface
{
    public class BusinessPartnerService : Service
    {
        private readonly ICollectionRepository<BusinessPartnerModel> businessPartnerRepository;
        private static ILogger Log = LogManager.GetLogger("FileLogger");

        public BusinessPartnerService(ICollectionRepository<BusinessPartnerModel> bpRepository)
        {
            businessPartnerRepository = bpRepository;
        }

        public BusinessPartnerResponse Get(GetBusinessPartner request)
        {
            try
            {
                BusinessPartnerModel businessPartner = businessPartnerRepository.Find(request.Id);
                if (businessPartner == default(BusinessPartnerModel))
                    throw HttpError.NotFound($"There is no BusinessPartner with id : {request.Id}");

                return businessPartner.ConvertTo<BusinessPartnerResponse>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong while getting BusinessPartner.");
                throw;
            }
        }

        public BusinessPartnersResponse Get(GetBusinessPartners request)
        {
            try
            {
                IEnumerable<BusinessPartnerModel> businessPartners = businessPartnerRepository.List();
                BusinessPartnersResponse response = new BusinessPartnersResponse
                {
                    BusinessPartners = businessPartners.Select(bp => bp.ConvertTo<BusinessPartnerResponse>()).ToList()
                };

                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong while getting BusinessPartners list.");
                throw;
            }
        }

        public BusinessPartnerResponse Post(CreateBusinessPartner request)
        {
            try
            {
                BusinessPartnerModel result = request.ConvertTo<BusinessPartnerModel>();
                businessPartnerRepository.Create(result);

                return result.ConvertTo<BusinessPartnerResponse>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong while posting BusinessPartner.");
                throw;
            }
        }

        public BusinessPartnerResponse Put(UpdateBusinessPartner request)
        {
            try
            {
                BusinessPartnerModel bpModel = request.ConvertTo<BusinessPartnerModel>();
                var replaceResult = businessPartnerRepository.Upsert(bpModel);
                BusinessPartnerResponse response = bpModel.ConvertTo<BusinessPartnerResponse>();

                if (!replaceResult.IsAcknowledged)
                {
                    Log.Error($"Upsert operation was not Acknowledged for BusinessPartner with Id:{request.Id} Name:{request.Name}.");
                    throw new Exception($"Upsert operation was not Acknowledged for BusinessPartner with Id:{request.Id} Name:{request.Name}.");
                }

                if (replaceResult.UpsertedId != null)
                    response.Id = replaceResult.UpsertedId.AsGuid;

                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong while upserting BusinessPartner.");
                throw;
            }
        }

        public HttpResult Delete(DeleteBusinessPartner request)
        {
            try
            {
                DeleteResult result = businessPartnerRepository.Delete(request.Id);

                return new HttpResult()
                {
                    StatusCode = System.Net.HttpStatusCode.NoContent
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong while deleting BusinessPartner.");
                throw;
            }
        }
    }
}
