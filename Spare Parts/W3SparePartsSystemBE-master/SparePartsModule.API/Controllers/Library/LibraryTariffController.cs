using MarkaziaPOS.API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Suppliers;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.LibraryTariff;

namespace SparePartsModule.API.Controllers.Library
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryTariffController : ControllerBase
    {
        private readonly ILibraryTariffService _service;
        private readonly ILogger<LibraryTariffController>? _logger;

        public LibraryTariffController(ILibraryTariffService service, ILogger<LibraryTariffController>? logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger;
        }

        [HttpPost("AddTariff")]
        public async ValueTask<ApiResponseModel> AddTariff([FromForm] AddLibraryTariffModel model)
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
            var response = await _service.AddLibraryTariff(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }

        [HttpPost("EditTariff")]
        public async ValueTask<ApiResponseModel> EditTariff([FromForm] EditLibraryTariffModel model)
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
            var response = await _service.EditLibraryTariff(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("DeleteTariff")]
        public async ValueTask<ApiResponseModel> DeleteTariff([FromForm][Required] string TariffId)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.DeleteLibraryTariff(TariffId, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(TariffId));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("ItemNameChangeStatus")]
        public async ValueTask<ApiResponseModel> ItemNameChangeStatus([FromForm][Required] string TariffId, [FromForm][Required] int StatusId)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.ItemNameChangeStatus(TariffId, StatusId, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(TariffId));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }

        [HttpGet("GetTariff")]
        public async ValueTask<ApiResponseModel> GetTariff([FromQuery] GetLibraryTariffModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetLibraryTarifs(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
    }
}
