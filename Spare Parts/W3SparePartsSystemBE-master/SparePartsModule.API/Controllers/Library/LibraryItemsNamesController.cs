using MarkaziaPOS.API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.LibraryTariff;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Items;
using System.Text.Json;
using SparePartsModule.Infrastructure.ViewModels.Models;

namespace SparePartsModule.API.Controllers.Library
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryItemsNamesController : ControllerBase
    {
        private readonly ILibraryItemsNamesService _service;
        private readonly ILogger<LibraryItemsNamesController>? _logger;

        public LibraryItemsNamesController(ILibraryItemsNamesService service, ILogger<LibraryItemsNamesController>? logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger;
        }

        [HttpPost("AddItemName")]
        public async ValueTask<ApiResponseModel> AddItemName([FromForm] AddItemNameModel model)
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
            var response = await _service.AddItemName(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("EditItemName")]
        public async ValueTask<ApiResponseModel> EditItemName([FromForm] EditItemNameModel model)
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
            var response = await _service.EditItemName(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("DeleteItemName")]
        public async ValueTask<ApiResponseModel> DeleteItemName([FromForm] string ItemNameID)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.DeleteItemName(ItemNameID, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(ItemNameID));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("ItemNameChangeStatus")]
        public async ValueTask<ApiResponseModel> ItemNameChangeStatus([FromForm] string ItemNameID, [FromForm] int StatusId)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.ItemNameChangeStatus(ItemNameID, StatusId, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(ItemNameID));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpGet("GetItemNames")]
        public async ValueTask<ApiResponseModel> GetItemNames([FromQuery] GetItemNameModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetItemNames(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpGet("GetENItemNames")]
        public async ValueTask<ApiResponseModel> GetENItemNames([FromQuery] string? search, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetENItemNames(search,paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpPost("UploadItemName")]
        public async ValueTask<ApiResponseModel> UploadItemName([FromForm] UpdateFileModel2 model)
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
            var response = await _service.UploadItemName(model, userId);
            if (response != null)
            {
                return response;// ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));

            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
    }
}
