using MarkaziaPOS.API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Category;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using System.Text.Json;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.ItemsBundels;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Items.Substitute;
using SparePartsModule.Infrastructure.ViewModels.Models;
using System.ComponentModel.DataAnnotations;

namespace SparePartsModule.API.Controllers.Library
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsBundelsController : ControllerBase
    {
        private readonly IItemsBundelsService _service;
        private readonly ILogger<ItemsBundelsController>? _logger;

        public ItemsBundelsController(IItemsBundelsService service, ILogger<ItemsBundelsController>? logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger;
        }
        [HttpPost("AddBundle")]

        public async ValueTask<ApiResponseModel> AddBundle([FromForm] AddBundleModel model)
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
            var response = await _service.AddBundle(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpGet("GetBundles")]
        public async ValueTask<ApiResponseModel> GetBundles([FromQuery] GetBundlesModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetBundles(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpPost("DeleteBundle")]

        public async ValueTask<ApiResponseModel> DeleteBundle([FromForm][Required] string  BundleID)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.DeleteBundle(BundleID, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(BundleID));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("BundlesChangeStatus")]

        public async ValueTask<ApiResponseModel> BundlesChangeStatus([FromForm][Required] string BundleID, [FromForm][Required] int StatusId)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.BundlesChangeStatus(BundleID, StatusId, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(BundleID));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("AddBundleItems")]

        public async ValueTask<ApiResponseModel> AddBundleItems(AddBundleItemsModel model)
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
            var response = await _service.AddBundleItems(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("DeleteBundleItem")]

        public async ValueTask<ApiResponseModel> DeleteBundleItem([FromForm][Required] int BundleLineID)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.DeleteBundleItem(BundleLineID, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(BundleLineID));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpGet("GetBundlesItems")]
        public async ValueTask<ApiResponseModel> GetBundlesItems([FromQuery] GetBundlesItemsModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetBundlesItems(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpPost("EditBundle")]

        public async ValueTask<ApiResponseModel> EditBundle(EditBundleModel model)
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
            var response = await _service.EditBundle(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("SendBundleForApproval")]

        public async ValueTask<ApiResponseModel> SendBundleForApproval([FromForm][Required] int BundleID)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.SendBundleForApproval(BundleID, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(BundleID));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("HandleBundleApprovalRequest")]

        public async ValueTask<ApiResponseModel> HandleBundleApprovalRequest([FromForm] HandleBundleApprovalRequestModel model)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.HandleBundleApprovalRequest(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("AddBundleComplete")]

        public async ValueTask<ApiResponseModel> AddBundleComplete(AddBundleModelComplete model)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.AddBundleComplete(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("ImportBundle")]

        public async ValueTask<ApiResponseModel> ImportBundle([FromForm]UpdateFileModel2 model)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.ImportBundle(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }


    }
}
