using MarkaziaPOS.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using SparePartsModule.Infrastructure.ViewModels.Models.Users;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using SparePartsModule.API;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Interface.Users;

namespace SparePartsModule.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //[ApiExplorerSettings(IgnoreApi = true)]
    //public class UserController : ControllerBase
    //{
    //    private readonly IUserService _userService;
    //    private readonly ILogger<UserController>? _logger;

       
    //    public UserController(IUserService userManager, ILogger<UserController>? logger)
    //    {
    //        _userService = userManager ?? throw new ArgumentNullException(nameof(userManager));
    //        _logger = logger;
    //    }

   
      
    //    [HttpGet("GetUsers")]
    //    public async ValueTask<ApiResponseModel> GetUsers([FromQuery] GetUsersModel model, [FromQuery] PaginationModel paginationPostModel)
    //    {
    //        var userId = User.GetUserId();
    //        if (userId < 1)
    //        {
    //            return ApiResponseFactory.CreateErrorResponse("000001");

    //        }
    //        var response = await _userService.GetUsers(model,paginationPostModel);
    //        if (response != null)
    //        {
    //            return ApiResponseFactory.CreateSuccessResponse(response.Data, 0, response.TotalPageCount, response.TotalRecordCount);

    //        }
    //        return ApiResponseFactory.CreateBadRequestResponse("1635");
    //    }
    
    //    [HttpGet("GetUSERDetails")]
    //    public async ValueTask<ApiResponseModel> GetUSERDetails([FromQuery] int userId)
    //    {
    //        var userId1 = User.GetUserId();
    //        if (userId1 < 1)
    //        {
    //            return ApiResponseFactory.CreateErrorResponse("000001");

    //        }
    //        var response = await _userService.GetUSERDetails(userId);
    //        if (response != null)
    //        {
    //            return ApiResponseFactory.CreateSuccessResponse(response, 0);

    //        }
    //        return ApiResponseFactory.CreateBadRequestResponse("1635");
    //    }
    //    [HttpGet("GetUserPermissions")]
    //    public async ValueTask<ApiResponseModel> GetUserPermissions([FromQuery] int userId, [FromQuery]int? portalId)
    //    {
    //        var userId1 = User.GetUserId();
    //        if (userId1 < 1)
    //        {
    //            return ApiResponseFactory.CreateErrorResponse("000001");

    //        }
    //        var response = await _userService.GetUserPermissions(userId,portalId);
    //        if (response != null)
    //        {
    //            return ApiResponseFactory.CreateSuccessResponse(response, 0);

    //        }
    //        return ApiResponseFactory.CreateBadRequestResponse("1635");
    //    }
       
    //    [HttpGet("GetUserMenu")]
    //    public async ValueTask<ApiResponseModel> GetUserMenu( int portalId)
    //    {
    //        var userId = User.GetUserId();
    //        if (userId < 1)
    //        {
    //            return ApiResponseFactory.CreateErrorResponse("000001");

    //        }
    //        string getway = string.Empty;
    //        try
    //        {


    //            string ip = Response.HttpContext.Connection.RemoteIpAddress.ToString();

    //            getway += ip;
    //            if (ip == "::1")
    //            {
    //                getway = getway.Replace("::1", "");

    //                var addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToList();
    //                foreach (var address in addresses)
    //                {
    //                    getway += "," + address.ToString();
    //                }

    //            }
    //        }
    //        catch (Exception)
    //        {

    //        }
    //        var response = await _userService.GetUserMenu(userId, getway,portalId);
    //        if (response != null)
    //        {
    //            return ApiResponseFactory.CreateSuccessResponse(response, 0);

    //        }
    //        return ApiResponseFactory.CreateBadRequestResponse("1635");
    //    }
    //    [HttpGet("GetUserMenuNew")]
    //    public async ValueTask<ApiResponseModel> GetUserMenuNew( int portalId)
    //    {
    //        var userId = User.GetUserId();
    //        if (userId < 1)
    //        {
    //            return ApiResponseFactory.CreateErrorResponse("000001");

    //        }
    //        string getway = string.Empty;
    //        try
    //        {


    //            string ip = Response.HttpContext.Connection.RemoteIpAddress.ToString();

    //            getway += ip;
    //            if (ip == "::1")
    //            {
    //                getway = getway.Replace("::1", "");

    //                var addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToList();
    //                foreach (var address in addresses)
    //                {
    //                    getway += "," + address.ToString();
    //                }

    //            }
    //        }
    //        catch (Exception)
    //        {

    //        }
    //        var response = await _userService.GetUserMenuNew(userId, getway,  portalId);
    //        if (response != null)
    //        {
    //            var res = ApiResponseFactory.CreateSuccessResponse(response, 0);
    //            res.Info = new { yourIp = getway };
    //            return res;

    //        }
    //        return ApiResponseFactory.CreateBadRequestResponse("1635");
    //    }
    //    [HttpGet("GetUserPortals")]
    //    public async ValueTask<ApiResponseModel> GetUserPortals()
    //    {
    //        var userId1 = User.GetUserId();
    //        if (userId1 < 1)
    //        {
    //            return ApiResponseFactory.CreateErrorResponse("000001");

    //        }
    //        var response = await _userService.GetUserPortals(userId1);
    //        if (response != null)
    //        {
    //            return ApiResponseFactory.CreateSuccessResponse(response, 0);

    //        }
    //        return ApiResponseFactory.CreateBadRequestResponse("1635");
    //    }

    //}
}
