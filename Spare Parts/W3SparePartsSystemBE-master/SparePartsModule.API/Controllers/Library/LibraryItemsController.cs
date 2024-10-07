using MarkaziaPOS.API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Category;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using System.Text.Json;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Items;
using DocumentFormat.OpenXml.Spreadsheet;
using SparePartsModule.Infrastructure.ViewModels.Models.Lookups;
using SparePartsModule.Infrastructure.ViewModels.Models;
using System.ComponentModel.DataAnnotations;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Items.Substitute;

namespace SparePartsModule.API.Controllers.Library
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryItemsController : ControllerBase
    {
        private readonly ILibraryItemsService _service;
        private readonly ILogger<LibraryItemsController>? _logger;

        public LibraryItemsController(ILibraryItemsService service, ILogger<LibraryItemsController>? logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger;
        }

        [HttpPost("UploadSupplierMasterItems")]

        public async ValueTask<ApiResponseModel> UploadSupplierMasterItems([FromForm] UploadSupplierMasterItemsModle model)
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
            var response = await _service.UploadSupplierMasterItems(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("UploadSupplierMasterItemsV2")]

        public async ValueTask<ApiResponseModel> UploadSupplierMasterItemsV2([FromForm] UpdateFileModel2 model)
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
            var response = await _service.UploadSupplierMasterItemsV2(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("AddItem")]

        public async ValueTask<ApiResponseModel> AddItem([FromForm] AddItemModel model)
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
            var response = await _service.AddItem(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("EditItem")]

        public async ValueTask<ApiResponseModel> EditItem([FromForm] EditItemModel model)
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
            var response = await _service.EditItem(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("DeleteItem")]

        public async ValueTask<ApiResponseModel> DeleteItem([FromForm] string ItemId)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.DeleteItem(ItemId, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(ItemId));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("ItemChangeStatus")]

        public async ValueTask<ApiResponseModel> ItemChangeStatus([FromForm] string ItemId, [FromForm] int StatusId)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.ItemChangeStatus(ItemId, StatusId, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(ItemId));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpGet("GetItems")]
        public async ValueTask<ApiResponseModel> GetItems([FromQuery] GetItemsModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetItems(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpPost("AddItemSupplier")]

        public async ValueTask<ApiResponseModel> AddItemSupplier([FromForm] AddItemSupplierModel model)
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
            var response = await _service.AddItemSupplier(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("EditItemSupplier")]

        public async ValueTask<ApiResponseModel> EditItemSupplier([FromForm] EditItemSupplierModel model)
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
            var response = await _service.EditItemSupplier(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("DeleteItemSupplier")]

        public async ValueTask<ApiResponseModel> DeleteItemSupplier([FromForm] DeleteItemSupplierModel model)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.DeleteItemSupplier(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpGet("GetItemSupplier")]
        public async ValueTask<ApiResponseModel> GetItemSupplier([FromQuery] GetItemSupplierModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetItemSupplier(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpPost("AddItemLocation")]

        public async ValueTask<ApiResponseModel> AddItemLocation([FromForm] AddItemLocationModel model)
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
            var response = await _service.AddItemLocation(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("EditItemLocation")]

        public async ValueTask<ApiResponseModel> EditItemLocation([FromForm] EditItemLocationModel model)
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
            var response = await _service.EditItemLocation(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("DeleteItemLocation")]

        public async ValueTask<ApiResponseModel> DeleteItemLocation([FromForm][Required] int ItemLocationID)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.DeleteItemLocation(ItemLocationID, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(ItemLocationID));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpGet("GetItemLocations")]
        public async ValueTask<ApiResponseModel> GetItemLocations([FromQuery] GetItemLocationsModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetItemLocations(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpPost("AddItemVehicleModelCode")]

        public async ValueTask<ApiResponseModel> AddItemVehicleModelCode([FromForm] AddItemVehicleModelCodeModel model)
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
            var response = await _service.AddItemVehicleModelCode(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("EditItemVehicleModelCode")]

        public async ValueTask<ApiResponseModel> EditItemVehicleModelCode([FromForm] EditItemVehicleModelCodeModel model)
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
            var response = await _service.EditItemVehicleModelCode(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("DeleteItemVehicleModelCode")]

        public async ValueTask<ApiResponseModel> DeleteItemVehicleModelCode([FromForm] int ItemModelCodeID)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.DeleteItemVehicleModelCode(ItemModelCodeID, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(ItemModelCodeID));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpGet("GetItemVehicleModelCode")]
        public async ValueTask<ApiResponseModel> GetItemVehicleModelCode([FromQuery] GetItemVehicleModelCodeModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetItemVehicleModelCode(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpPost("AddItemSubsitute")]

        public async ValueTask<ApiResponseModel> AddItemSubsitute(AddItemSubsituteModel model)
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
            var response = await _service.AddItemSubsitute(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpGet("GetItemSubstitutes")]
        public async ValueTask<ApiResponseModel> GetItemSubstitutes([FromQuery] GetItemSubstitutesModel model)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetItemSubstitutes(model);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpPost("DeleteItemSubstitute")]

        public async ValueTask<ApiResponseModel> DeleteItemSubstitute([FromForm] int ItemSubstituteID)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.DeleteItemSubstitute(ItemSubstituteID, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(ItemSubstituteID));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("DeactivateItemSubstitute")]
        public async ValueTask<ApiResponseModel> DeactivateItemSubstitute([FromForm] DeactivateItemSubstituteModel model)
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
            var response = await _service.DeactivateItemSubstitute(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpGet("GetSupplierMasterItemDetails")]
        public async ValueTask<ApiResponseModel> GetSupplierMasterItemDetails([FromQuery] string ItemSupNo)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetSupplierMasterItemDetails(ItemSupNo);
            return ApiResponseFactory.CreateSuccessResponse(response);
            //if (response != null)
            //{
            //    return ApiResponseFactory.CreateSuccessResponse(response);

            //}
            //return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpGet("GetSupplierMasterItemDetailsV2")]
        public async ValueTask<ApiResponseModel> GetSupplierMasterItemDetailsV2([FromQuery] string ItemSupNo, [FromQuery] int? supplierId)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetSupplierMasterItemDetailsV2(ItemSupNo, supplierId);
            return ApiResponseFactory.CreateSuccessResponse(response);
            //if (response != null)
            //{
            //    return ApiResponseFactory.CreateSuccessResponse(response);

            //}
            //return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpPost("UploadCTAItems")]

        public async ValueTask<ApiResponseModel> UploadCTAItems([FromForm] UploadSupplierMasterItemsModle2 model)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.UploadCTAItems(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            // _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(File));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpGet("GetCTAItems")]
        public async ValueTask<ApiResponseModel> GetCTAItems([FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetCTAItems(paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpPost("UploadItemsLocations")]

        public async ValueTask<ApiResponseModel> UploadItemsLocations([FromForm] UploadSupplierMasterItemsModle2 model)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.UploadItemsLocations(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            // _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(File));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("UploadItemsSuppliers")]

        public async ValueTask<ApiResponseModel> UploadItemsSuppliers([FromForm] UploadSupplierMasterItemsModle2 model)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.UploadItemsSuppliers(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            // _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(File));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("UploadOtherSuppliersJPM")]

        public async ValueTask<ApiResponseModel> UploadOtherSuppliersJPM([FromForm] UpdateFileModel2 model)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.UploadOtherSuppliersJPM(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            // _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(File));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("UploadItemsSubstitutes")]
        public async ValueTask<ApiResponseModel> UploadItemsSubstitutes([FromForm] UploadSupplierMasterItemsModle2 model)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.UploadItemsSubstitutes(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            // _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(File));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }

        [HttpGet("GetItemInquiryCards")]
        public async ValueTask<ApiResponseModel> GetItemInquiryCards([FromQuery] GetItemsModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetItemInquiryCards(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
    }
}
