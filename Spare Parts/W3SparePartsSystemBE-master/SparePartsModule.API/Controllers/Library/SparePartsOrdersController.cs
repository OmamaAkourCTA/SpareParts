using MarkaziaPOS.API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.LibraryTariff;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using System.Text.Json;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.SPOrder;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.ItemsBundels;
using SparePartsModule.Infrastructure.ViewModels.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.ComponentModel.DataAnnotations;

namespace SparePartsModule.API.Controllers.Library
{
    [Route("api/[controller]")]
    [ApiController]
    public class SparePartsOrdersController : ControllerBase
    {
        private readonly ISparePartsOrdersService _service;
        private readonly ILogger<SparePartsOrdersController>? _logger;

        public SparePartsOrdersController(ISparePartsOrdersService service, ILogger<SparePartsOrdersController>? logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger;
        }




        /// <summary>
        /// OrderType:lookup type id 27
        /// OrderMethod:lookup type id 28
        /// OrderFrieght:lookup type id 29
        /// Api: getsuppliers
        /// supplier flag lookup type id 18
        /// OrderFlag lookup type id 19
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("CreateSPOrder")]
        public async ValueTask<ApiResponseModel> CreateSPOrder( CreateSPOrderModel model)
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
            var response = await _service.CreateSPOrder(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpGet("GetSPOrders")]
        public async ValueTask<ApiResponseModel> GetBundles([FromQuery] GetSPOrdersModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetSPOrders(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }

        [HttpGet("GetInquiryOrders")]
        public async ValueTask<ApiResponseModel> GetInquiryOrders([FromQuery] GetSPInquiryOrdersModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetInquiryOrders(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }






        [HttpGet("GetSPOrdersDetails")]
        public async ValueTask<ApiResponseModel> GetSPOrdersDetails([FromQuery] int OrderId)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetSPOrdersDetails( OrderId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        /// <summary>
        /// template Orders-ImportItems
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UploadOrderItems")]
        public async ValueTask<ApiResponseModel> UploadOrderItems([FromForm]UploadOrderItemsModel model)
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
            var response = await _service.UploadOrderItems(model, userId);
            if (response != null)
            {
                return response;
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        /// <summary>
        /// OrderType:lookup type id 27
        /// OrderMethod:lookup type id 28
        /// OrderFrieght:lookup type id 29
        /// Api: getsuppliers
        /// supplier flag lookup type id 18
        /// OrderFlag lookup type id 19
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("EditSPOrder")]
        public async ValueTask<ApiResponseModel> EditSPOrder(EditSPOrderModel model)
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
            var response = await _service.EditSPOrder(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("DeleteSPOrder")]
        public async ValueTask<ApiResponseModel> DeleteSPOrder([FromForm][Required]string OrderId )
        {
           
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.DeleteSPOrder(OrderId, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(OrderId));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpGet("GetFlagsSummery")]
        public async ValueTask<ApiResponseModel> GetFlagsSummery([FromQuery] int OrderId)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.GetFlagsSummery(OrderId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
        [HttpPost("DeleteOrderItem")]
        public async ValueTask<ApiResponseModel> DeleteOrderItem([FromForm][Required] string OrderLineID)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.DeleteOrderItem(OrderLineID, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(OrderLineID));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("EditOrderItem")]
        public async ValueTask<ApiResponseModel> EditOrderItem([FromForm][Required] int OrderLineID, [FromForm][Required] int Qty)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.EditOrderItem(OrderLineID, Qty, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(OrderLineID));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("SkipPosss")]
        public async ValueTask<ApiResponseModel> SkipPosss([FromForm][Required] int OrderId)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.SkipPosss(OrderId, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(OrderId));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("WithdrawOrder")]
        public async ValueTask<ApiResponseModel> WithdrawOrder([FromForm][Required] int OrderId)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.WithdrawOrder(OrderId, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(OrderId));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("ConfirmOrder")]
        public async ValueTask<ApiResponseModel> ConfirmOrder( ConfirmOrderModel OrderId)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");

            }
            var response = await _service.ConfirmOrder(OrderId, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(OrderId));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
    }
}
