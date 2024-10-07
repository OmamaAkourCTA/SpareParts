using MarkaziaPOS.API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.SupplierDelivery;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Suppliers;
using SparePartsModule.Interface.Library;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SparePartsModule.API.Controllers.Library
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrarySuppliersController : ControllerBase
    {
        private readonly ILibrarySuppliersService _service;
        private readonly ILogger<LibrarySuppliersController>? _logger;

        public LibrarySuppliersController(ILibrarySuppliersService service, ILogger<LibrarySuppliersController>? logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger;
        }
        ///// <summary>
        ///// Date format yyyy-MM-dd,
        ///// OriginalCountryID lookup type id 2,
        ///// SupplierCurrencyID lookup type id 3,
        ///// PaymentMethod lookup type id 4,
        ///// DeliveryMethodID lookup type id 5,
        ///// </summary>

        ///// <param name="userId"></param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentNullException"></exception>
        ///// <exception cref="ManagerProcessException"></exception>
        [HttpPost("AddSupplier")]
        public async ValueTask<ApiResponseModel> AddSupplier([FromForm] AddSupplierModel model)
        {
            if (model == null)
            {
                return ApiResponseFactory.CreateBadRequestResponse("000005");
            }
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.AddSupplier(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        ///// <summary>
        ///// Date format yyyy-MM-dd,
        ///// OriginalCountryID lookup type id 2,
        ///// SupplierCurrencyID lookup type id 3,
        ///// PaymentMethod lookup type id 4,
        ///// DeliveryMethodID lookup type id 5,
        ///// </summary>

        ///// <param name="userId"></param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentNullException"></exception>
        ///// <exception cref="ManagerProcessException"></exception>
        [HttpPost("EditSupplier")]
        public async ValueTask<ApiResponseModel> EditSupplier([FromForm] EditSupplierModel model)
        {
            if (model == null)
            {
                return ApiResponseFactory.CreateBadRequestResponse("000005");
            }
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.EditSupplier(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }

        [HttpPost("DeleteSupplier")]
        public async ValueTask<ApiResponseModel> DeleteSupplier([FromForm][Required] string SupplierId)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.DeleteSuppliers(SupplierId, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(SupplierId));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("SuppliersChangeStatus")]
        public async ValueTask<ApiResponseModel> SuppliersChangeStatus([FromForm][Required] string SupplierIds, [FromForm][Required] int StatusId)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.SuppliersChangeStatus(SupplierIds, StatusId, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(SupplierIds));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }

        [HttpGet("GetSuppliers")]
        public async ValueTask<ApiResponseModel> GetSuppliers([FromQuery] GetSupplierModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetSuppliers(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpGet("GetSupplierMasterItems")]
        public async ValueTask<ApiResponseModel> GetSupplierMasterItems([FromQuery] GetSupplierMasterItemsModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetSupplierMasterItems(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpGet("GetSupplierMasterItemsV2")]
        public async ValueTask<ApiResponseModel> GetSupplierMasterItemsV2([FromQuery] GetSupplierMasterItemsModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetSupplierMasterItemsV2(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }


        [HttpPost("AddSupplierDeliveryMethod")]
        public async ValueTask<ApiResponseModel> AddSupplierDeliveryMethod([FromForm] AddSupplierDeliveryModel model)
        {
            if (model == null)
            {
                return ApiResponseFactory.CreateBadRequestResponse("000005");
            }
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.AddSupplierDeliveryMethod(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while Adding Supplier Delivery => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }

        [HttpPost("EditSupplierDeliveryMethod")]
        public async ValueTask<ApiResponseModel> EditSupplierDeliveryMethod([FromForm] EditSupplierDeliveryModel model)
        {
            if (model == null)
            {
                return ApiResponseFactory.CreateBadRequestResponse("000005");
            }
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.EditSupplierDeliveryMethod(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while Adding Supplier Delivery => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("DeleteSupplierDeliveryMethod")]
        public async ValueTask<ApiResponseModel> DeleteSupplierDeliveryMethod([FromForm][Required] int SupplierDeliveryMethodID)
        {
           
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.DeleteSupplierDeliveryMethod(SupplierDeliveryMethodID, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while Adding Supplier Delivery => ", JsonSerializer.Serialize(SupplierDeliveryMethodID));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }

        [HttpGet("GetSupplierDeliveryMethod")]
        public async ValueTask<ApiResponseModel> GetSupplierDeliveryMethod([FromQuery][Required] int SupplierId)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetSupplierDeliveryMethod(SupplierId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response) ;

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpPost("ImportSupplier")]
        public async ValueTask<ApiResponseModel> ImportSupplier([FromForm] UpdateFileModel2 model)
        {
            if (model == null)
            {
                return ApiResponseFactory.CreateBadRequestResponse("000005");
            }
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.ImportSupplier(model, userId);
            if (response != null)
            {
                return response;//ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while Adding Supplier Delivery => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }



        //[HttpGet("GetSupplierMasterItems1")]
        //public async ValueTask<ApiResponseModel> GetSupplierMasterItem1([FromQuery][Required] int SupplierId, [FromQuery] PaginationModel paginationPostModel)
        //{
        //    var userId = User.GetUserId();
        //    if (userId < 1)
        //    {
        //        return ApiResponseFactory.CreateErrorResponse("000001");
        //    }
        //   var response = await _service.GetSupplierDeliveryMethod1(SupplierId, paginationPostModel);
        //    if (response != null)
        //    {
        //        return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);
        //    }
        //    return ApiResponseFactory.CreateBadRequestResponse("1635");
        //}



    }
}
