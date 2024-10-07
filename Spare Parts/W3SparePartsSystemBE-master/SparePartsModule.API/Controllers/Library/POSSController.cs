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
using SparePartsModule.Infrastructure.ViewModels.Models.Library.POSS;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.SPOrder;

namespace SparePartsModule.API.Controllers.Library
{
    [Route("api/[controller]")]
    [ApiController]
    public class POSSController : ControllerBase
    {
        private readonly IPOSSService _service;
        private readonly ILogger<LibraryItemsController>? _logger;

        public POSSController(IPOSSService service, ILogger<LibraryItemsController>? logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger;
        }

        [HttpPost("UploadPOSS")]
        public async ValueTask<ApiResponseModel> UploadPOSS([FromForm] UploadPOSSModle model)
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
            var response = await _service.UploadPOSS(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }

        [HttpGet("GetPOSS")]
        public async ValueTask<ApiResponseModel> GetPOSS([FromQuery] GetPOSSModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetPOSS(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }

        [HttpGet("GetPOSSRawViewDetails")]
        public async ValueTask<ApiResponseModel> GetPOSSRawViewDetails([FromQuery] int POSSID, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetPOSSRawViewDetails(POSSID, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }

        [HttpPost("AddPOSSManually")]
        public async ValueTask<ApiResponseModel> AddPOSSManually( AddPOSSManuallyModle model)
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
            var response = await _service.AddPOSSManually(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }

        [HttpGet("GetPOSSTMCSummeryView")]
        public async ValueTask<ApiResponseModel> GetPOSSTMCSummeryView([FromQuery] int POSSID )
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetPOSSTMCSummeryView(POSSID);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }

        [HttpPost("AddNonTMCPOSS")]
        public async ValueTask<ApiResponseModel> AddNonTMCPOSS(AddNonTMCPOSSModel model)
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
            var response = await _service.AddNonTMCPOSS(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }

        [HttpPost("UpdateNonTMCPOSS")]
        public async ValueTask<ApiResponseModel> UpdateNonTMCPOSS(UpdateNonTMCPOSSModel model)
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
            var response = await _service.UpdateNonTMCPOSS(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }

        [HttpGet("GetOrderPOSSSummeryView")]
        public async ValueTask<ApiResponseModel> GetOrderPOSSSummeryView([FromQuery] int OrderId)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetOrderPOSSSummeryView(OrderId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
    }
}
