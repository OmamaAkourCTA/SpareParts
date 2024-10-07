using MarkaziaPOS.API.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Suppliers;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using System.Text.Json;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Category;
using System.ComponentModel.DataAnnotations;
using SparePartsModule.Infrastructure.ViewModels.Models;

namespace SparePartsModule.API.Controllers.Library
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryItemCategoryController : ControllerBase
    {
        private readonly ILibraryItemCategoryService _service;
        private readonly ILogger<LibraryItemCategoryController>? _logger;
        /// <summary>
        /// LibraryItem Category
        /// </summary>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public LibraryItemCategoryController(ILibraryItemCategoryService service, ILogger<LibraryItemCategoryController>? logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger;
        }
        ///// <summary>
        ///// Datefomrat yyyy-MM-dd,
        ///// ItemCategoryType lookup type id 6
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        [HttpPost("AddItemCategory")]
        [Produces("application/json")]
        public async ValueTask<ApiResponseModel> AddItemCategory([FromForm] AddItemCategoryModel model)
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
            var response = await _service.AddItemCategory(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        /// <summary>
        /// Datefomrat yyyy-MM-dd,
        /// ItemCategoryType lookup type id 6
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("EditItemCategory")]
        public async ValueTask<ApiResponseModel> EditItemCategory([FromForm] EditItemCategoryModel model)
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
            var response = await _service.EditItemCategory(model, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(model));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="ItemCategoryID">Category id</param>
        ///// <returns>success</returns>
        [HttpPost("DeleteItemCategory")]
        public async ValueTask<ApiResponseModel> DeleteItemCategory([FromForm][Required] string ItemCategoryID)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");
            }
            var response = await _service.DeleteItemCategory(ItemCategoryID, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(ItemCategoryID));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        [HttpPost("ItemCategoryChangeStatus")]
        public async ValueTask<ApiResponseModel> ItemCategoryChangeStatus([FromForm][Required] string ItemCategoryID, [FromForm][Required] int StatusId)
        {

            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");
            }
            var response = await _service.ItemCategoryChangeStatus(ItemCategoryID, StatusId, userId);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(null);
            }
            _logger?.LogError("Error while update teacher => ", JsonSerializer.Serialize(ItemCategoryID));
            return ApiResponseFactory.CreateErrorResponse("000002");
        }
        /// <summary>
        /// ItemCategoryType lookup type id 6
        /// </summary>
        /// <param name="model">ItemCategoryType lookup type id 6</param>
        /// <param name="paginationPostModel"></param>
        /// <returns></returns>
        [HttpGet("GetItemCategories")]
        public async ValueTask<ApiResponseModel> GetItemCategories([FromQuery] GetItemCategoriesModel model, [FromQuery] PaginationModel paginationPostModel)
        {
            var userId = User.GetUserId();
            if (userId < 1)
            {
                return ApiResponseFactory.CreateErrorResponse("000001");
            }
            var response = await _service.GetItemCategories(model, paginationPostModel);
            if (response != null)
            {
                return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount, response.TotalAllRecordCount);

            }
            return ApiResponseFactory.CreateBadRequestResponse("1635");
        }
    }
}
