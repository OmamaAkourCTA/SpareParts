using MarkaziaPOS.API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePartsModule.Domain.Context;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.LibraryTariff;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Location;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Suppliers;
using System.Text.Json;

namespace SparePartsModule.API.Controllers.Library
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryLocationsController : ControllerBase
    {
        private readonly ILibraryLocationsService _service;
        private readonly ILogger<LibraryLocationsController>? _logger;

        public LibraryLocationsController(ILibraryLocationsService service, ILogger<LibraryLocationsController>? logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger;
        }
        [HttpPost("AddLocations")]
        public async ValueTask<ApiResponseModel> AddLocations([FromForm] AddLocationsModel model)
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
            var response = await _service.AddLocations(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("EditLocations")]
        public async ValueTask<ApiResponseModel> EditLocations([FromForm] EditLocationsModel model)
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
            var response = await _service.EditLocations(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("DeleteLocations")]
        public async ValueTask<ApiResponseModel> DeleteLocations([FromForm] string LocationId )
        {
         
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.DeleteLocations(LocationId, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(LocationId));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("LocationsChangeStatus")]
        public async ValueTask<ApiResponseModel> LocationsChangeStatus([FromForm] string LocationId, [FromForm] int StatusId)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.LocationsChangeStatus(LocationId, StatusId, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(LocationId));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpGet("GetLocations")]
        public async ValueTask<ApiResponseModel> GetLocations([FromQuery] GetLocationsModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetLocations(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        /// <summary>
        /// WareHouseTemplate
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ImportLocations")]
        public async ValueTask<ApiResponseModel> ImportLocations([FromForm] UpdateFileModel2 model)
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
            var response = await _service.ImportLocations(model, userId);
            if (response != null)
            {
                return response;// ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpGet("GetLocationsV2")]
        public async ValueTask<ApiResponseModel> GetLocationsV2([FromQuery] GetLocationsModelV2 model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetLocationsV2(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpPost("AddLocationsV2")]
        public async ValueTask<ApiResponseModel> AddLocationsV2([FromForm] AddLocationsModelV2 model)
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
            var response = await _service.AddLocationsV2(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("EditLocationsV2")]
        public async ValueTask<ApiResponseModel> EditLocationsV2([FromForm] EditLocationsModelV2 model)
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
            var response = await _service.EditLocationsV2 (model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
    }
}
