using Microsoft.Extensions.Configuration;
using SparePartsModule.Domain.Context;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Category;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using SparePartsModule.Infrastructure.ViewModels.Models.Lookups;
using SparePartsModule.Infrastructure.ViewModels.Dtos.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Domain.Models;
using SparePartsModule.Core.Helpers;

namespace SparePartsModule.Core
{
    public class LookupService: ILookupService
    {
        private readonly IConfiguration _config;
        private readonly SparePartsModuleContext _context;
        private readonly MarkaziaMasterContext _contextMarkaziaMaster;
        private readonly FileHelper _FileHelper;
        private readonly UtilitiesHelper _utilitiesHelper ;
        List<string> errors = new List<string>();

        public LookupService(SparePartsModuleContext context, IConfiguration config, FileHelper fileHelper, MarkaziaMasterContext contextMarkaziaMaster, UtilitiesHelper utilitiesHelper)
        {

            _config = config;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _FileHelper = fileHelper;
            _contextMarkaziaMaster = contextMarkaziaMaster;
            _utilitiesHelper = utilitiesHelper;
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetLookups(GetLookupsModel model, PaginationModel paginationPostModel)
        {
            if(model.Status==null)
            {
                model.Status = (int)Status.Active;
            }

            var data = _context.MasterSPLookup.Where(e => e.Cancelled == false &&e.Status==model.Status&&
          
            e.LookupTypeID == model.LookupTypeId&&
            (model.ParentId == null || e.LookupParent == model.ParentId)
            ).Select(e => new
            {
             e.LookupID,  e.LookupName,e.LookupDesc,e.LookupImage,e.LookupParent,e.Status
            });
            

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetLookupsMaster(GetLookupsModel model, PaginationModel paginationPostModel)
        {
            if (model.Status == null)
            {
                model.Status = (int)Status.Active;
            }

            var data = _contextMarkaziaMaster.MasterLookup.Where(e => e.Cancelled == false && e.Status == model.Status &&

            e.LookupTypeID == model.LookupTypeId &&
            (model.ParentId == null || e.LookupParent == model.ParentId) &&
             (model.Name == null || e.LookupName.Contains(model.Name))
            ).Select(e => new
            {
                e.LookupID,
                e.LookupName,
                e.LookupDesc,
                e.LookupValue,
                LookupImage = string.IsNullOrEmpty(e.LookupImage) ? null : (_config["Settings:BaseUrl"] + e.LookupImage.Replace(",", "," + _config["Settings:BaseUrl"])).Split(",", StringSplitOptions.None),

                e.LookupParent,
                e.Status
            });


            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetLookupsTypesMaster(GetLookupsTypeModel model, PaginationModel paginationPostModel)
        {
            if (model.Status == null)
            {
                model.Status = (int)Status.Active;
            }

            var data = _contextMarkaziaMaster.MasterLookupType.Where(e => e.Cancelled == false && e.Status == model.Status );


            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }

        public int GeSpLookup(int userId, string name, int lookupTypeId,int? parent=null)
        {
            var lookupId = _context.MasterSPLookup
               .Where(e =>e.Cancelled==false&& e.LookupName == name && e.LookupTypeID == lookupTypeId)
               .Select(e => e.LookupID).FirstOrDefault();
            if (lookupId == 0)
            {


                lookupId = ((int?)_context.MasterSPLookup.Where(e => e.LookupTypeID == lookupTypeId)
                .Max(inv => (int?)inv.LookupID) ?? lookupTypeId * 1000) + 1;
                var lookup = new MasterSPLookup
                {
                    LookupTypeID = lookupTypeId,
                    LookupID = lookupId,
                    LookupName = name,
                    LookupParent=parent,
                    LookupDefault = false,

                    LookupStatic = false,
                    Cancelled = false,
                    Status = (int)Status.Active,
                    EnterUser = userId,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay
                };

                _context.MasterSPLookup.Add(lookup);
                _context.SaveChanges();





            }

            return lookupId;
        }
        public int GetMasterLookup(int userId, string name, int lookupTypeId)
        {
            var lookupId = _context.MasterLookup
               .Where(e => e.Cancelled == false && e.LookupName == name && e.LookupTypeID == lookupTypeId)
               .Select(e => e.LookupID).FirstOrDefault();
            if (lookupId == 0)
            {

                lookupId = ((int?)_context.MasterLookup.Where(e => e.LookupTypeID == lookupTypeId)
                .Max(inv => (int?)inv.LookupID) ?? lookupTypeId * 1000) + 1;
                var lookup = new MasterLookup
                {
                    LookupTypeID = lookupTypeId,
                    LookupID = lookupId,
                    LookupName = name,

                    LookupDefault = false,

                    LookupStatic = false,
                    Cancelled = false,
                    Status = (int)Status.Active,
                    EnterUser = userId,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay
                };

                _contextMarkaziaMaster.MasterLookup.Add(lookup);

                _contextMarkaziaMaster.SaveChanges();



            }

            return lookupId;
        }
        public async ValueTask<ApiResponseModel> AddLookup(AddLookupModel model, int userId)
        {

            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var exists = _context.MasterSPLookup.Where(e => e.Cancelled == false && e.LookupTypeID == model.LookupTypeID && e.LookupName == model.LookupName).FirstOrDefault();
            if (exists != null)
            {
                if (exists.Status == (int)Status.Inactive)
                {
                    throw new ManagerProcessException("100024");
                }
                throw new ManagerProcessException("100023");
            }




            var isValid = _context.MasterSPLookupType.Where(e => e.LookupTypeID == model.LookupTypeID).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("100025");
            }

            if (model.LookupParent != null)
            {
                isValid = _context.MasterSPLookup.Where(e => e.Cancelled == false && e.LookupID == model.LookupParent).Any();

                if (!isValid)
                {

                    throw new ManagerProcessException("100026");
                }
            }
            if (model.LookupBGColor != null)
            {
                isValid = _utilitiesHelper.IsValidColorHexa(model.LookupBGColor);
                if (!isValid)
                {

                    throw new ManagerProcessException("100027");
                }
            }
            if (model.LookupTextColor != null)
            {
                isValid = _utilitiesHelper.IsValidColorHexa(model.LookupTextColor);
                if (!isValid)
                {

                    throw new ManagerProcessException("100028");
                }
            }

            var isValids = _context.MasterSPLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            var newId = ((int?)_context.MasterSPLookup.Where(e => e.LookupTypeID == model.LookupTypeID).Max(inv => (int?)inv.LookupID) ?? model.LookupTypeID * 1000) + 1;
            var lookup = new MasterSPLookup
            {
                LookupTypeID = model.LookupTypeID,
                LookupID = newId,
                LookupName = model.LookupName,

                LookupDesc = model.LookupDesc,
                LookupDefault = model.LookupDefault ?? false,
                LookupAction = model.LookupAction,
                LookupBGColor = model.LookupBGColor,
                LookupComments = model.LookupComments,
                LookupIntegration = model.LookupIntegration,
                LookupParent = model.LookupParent,
                LookupStatic = model.LookupStatic ?? false,
                LookupSystem = model.LookupSystem,
                LookupTextColor = model.LookupTextColor,
                LookupValue = model.LookupValue,
                Cancelled = false,
                Status = model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };
            if (model.LookupImage != null)
            {
                lookup.LookupImage = null;
                foreach (var image in model.LookupImage)
                {
                    var uploadResult = await _FileHelper.WriteFile(image, "Lookups");
                    lookup.LookupImage += (lookup.LookupImage == null ? "" : ",") + uploadResult.ReturnUrl;
                }

            }
            await _context.MasterSPLookup.AddAsync(lookup);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000032");
        }
        public async ValueTask<ApiResponseModel> EditLookup(EditLookupModel model, int userId)
        {

            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var lookup = _context.MasterSPLookup.Where(e => e.Cancelled == false && e.LookupID == model.LookupID).FirstOrDefault();
            if (lookup == null)
            {
                throw new ManagerProcessException("100029");
            }


            var exists = _context.MasterSPLookup.Where(e => e.Cancelled == false && e.LookupID != model.LookupID && e.LookupTypeID == model.LookupTypeID && e.LookupName == model.LookupName).FirstOrDefault();
            if (exists != null)
            {
                if (exists.Status == (int)Status.Inactive)
                {
                    throw new ManagerProcessException("100024");
                }
                throw new ManagerProcessException("100023");
            }



            var isValid = _context.MasterSPLookupType.Where(e => e.LookupTypeID == model.LookupTypeID).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("100025");
            }

            if (model.LookupParent != null)
            {
                isValid = _context.MasterSPLookup.Where(e => e.Cancelled == false && e.LookupID == model.LookupParent).Any();

                if (!isValid)
                {

                    throw new ManagerProcessException("100026");
                }
            }

            var isValids = _context.MasterSPLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }

            //isValid = _utilitiesHelper.IsArabic(model.LookupName);
            //if (isValid)
            //{
            //    throw new ManagerProcessException("000106");
            //}
            if (model.LookupBGColor != null)
            {
                isValid = _utilitiesHelper.IsValidColorHexa(model.LookupBGColor);
                if (!isValid)
                {

                    throw new ManagerProcessException("100027");
                }
            }
            if (model.LookupTextColor != null)
            {
                isValid = _utilitiesHelper.IsValidColorHexa(model.LookupTextColor);
                if (!isValid)
                {

                    throw new ManagerProcessException("100028");
                }
            }
            lookup.LookupTypeID = model.LookupTypeID;

            lookup.LookupName = model.LookupName;
            lookup.LookupDesc = model.LookupDesc;
            lookup.LookupDefault = model.LookupDefault ?? false;
            lookup.LookupAction = model.LookupAction;
            lookup.LookupBGColor = model.LookupBGColor;
            lookup.LookupComments = model.LookupComments;
            lookup.LookupIntegration = model.LookupIntegration;
            lookup.LookupParent = model.LookupParent;
            lookup.LookupStatic = model.LookupStatic ?? false;
            lookup.LookupSystem = model.LookupSystem;
            lookup.LookupTextColor = model.LookupTextColor;
            lookup.LookupValue = model.LookupValue;



            lookup.Status = model.Status;
            lookup.ModUser = userId;
            lookup.ModDate = DateTime.Now;
            lookup.ModTime = DateTime.Now.TimeOfDay;

            lookup.LookupImage = null;
            if (model.LookupImage != null)
            {
                foreach (var image in model.LookupImage)
                {
                    var uploadResult = await _FileHelper.WriteFile(image, "Lookups");
                    lookup.LookupImage += (lookup.LookupImage == null ? "" : ",") + uploadResult.ReturnUrl;
                }

            }

            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000032");
        }
        public async ValueTask<ApiResponseModel> DeleteLookup(int LookupId, int userId)
        {


            var lookup = _context.MasterSPLookup.Where(e => e.LookupID == LookupId).FirstOrDefault();
            if (lookup == null)
            {

                throw new ManagerProcessException("100029");
            }
            if (lookup.Cancelled == true)
            {

                throw new ManagerProcessException("000071");
            }

            lookup.CancelDate = DateTime.Now;
            lookup.Cancelled = true;
            lookup.ModUser = userId;
            lookup.ModDate = DateTime.Now;
            lookup.ModTime = DateTime.Now.TimeOfDay;


            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000032");
        }
        public async ValueTask<ApiResponseModel> AddLookupType(AddLookupTypeModel model, int userId)
        {

            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var exists = _context.MasterSPLookupType.Where(e => e.Cancelled == false && e.LookupTypeName == model.LookupTypeName).FirstOrDefault();
            if (exists != null)
            {
                if (exists.Status == (int)Status.Inactive)
                {
                    throw new ManagerProcessException("100024");
                }
                throw new ManagerProcessException("100023");
            }


            var isValids = _context.MasterSPLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            var newId = ((int)_context.MasterSPLookupType.Max(inv => (int?)inv.LookupTypeID)) + 1;
            var lookup = new MasterSPLookupType
            {
                LookupTypeID = newId,
                LookupTypeName = model.LookupTypeName,
                Comments = model.Comments,

                Cancelled = false,
                Status = model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };

            await _context.MasterSPLookupType.AddAsync(lookup);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000032");
        }
        public async ValueTask<ApiResponseModel> EditLookupType(EditLookupTypeModel model, int userId)
        {

            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var lookup = _context.MasterSPLookupType.Where(e => e.Cancelled == false && e.LookupTypeID == model.LookupTypeId).FirstOrDefault();
            if (lookup == null)
            {
                throw new ManagerProcessException("100025");
            }

            var exists = _context.MasterSPLookupType.Where(e => e.Cancelled == false && e.LookupTypeID != model.LookupTypeId && e.LookupTypeName == model.LookupTypeName).FirstOrDefault();
            if (exists != null)
            {
                if (exists.Status == (int)Status.Inactive)
                {
                    throw new ManagerProcessException("100024");
                }
                throw new ManagerProcessException("100023");
            }


            var isValids = _context.MasterSPLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }

            lookup.LookupTypeName = model.LookupTypeName;
            lookup.Comments = model.Comments;

            lookup.Status = model.Status;
            lookup.ModUser = userId;
            lookup.ModDate = DateTime.Now;
            lookup.ModTime = DateTime.Now.TimeOfDay;

            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000032");
        }
        public async ValueTask<ApiResponseModel> DeleteLookupType(int LookupTypeId, int userId)
        {


            var lookup = _context.MasterSPLookupType.Where(e => e.LookupTypeID == LookupTypeId).FirstOrDefault();
            if (lookup == null)
            {

                throw new ManagerProcessException("100025");
            }
            if (lookup.Cancelled == true)
            {

                throw new ManagerProcessException("000071");
            }

            lookup.CancelDate = DateTime.Now;
            lookup.Cancelled = true;
            lookup.ModUser = userId;
            lookup.ModDate = DateTime.Now;
            lookup.ModTime = DateTime.Now.TimeOfDay;


            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000032");
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetLookupTypes(GetLookupTypesModel model, PaginationModel paginationPostModel)
        {
            //if (model.Status == null)
            //{
            //    model.Status = (int)Status.Active;
            //}

            var data = _context.MasterSPLookupType.Where(e => e.Cancelled == false && (model.Status == null || e.Status == model.Status) &&



             (model.Name == null || e.LookupTypeName.Contains(model.Name))
            ).Select(e => new
            {
                e.LookupTypeID,
                e.LookupTypeName,
                e.Comments,
                LookupsCount = _context.MasterSPLookup.Where(x => x.LookupTypeID == e.LookupTypeID && e.Cancelled == false).Count(),


                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                e.EnterDate,
                e.EnterTime,
                e.ModDate,
                e.ModTime,
                EnterUser = _context.Users.Where(x => x.UserId == e.EnterUser).Select(x => new { x.UserId, x.FullName }).FirstOrDefault(),
                ModUser = _context.Users.Where(x => x.UserId == e.ModUser).Select(x => new { x.UserId, x.FullName }).FirstOrDefault(),
            });

            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.LookupTypeID);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.LookupTypeID);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.LookupTypeID);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.LookupTypeName);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.LookupTypeName);
            }
            //if (model.Sort == 6)
            //{
            //    data = data.OrderBy(e => e.EnterUser.FullName);
            //}
            //if (model.Sort == 7)
            //{
            //    data = data.OrderByDescending(e => e.EnterUser.FullName);
            //}
            if (model.Sort == 8)
            {
                data = data.OrderBy(e => e.LookupTypeID);
            }
            if (model.Sort == 9)
            {
                data = data.OrderByDescending(e => e.LookupTypeID);
            }

            //if (model.Sort == 10)
            //{
            //    data = data.OrderBy(e => e.ModUser.FullName);
            //}
            //if (model.Sort == 11)
            //{
            //    data = data.OrderByDescending(e => e.ModUser.FullName);
            //}
            if (model.Sort == 12)
            {
                data = data.OrderBy(e => e.ModDate);
            }
            if (model.Sort == 13)
            {
                data = data.OrderByDescending(e => e.ModDate);
            }
            if (model.Sort == 14)
            {
                data = data.OrderBy(e => e.Status.LookupName);
            }
            if (model.Sort == 15)
            {
                data = data.OrderByDescending(e => e.Status.LookupName);
            }

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetLookups(GetLookupsModel2 model, PaginationModel paginationPostModel)
        {
            if (model.Status == null)
            {
                model.Status = (int)Status.Active;
            }


            var data = _context.MasterSPLookup.Where(e => e.Cancelled == false && e.Status == model.Status &&

            (model.LookupTypeId == null || e.LookupTypeID == model.LookupTypeId) &&
            (model.ParentId == null || e.LookupParent == model.ParentId) &&
              (model.HasParent == null || (model.HasParent == true && e.LookupParent != null) || (model.HasParent == false && e.LookupParent == null)) &&
                (model.LookupStatic == null || e.LookupStatic == model.LookupStatic) &&
                  (model.LookupDefault == null || e.LookupDefault == model.LookupDefault) &&
             (model.Search == null
             || e.LookupName.Contains(model.Search)
             || e.LookupValue.ToString().Contains(model.Search)
             || e.LookupDesc.Contains(model.Search)
             || e.LookupSystem.Contains(model.Search)
             )
            ).Select(e => new
            {
                e.LookupID,
                e.LookupTypeID,
                e.LookupName,
                e.LookupDesc,
                LookupImage = string.IsNullOrEmpty(e.LookupImage) ? null : (_config["Settings:BaseUrl"] + e.LookupImage.Replace(",", "," + _config["Settings:BaseUrl"])).Split(",", StringSplitOptions.None),
                e.LookupParent,
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                LookupParentObj = _context.MasterSPLookup.Where(x => x.LookupID == e.LookupParent).Select(x => new {
                    x.LookupID,
                    x.LookupName,
                    x.LookupBGColor,
                    x.LookupTextColor,
                    x.LookupTypeID,
                    LookupType = _context.MasterSPLookupType.Where(y => y.LookupTypeID == x.LookupTypeID).Select(x => new { x.LookupTypeID, x.LookupTypeName }).FirstOrDefault(),

                }).FirstOrDefault(),
                LookupType = _context.MasterSPLookupType.Where(x => x.LookupTypeID == e.LookupTypeID).Select(x => new { x.LookupTypeID, x.LookupTypeName }).FirstOrDefault(),
                e.LookupValue,
                e.LookupSystem,
                e.LookupStatic,
                e.LookupDefault,
                e.LookupAction,
                e.LookupIntegration,
                e.LookupComments,
                e.LookupTextColor,
                e.LookupBGColor,
                e.Cancelled,
                e.CancelDate,

                e.EnterDate,
                e.EnterTime,

                e.ModDate,
                e.ModTime,
                EnterUser = _context.Users.Where(x => x.UserId == e.EnterUser).Select(x => new { x.UserId, x.FullName }).FirstOrDefault(),
                ModUser = _context.Users.Where(x => x.UserId == e.ModUser).Select(x => new { x.UserId, x.FullName }).FirstOrDefault(),
            });
            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.LookupID);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.LookupName);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.LookupName);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.LookupTypeID);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.LookupTypeID);
            }
            if (model.Sort == 6)
            {
                data = data.OrderBy(e => e.LookupValue);
            }
            if (model.Sort == 7)
            {
                data = data.OrderByDescending(e => e.LookupValue);
            }
            if (model.Sort == 8)
            {
                data = data.OrderBy(e => e.LookupSystem);
            }
            if (model.Sort == 9)
            {
                data = data.OrderByDescending(e => e.LookupSystem);
            }
            if (model.Sort == 10)
            {
                data = data.OrderBy(e => e.LookupStatic);
            }
            if (model.Sort == 11)
            {
                data = data.OrderByDescending(e => e.LookupStatic);
            }
            if (model.Sort == 12)
            {
                data = data.OrderBy(e => e.Status.LookupName);
            }
            if (model.Sort == 13)
            {
                data = data.OrderByDescending(e => e.Status.LookupName);
            }
            if (model.Sort == null)
            {
                data = data.OrderByDescending(e => e.LookupID);
            }


            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }
    }
}
