using SparePartsModule.Infrastructure.ViewModels.Models.Library.SPOrder;
using SparePartsModule.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Models;

namespace SparePartsModule.Interface.Library
{
    public interface ISparePartsOrdersService
    {
        ValueTask<ApiResponseModel> CreateSPOrder(CreateSPOrderModel model, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetSPOrders(GetSPOrdersModel model, PaginationModel paginationPostModel);
        ValueTask<PaginationDatabaseResponseDto<object>> GetInquiryOrders(GetSPInquiryOrdersModel model, PaginationModel paginationPostModel);
        ValueTask<object> GetSPOrdersDetails(int OrderId);
        ValueTask<ApiResponseModel> UploadOrderItems(UploadOrderItemsModel model, int userId);
        ValueTask<ApiResponseModel> EditSPOrder(EditSPOrderModel model, int userId);
        ValueTask<ApiResponseModel> DeleteSPOrder(string OrderId, int userId);
        ValueTask<object> GetFlagsSummery(int OrderId);
        ValueTask<ApiResponseModel> DeleteOrderItem(string OrderLineID, int userId);
        ValueTask<ApiResponseModel> EditOrderItem(int OrderLineID, int Qty, int userId);
        ValueTask<ApiResponseModel> SkipPosss(int OrderId, int userId);
        ValueTask<ApiResponseModel> WithdrawOrder(int OrderId, int userId);
        ValueTask<ApiResponseModel> ConfirmOrder(ConfirmOrderModel model, int userId);


    }
}
