using Microsoft.Extensions.Configuration;
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Domain.Context;
using SparePartsModule.Domain.Models.Library;
using SparePartsModule.Infrastructure.ViewModels.Dtos.Enums;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Suppliers;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.LibraryTariff;
using SparePartsModule.Infrastructure.ViewModels.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace SparePartsModule.Core.Library
{
    public class LibraryTariffService: ILibraryTariffService
    {

        private readonly SparePartsModuleContext _context;
        public LibraryTariffService(SparePartsModuleContext context)
        {

 
            _context = context ?? throw new ArgumentNullException(nameof(context));
     
        }

        public async ValueTask<ApiResponseModel> AddLibraryTariff(AddLibraryTariffModel model, int userId)
        {

          
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var exists = _context.MasterSPGeneralTariff.Where(e => e.Cancelled == false && e.TariffCode == model.TariffCode).FirstOrDefault();
            if (exists!=null)
            {
                if(exists.Status==(int)Status.Inactive)
                {
                    throw new ManagerProcessException("000033");
                }
                throw new ManagerProcessException("000026");
            }
           
           
                if (model.TariffPer > 100 || model.TariffPer < 0)
                {
                    throw new ManagerProcessException("000027");
                }
            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            var newId = ((int?)_context.MasterSPGeneralTariff.Max(inv => (int?)inv.TariffID) ?? 0) + 1;
            var tariff = new MasterSPGeneralTariff
            {
                TariffID=newId,


                TariffCode = model.TariffCode,
                TariffPer = model.TariffPer,
                Cancelled=false,
                Status = model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };
       
            await _context.MasterSPGeneralTariff.AddAsync(tariff);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> EditLibraryTariff(EditLibraryTariffModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!string.IsNullOrEmpty(errorCodes))
            {
                throw new ManagerProcessException(errorCodes);
            }

            var tariff = _context.MasterSPGeneralTariff.Where(e => e.TariffID == model.TariffID).FirstOrDefault();
            if (tariff == null)
            {

                throw new ManagerProcessException("000028");
            }
            var exists = _context.MasterSPGeneralTariff.Where(e => e.Cancelled == false&&e.TariffID!=model.TariffID && e.TariffCode == model.TariffCode).FirstOrDefault();
            if (exists!=null)
            {
                if(exists.Status==(int)Status.Inactive)
                {
                    throw new ManagerProcessException("000029");
                }

                throw new ManagerProcessException("000026");
            }


            if (model.TariffPer > 100 || model.TariffPer < 0)
            {
                throw new ManagerProcessException("000027");
            }
            tariff.TariffCode = model.TariffCode;
            tariff.TariffPer = model.TariffPer;
            tariff.Status = model.Status;
            tariff.ModUser = userId;
            tariff.ModDate = DateTime.Now;
            tariff.ModTime = DateTime.Now.TimeOfDay;

       
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> DeleteLibraryTariff(string TariffID, int userId)
        {
            var afftedRows = _context.MasterSPGeneralTariff.Where(e => ("," + TariffID + ",").Contains("," + e.TariffID + ","))
            .ExecuteUpdate(e =>
            e.SetProperty(x => x.Status, (int)Status.Deleted)
            .SetProperty(x => x.CancelDate, DateTime.Now)
            .SetProperty(x => x.Cancelled, true)
            .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
            .SetProperty(x => x.ModUser, userId)


            );
            if (afftedRows < 1)
            {

                throw new ManagerProcessException("000028");
            }


            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");


        }
        public async ValueTask<ApiResponseModel> ItemNameChangeStatus(string TariffID, int StatusId, int userId)
        {
            var isValids = _context.MasterLookup.Where(e => e.LookupID == StatusId && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }

            var afftedRows = _context.MasterSPGeneralTariff.Where(e => ("," + TariffID + ",").Contains("," + e.TariffID + ","))
                .ExecuteUpdate(e =>
                e.SetProperty(x => x.Status, StatusId)
                .SetProperty(x => x.ModDate, DateTime.Now)
                .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
                .SetProperty(x => x.ModUser, userId)


                );
            if (afftedRows < 1)
            {

                throw new ManagerProcessException("000028");
            }


            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetLibraryTarifs(GetLibraryTariffModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPGeneralTariff.Where(e => e.Cancelled == false &&
            (model.Search == null || e.TariffCode.Contains(model.Search))&&
            (model.Code == null || e.TariffCode.Contains(model.Code))&&
             (model.Status == null || e.Status == model.Status)
            ).Select(e => new
            {
              e.TariffID,e.TariffCode,e.TariffPer,e.EnterDate,
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
            });
            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.EnterDate);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.TariffCode);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.TariffCode);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.TariffPer);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.TariffPer);
            }
            if (model.Sort == 6)
            {
                data = data.OrderBy(e => e.Status.LookupName);
            }
            if (model.Sort == 7)
            {
                data = data.OrderByDescending(e => e.Status.LookupName);
            }
      

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }

    }
}
