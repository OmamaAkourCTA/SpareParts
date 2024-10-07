using MarkaziaPOS.API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Core.Helpers;
using SparePartsModule.Core;
using SparePartsModule.Infrastructure.ViewModels.Models.ListNames;
using System.ComponentModel.DataAnnotations;

namespace SparePartsModule.API.Controllers.Library
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleSystemViewsController : ControllerBase
    {
        private readonly IVehicleSystemViewsService _service;
        private readonly ILogger<VehicleSystemViewsController>? _logger;

        public VehicleSystemViewsController(IVehicleSystemViewsService service, ILogger<VehicleSystemViewsController>? logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger;
        }
        [HttpGet("GetBrands")]
        public async ValueTask<ApiResponseModel> GetBrands([FromQuery] GetBrandModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetBrands(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpGet("GetModels")]
        public async ValueTask<ApiResponseModel> GetModels([FromQuery] GetModelModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetModels(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpGet("GetModelCode")]
        public async ValueTask<ApiResponseModel> GetModelCode([FromQuery] GetModelCodeModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetModelCode(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }

        [HttpGet("GetListName")]
        public async ValueTask<ApiResponseModel> GetListName([FromQuery] GetListNameModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetListName(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }

        [HttpGet("ExportSalesSpecsSheet")]
        public async ValueTask<ApiResponseModel> ExportSalesSpecsSheet([FromQuery][Required] int ListId)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.ExportSalesSpecsSheet(ListId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }

        [HttpGet("GetListNameSpecsValues")]
        public async ValueTask<ApiResponseModel> GetListNameSpecsValues([FromQuery] GetListNameSpecsValuesModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetListNameSpecsValues(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }

    }
}
