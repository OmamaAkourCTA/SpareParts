using SparePartsModule.Infrastructure.ViewModels.Models.Users;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Infrastructure.ViewModels.Models;

namespace SparePartsModule.Interface.Users
{
    public interface IUserService
    {
    

        ValueTask<PaginationDatabaseResponseDto<object>> GetUsers(GetUsersModel model, PaginationModel paginationPostModel);

        ValueTask<object> GetUSERDetails(int userId);
        ValueTask<object> GetUserPermissions(int userId, int? PortalId);
   

        ValueTask<object> GetUserMenu(int userId, string ipAddress, int portalId);
        ValueTask<object> GetUserMenuNew(int userId, string ipAddress, int portalId);
        ValueTask<object> GetUserPortals(int userId);
      

    }
}
