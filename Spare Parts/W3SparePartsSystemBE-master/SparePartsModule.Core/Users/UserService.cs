using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Core.Helpers;
using SparePartsModule.Domain.Context;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Infrastructure.ViewModels.Dtos.Enums;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels.Models.Users;
using SparePartsModule.Interface;
using SparePartsModule.Interface.Users;
using System.Data;

namespace SparePartsModule.Core
{
    public class UserService: IUserService
    {
        private readonly IJWTManagerManager _jwtManagerManager;
        private readonly FileHelper _FileHelper;
        private readonly UtilitiesHelper _utilitiesHelper;

        private readonly IConfiguration _config;
        private readonly DX_AdmimistrationContext _context;

   

        List<string> errors = new List<string>();

        public UserService( DX_AdmimistrationContext context, IJWTManagerManager jwtManagerManager, FileHelper fileHelper, UtilitiesHelper utilitiesHelper, IConfiguration config)
        {
         
            _jwtManagerManager = jwtManagerManager ?? throw new ArgumentNullException(nameof(jwtManagerManager));
            _FileHelper = fileHelper;
            _config = config;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _utilitiesHelper = utilitiesHelper ?? throw new ArgumentNullException(nameof(utilitiesHelper));
  


        }
     
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetUsers(GetUsersModel model,PaginationModel paginationPostModel)
        {
            var StatusId = _context.LookupTranslations.Where(e => e.LookupId >= 2001 && e.LookupId < 3001 && e.LookupName.Contains(model.Search)).Select(e => e.LookupId).ToList();
            List<int> registerUsers = null;
            if (model.RegisterId != null)
            {
                registerUsers = _context.RegisterEmployees.Where(e => e.RegisterId == model.RegisterId).Select(e => e.Userid).ToList();
            }
            var branches = model.BranchId?.Split(',')?.Select(Int32.Parse)?.ToList();
            var roles = model.RoleId?.Split(',')?.Select(Int32.Parse)?.ToList();
            if (model.sort == null)
            {
                model.sort = 1;
            }
            var data = _context.Users.Where(e =>(model.Invitation==null|| e.Invitation==model.Invitation)&&
            (model.Search==null||e.FullName.Contains(model.Search) || e.Mobile.Contains(model.Search) || e.Email.Contains(model.Search)|| StatusId.Contains((int)e.Invitation)) &&
            (model.RoleId==null|| roles.Contains(e.RoleId))&&
            (model.BranchId == null || branches.Contains(e.BranchId))&&
            (model.Status==null||e.Status==model.Status)&&
             (registerUsers == null || registerUsers.Contains(e.UserId))
            )
            .Select(x => new
            {
                x.UserId,x.FullName,x.Email,x.Mobile,x.Outdoor,x.Indoor,x.City,x.CreatedAt,x.LastLogin,x.Status,x.Invitation,
                
                InvitationObj = _context.LookupTranslations.Where(c => c.LookupId == x.Invitation).Select(s => new
                {
                    s.LanguageId,
                    s.LookupName
                }).ToList(),
                ProfileImage = x.ProfileImage == null ? "" : _config["Settings:BaseUrl"] + x.ProfileImage,
                Branch = _context.Branches.Where(e => e.BranchId == x.BranchId).Select(e => new {e.BranchId,e.BranchName}).FirstOrDefault(),
                Role= _context.Roles.Where(e => e.RoleId == x.RoleId).Select(e => new { e.RoleId, e.Name }).FirstOrDefault(),
                WorkingHours = _context.UserWorkingHours.Where(e => e.UserId == x.UserId && e.Status == (int)StatusAdmin.Active).Select(e => new {
                    e.DayId,
                    Day = _context.LookupTranslations.Where(c => c.LookupId == e.DayId).Select(s => new
                    {
                        s.LanguageId,
                        s.LookupName
                    }).ToList(),
                    e.FromTime,
                    e.ToTime,
                    e.CreatedAt,
                }).ToList(),
   

            });
            if (model.sort == 1)
            {
                data = data.OrderBy(e=>e.Invitation).ThenByDescending(e=>e.Status).ThenByDescending(e => e.CreatedAt);
            }
            if (model.sort == 2)
            {
                data = data.OrderBy(e => e.FullName);
            }
            if (model.sort == 3)
            {
                data = data.OrderByDescending(e => e.FullName);
            }
            if (model.sort == 4)
            {
                data = data.OrderBy(e => e.Role.Name);
            }
            if (model.sort == 5)
            {
                data = data.OrderByDescending(e => e.Role.Name);
            }
            if (model.sort == 6)
            {
                data = data.OrderBy(e => e.Branch.BranchName);
            }
            if (model.sort == 7)
            {
                data = data.OrderByDescending(e => e.Branch.BranchName);
            }
            var result= new PaginationDatabaseResponseDto<object>(paginationPostModel, data);
            return result;


        }

        public async ValueTask<object> GetUSERDetails(int userId)
        {

            var data = _context.Users.Where(e => e.UserId == userId)
            .Select(x => new
            {
                x.UserId, x.FullName, x.Mobile, x.Email, x.CreatedAt, x.LastLogin, x.Status,
                Branch = _context.Branches.Where(e => e.BranchId == x.BranchId).Select(e => new { e.BranchName, e.BranchId }).FirstOrDefault(),
                x.City,
                x.ContractStartDate,
                x.ContractEndDate, x.Indoor, x.Outdoor,
                x.Invitation,
                x.MaxAllowedLineDiscount,x.MaxAllowedInvoiceDiscount,x.AllowedChangeDeductionAmount,
                InvitationObj = _context.LookupTranslations.Where(c => c.LookupId == x.Invitation).Select(s => new
                {
                    s.LanguageId,
                    s.LookupName
                }).ToList(),
                Role = _context.Roles.Where(e => e.RoleId == x.RoleId).Select(e => new { e.Name, e.RolePermissions, e.RoleId }).FirstOrDefault(),
                CreatedBy = _context.Users.Where(e => e.UserId == x.CreatedBy).Select(e => new { e.FullName, e.UserId }).FirstOrDefault(),


                UserWorkingHours = _context.UserWorkingHours.Where(e => e.UserId == x.UserId).Select(e => new
                {
                    e.DayId,
                    e.Status,
                    Selected = e.Status == 2001,
                    Day = _context.LookupTranslations.Where(c => c.LookupId == e.DayId).Select(s => new
                    {
                        s.LanguageId,
                        s.LookupName
                    }).ToList(),
                    e.FromTime,
                    e.ToTime,
                    e.CreatedAt,
                }).ToList(),
               
                Register = _context.RegisterEmployees.Where(e => e.Userid == x.UserId && e.Status == (int)StatusAdmin.Active).Select(e =>
                _context.Registers.Where(y => y.Id == e.RegisterId).Select(y=>new
                {
                    RegistersId= y.Id,y.RegistersName,
                    y.Branch.BranchId,y.Branch.BranchName,
                    Role = _context.Roles.Where(z => z.RoleId == e.RoleId).Select(z => new {z.RoleId,RoleName=z.Name}).FirstOrDefault(),
                }).FirstOrDefault()
               // Buss=_context.bus
                ).FirstOrDefault()

            }).FirstOrDefault();
            if (data == null)
            {
                throw new ManagerProcessException("000047");

            }
            return data;


        }
        public async ValueTask<object> GetUserPermissions(int userId,int?PortalId)
        {

            var data = _context.PermissionCategories.Where(e => e.Status == (int)StatusAdmin.Active&&(PortalId==null||e.PermissionPortalId==PortalId ))
                .Select(e => new
                {
                    e.PermissionCatId,
                    e.PerCatName,
                    e.CreatedAt,
                    e.CreatedBy,
                    PermissionSubCategories = _context.PermissionSubCategories.Where(x => x.PermissionCatId == e.PermissionCatId && x.Status == (int)StatusAdmin.Active)
                    .Select(y => new
                    {
                        y.PerSubCatName,
                        y.PermissionSubCatId,
                        PermissionItems = _context.PermissionItems.Where(z => z.PermissionSubCatId == y.PermissionSubCatId && z.Status == (int)StatusAdmin.Active)
                        .Select(w => new {
                            w.PerItemName,
                            w.PermissionItemId,

                            Selected = _context.UserPermissions.Any(a => a.UserId == userId && a.PermissionItemId == w.PermissionItemId && a.Status == (int)StatusAdmin.Active),
                            PermissionItemDetails = _context.PermissionItemDetails.Where(f => f.PermissionItemId == w.PermissionItemId && f.Status == (int)StatusAdmin.Active)
                            .Select(s => new {
                                s.PerItemDetailsName,
                                s.PermissionItemDetailsId,
                                Selected = _context.UserPermissions.Any(b => b.UserId == userId && b.PermissionItemDetailId == s.PermissionItemDetailsId && b.Status == (int)StatusAdmin.Active)

                            })
                            .ToList()
                        }).ToList()
                    }).ToList()
                }).ToList();
            if (data == null)
            {
                throw new ManagerProcessException("000047");

            }
            return data;


        }
  
   
         public async ValueTask<object> GetUserMenu(int userId, string ipAddress,int portalId)
        {
            List<object> menu=new List<object>();
            object RegisterSession;
            var roleId = _context.Users.Where(e => e.UserId == userId).Select(e => e.RoleId).FirstOrDefault(); 
            if (roleId == (int)Roles.CasherRegister&&portalId==(int)Portals.POS)
            {
                List<object> Cashier;
                CasherMenu(userId, ipAddress, out RegisterSession, out menu, out Cashier);
                 menu.Add(new { Cashier, RegisterSession,errors });
            }
            if (roleId == (int)Roles.SuperCashier && portalId == (int)Portals.POS)
            {
                List<object> SuperCashier;
                CasherMenu(userId, ipAddress, out RegisterSession, out menu, out SuperCashier);
                SuperCashier.Add(new { UnsettledSessions = new { CanView = true, CanClick = true } });
                menu.Add(new { SuperCashier, RegisterSession });
            }
            if (roleId == (int)Roles.PartsSalesConsultant && portalId == (int)Portals.SparepartsIntermediateLayers)
            {
                List<object> SparePartsWorkOrders=new List<object>();
                SparePartsWorkOrders.Add(new { SparePartsWorkOrders = new { CanView = true, CanClick = true } });
                menu.Add(new { SparePartsWorkOrders });
            }
            if (roleId == (int)Roles.ServiceAdvisor && portalId == (int)Portals.ServiceIntermediateLayer)
            {
                List<object> ServiceAdvisor = new List<object>();
                ServiceAdvisor.Add(new { ServiceAdvisor = new { CanView = true, CanClick = true } });
                menu.Add(new { ServiceAdvisor });
            }
            if (roleId == (int)Roles.SuperAdmin && portalId == (int)Portals.POS)
            {
                List<object> SuperAdmin = new List<object>();
                SuperAdmin.Add(new {
                    Branches = new { CanView = true, CanClick = true },
                    Regsiters = new { CanView = true, CanClick = true },
                    Roles = new { CanView = true, CanClick = true },
                    Users = new { CanView = true, CanClick = true },
                    OtherRevenuesSetup = new { CanView = true, CanClick = true }
                });
                menu.Add(new { SuperAdmin });
            }
            if (roleId == (int)Roles.TreasuryAdmin && portalId == (int)Portals.POS)
            {
                List<object> TreasuryAdmin = new List<object>();
                TreasuryAdmin.Add(new
                {
                    Allocation = new { CanView = true, CanClick = true },
                    PettyCash = new { CanView = true, CanClick = true },
                    TransactionsHistory = new { CanView = true, CanClick = true }
                });
                menu.Add(new { TreasuryAdmin });
            }
            if (roleId == (int)Roles.MainFund && portalId == (int)Portals.POS)
            {
                List<object> MainFundAdmin = new List<object>();
                MainFundAdmin.Add(new
                {
                    Dashboard = new { CanView = true, CanClick = true },
                    PettyCashOrders = new { CanView = true, CanClick = true },
                    Allocation = new { CanView = true, CanClick = true },
                    RegisterSettlements = new { CanView = true, CanClick = true }
                });
                menu.Add(new { MainFundAdmin });
            }

            return menu;
        }

        private void CasherMenu(int userId, string ipAddress, out object RegisterSession, out List<object> menu, out List<object> Cashier)
        {
            bool canOpenRegister = true;
            bool canViewRegister = true;
            bool canClose = true;
            bool viewCollect = true;
            bool canCollect = true;
            bool PrettyView = true;
            canViewRegister = CanView(userId, ipAddress);
            canOpenRegister = canViewRegister;
            canCollect = canViewRegister;
            canClose = canViewRegister;
            viewCollect = canViewRegister;
            if (canViewRegister)
            {
                canOpenRegister = CanOpen(userId);
                canCollect = canOpenRegister;
            }
            if (canClose)
            {
                canClose = CanClose(userId);
            }
            if (viewCollect)
            {
                viewCollect = ViewCollectCheck(userId);
            }
            canCollect = viewCollect;
            if (canCollect)
            {
                canCollect = CanCollectCheck(userId);
            }
            PrettyView = PrettyViewCheck(userId, ipAddress);
            var register = _context.RegisterEmployees.Where(e => e.Userid == userId && e.RoleId == (int)Roles.CasherRegister && e.Status == (int)StatusAdmin.Active).Select(e => new { e.RegisterId, e.Register.AllowedSessionTimeTo }).FirstOrDefault();
            RegisterSession = null;
            if (register != null)
            {
                var openSession = _context.RegisterSessions.Where(e => e.RegisterId == register.RegisterId && e.Status == (int)RegisterStatus.Opened).FirstOrDefault();
                RegisterSession = new
                {
                    hasOppendSession = openSession != null,
                    CurrentSessionId = openSession?.RegisterSessionId,
                    oppendAt = openSession?.CreatedAt,
                    oppendBy = openSession == null ? "" : _context.Users.Where(e => e.UserId == openSession.CreatedBy).Select(e => e.FullName).FirstOrDefault(),
                    AllowedToTime = register.AllowedSessionTimeTo
                };

            }


            menu = new List<object>();
            Cashier = new List<object>();
            Cashier.Add(new
            {
                Type = 1,
                Title = "Open Session",
                IsActive = true,
                Icon = _config["Settings:BaseSite"] + "openSession.png",
                Icon2 = _config["Settings:v"] + "openSession.png",
                url = "https://markaziaposdev.azurewebsites.net/",
                OpenSession = new
                {
                    Type = 2,
                    Title = "Open Session",
                    IsActive = true,
                    Icon = _config["Settings:BaseUrl"] + "openSession.png",
                    Icon2 = _config["Settings:BaseSite"] + "openSession.png",
                    url = "https://markaziaposdev.azurewebsites.net/",
                    CanView = canViewRegister,
                    CanClick = canOpenRegister
                }
            });
            Cashier.Add(new
            {
                Type = 1,
                Title = "Close Session",
                IsActive = true,
                Icon = _config["Settings:BaseSite"] + "assets/images/navicon/closeregister.png",
                Icon2 = _config["Settings:BaseSite"] + "assets/images/navicon/closeregister2.png",
                url = "h/close-register",
                CloseSession = new
                {
                    Type = 2,
                    Title = "Close Session",
                    IsActive = true,
                    Icon = _config["Settings:BaseSite"] + "assets/images/navicon/closeregister.png",
                    Icon2 = _config["Settings:BaseSite"] + "assets/images/navicon/closeregister2.png",
                    url = "/close-register",
                    CanView = canViewRegister,
                    CanClick = canClose
                }
            });
            Cashier.Add(new
            {
                Type = 1,
                Title = "Collect",
                IsActive = true,
                Icon = _config["Settings:BaseSite"] + "assets/images/navicon/collect.png",
                Icon2 = _config["Settings:BaseSite"] + "assets/images/navicon/collect2.png",
                url = "/collect",
                Collect = new
                {
                    Type = 2,
                    Title = "Collect",
                    IsActive = true,
                    Icon = _config["Settings:BaseSite"] + "assets/images/navicon/collect.png",
                    Icon2 = _config["Settings:BaseSite"] + "assets/images/navicon/collect2.png",
                    url = "/collect",
                    CanView = viewCollect,
                    CanClick = canCollect
                }
            });
            Cashier.Add(new
            {
                Type = 1,
                Title = "Pretty",
                IsActive = true,
                Icon = _config["Settings:BaseSite"] + "assets/images/navicon/pettycash.png",
                Icon2 = _config["Settings:BaseSite"] + "assets/images/navicon/pettycash2.png",
                url = "/pettycash",
                Pretty = new
                {
                    Type = 2,
                    Title = "Pretty",
                    IsActive = true,
                    Icon = _config["Settings:BaseSite"] + "openSession.png",
                    Icon2 = _config["Settings:BaseSite"] + "openSession.png",
                    url = "/pettycash",
                    CanView = PrettyView,
                    CanClick = PrettyView
                }
            });
            Cashier.Add(new
            {
                Type = 1,
                Title = "Unsettled Session",
                IsActive = true,
                Icon = _config["Settings:BaseSite"] + "openSession.png",
                Icon2 = _config["Settings:BaseSite"] + "openSession.png",
                url = "https://markaziaposdev.azurewebsites.net/",
                UnsettledSession = new
                {
                    Type = 2,
                    Title = "Unsettled Session",
                    IsActive = true,
                    Icon = _config["Settings:BaseSite"] + "openSession.png",
                    Icon2 = _config["Settings:BaseSite"] + "openSession.png",
                    url = "https://markaziaposdev.azurewebsites.net/",
                    CanView = true,
                    CanClick = true
                }
            });
        }

        private bool CanView(int userId, string ipAddress)
        {
            int? RegisterId = _context.RegisterEmployees.Where(e => e.Userid == userId && e.RoleId == (int)Roles.CasherRegister && e.Status == (int)StatusAdmin.Active).Select(e => e.RegisterId).FirstOrDefault();
            if (RegisterId == null || RegisterId == 0)
            {
                errors.Add("RegisterId");
                return false;

            }
            //var isIpAddressAllowed = _context.RegisterIpAddresss.Where(e => e.RegisterId == RegisterId && ipAddress.Contains(e.IpAddress.Trim()) && e.Status == (int)StatusAdmin.Active).FirstOrDefault();
            //if (isIpAddressAllowed == null)
            //{
            //    errors.Add("isIpAddressAllowed");
            //    return false;


            //}
            var userHasPermissions = _context.UserPermissions.Where(e => e.UserId == userId && e.PermissionItemId == (int)Permissions.OpenCloseRegister).Any();
            if (!userHasPermissions)
            {
                errors.Add("userHasPermissions");
                return false;


            }

            var register = _context.Registers.Where(e => e.Id == RegisterId && e.Status == (int)StatusAdmin.Active).Select(e => new { e.BranchId, e.City, e.NumberOfSessionsPerDay,/* e.NumberOfSessionsPerWeek,*/ e.AllowedSessionTimeFrom, e.AllowedSessionTimeTo }).FirstOrDefault();
            if (register == null)
            {
                return false;

            }
            return true;
        }
        private bool CanOpen(int userId)
        {
            int? RegisterId = _context.RegisterEmployees.Where(e => e.Userid == userId && e.RoleId == (int)Roles.CasherRegister && e.Status == (int)StatusAdmin.Active).Select(e => e.RegisterId).FirstOrDefault();
            var register = _context.Registers.Where(e => e.Id == RegisterId && e.Status == (int)StatusAdmin.Active).Select(e => new { e.BranchId, e.City, e.NumberOfSessionsPerDay,/* e.NumberOfSessionsPerWeek,*/ e.AllowedSessionTimeFrom, e.AllowedSessionTimeTo }).FirstOrDefault();

            var existOpenSession = _context.RegisterSessions.Any(e => e.RegisterId == RegisterId && e.Status == (int)RegisterStatus.Opened);

            if (existOpenSession)
            {
                return false;

            }

            var existSessionforTodayCount = _context.RegisterSessions.Where(e => e.RegisterId == RegisterId && e.CreatedAt.Value.Date == DateTime.Now.Date /*&& e.Status == (int)StatusAdmin.Active*/).Count();
            if (existSessionforTodayCount >= register.NumberOfSessionsPerDay)
            {
                errors.Add("existSessionforTodayCount");
                return false;

            }
            //DateTime weekFirstDate = _utilitiesHelper.StartOfWeek(DateTime.Now, DayOfWeek.Sunday);
            //DateTime weekEndDate = _utilitiesHelper.StartOfWeek(DateTime.Now, DayOfWeek.Saturday);

            //var existSessionforWeekCount = _context.RegisterSessions.Where(e => e.RegisterId == RegisterId && e.CreatedAt.Value.Date >= weekFirstDate && e.CreatedAt.Value.Date <= weekEndDate /*&& e.Status == (int)StatusAdmin.Active*/).Count();
            //if (existSessionforWeekCount >= register.NumberOfSessionsPerWeek)
            //{
            //    return false;
            //}
            if (DateTime.Now.TimeOfDay < register.AllowedSessionTimeFrom || DateTime.Now.TimeOfDay > register.AllowedSessionTimeTo)
            {
                errors.Add("AllowedSessionTimeFrom");
                return false;
            }
            // var closedBeforceToday = _context.RegisterSessionsForceClose.Where(e => e.RegisterID == RegisterId && e.CreatedAt.Date == DateTime.Now.Date).Any();
            var CantClose = _context.RegisterSessions.Where(e => e.RegisterId == RegisterId && e.CantClose == true && e.CreatedAt.Value.Date == DateTime.Now.Date).Any();

            if (CantClose)
            {
                errors.Add("CantClose");
                return false;

            }
            return true;

        }
        private bool CanClose(int userId)
        {
            int? RegisterId = _context.RegisterEmployees.Where(e => e.Userid == userId && e.RoleId == (int)Roles.CasherRegister && e.Status == (int)StatusAdmin.Active).Select(e => e.RegisterId).FirstOrDefault();

            int? CreatedBy = _context.RegisterSessions.Where(e => e.RegisterId == RegisterId && e.Status == (int)RegisterStatus.Opened).Select(e => e.CreatedBy).FirstOrDefault();

            if (CreatedBy == null || CreatedBy == 0)
            {
                return false;

            }
            if (CreatedBy != userId)
            {
                errors.Add("CantCloseCreatedbyAnoterUser");
                return false;
            }
            var CantClose = _context.RegisterSessions.Where(e => e.RegisterId == RegisterId && e.CantClose == true && e.CreatedAt.Value.Date == DateTime.Now.Date).Any();
            if (CantClose == true)
            {
                errors.Add("CantCloseFlag");
                return false;
            }
            return true;
        }
        private bool ViewCollectCheck(int userId)
        {
            int? RegisterId = _context.RegisterEmployees.Where(e => e.Userid == userId && e.RoleId == (int)Roles.CasherRegister && e.Status == (int)StatusAdmin.Active).Select(e => e.RegisterId).FirstOrDefault();
            var registerCollect = _context.Registers.Where(e => e.Id == RegisterId && e.Status == (int)StatusAdmin.Active && (e.CollectForAllBranches == true || e.CollectForOwnBranch == true)).Any();
            var CollectRegisterYes = _context.UserPermissions.Where(e => e.UserId == userId && e.PermissionItemId == (int)Permissions.CollectRegisterYes).Any();
            if (registerCollect == false || CollectRegisterYes == false)
            {
                errors.Add("registerCollect");
                return false;
            }
            return true;
        }
        private bool CanCollectCheck(int userId)
        {
            int? RegisterId = _context.RegisterEmployees.Where(e => e.Userid == userId && e.RoleId == (int)Roles.CasherRegister && e.Status == (int)StatusAdmin.Active).Select(e => e.RegisterId).FirstOrDefault();
            var AllowedSessionTimeTo = _context.Registers.Where(e => e.Id == RegisterId).Select(e => e.AllowedSessionTimeTo).FirstOrDefault();
            var openSession = _context.RegisterSessions.Where(e => e.RegisterId == RegisterId && e.Status == (int)RegisterStatus.Opened).FirstOrDefault();
            if (openSession == null)
            {
                return false;
            }
            if (openSession.CreatedBy != userId)
            {
                return false;
            }
            if (DateTime.Now.TimeOfDay > AllowedSessionTimeTo || DateTime.Now.Date != openSession?.CreatedAt.Value.Date)
            {
                errors.Add("CanCollectCheckTime");
                return false;
            }
            return true;
        }
        private bool PrettyViewCheck(int userId, string ipAddress)
        {
            int? RegisterId = _context.RegisterEmployees.Where(e => e.Userid == userId && e.RoleId == (int)Roles.CasherRegister && e.Status == (int)StatusAdmin.Active).Select(e => e.RegisterId).FirstOrDefault();
            if (RegisterId == null || RegisterId == 0)
            {
                return false;

            }
            //var isIpAddressAllowed = _context.RegisterIpAddresss.Where(e => e.RegisterId == RegisterId && ipAddress.Contains(e.IpAddress.Trim()) && e.Status == (int)StatusAdmin.Active).FirstOrDefault();
            //if (isIpAddressAllowed == null)
            //{
            //    errors.Add("PrettyViewCheck IP");
            //    return false;


            //}

            var register = _context.Registers.Where(e => e.Id == RegisterId && e.Status == (int)StatusAdmin.Active).Select(e => new { e.BranchId, e.City, e.NumberOfSessionsPerDay, /*e.NumberOfSessionsPerWeek,*/ e.AllowedSessionTimeFrom, e.AllowedSessionTimeTo }).FirstOrDefault();
            if (register == null)
            {
                return false;

            }
            return true;
        }
        public async ValueTask<object> GetUserMenuNew(int userId, string ipAddress, int portalId)
        {


            bool canOpenRegister = true;
            bool canViewRegister = true;
            bool canClose = true;
            bool viewCollect = true;
            bool canCollect = true;
            bool PrettyView = true;
            canViewRegister = CanViewNew(userId, ipAddress);
            canOpenRegister = canViewRegister;
            canCollect = canViewRegister;
            canClose = canViewRegister;
            viewCollect = canViewRegister;
            var roleId=_context.Users.Where(e=>e.UserId==userId).Select(e=>e.RoleId).FirstOrDefault();  
            if (canViewRegister)
            {
                canOpenRegister = CanOpenNew(userId);
                canCollect = canOpenRegister;
            }
            if (canClose)
            {
                canClose = CanCloseNew(userId);
            }
            if (viewCollect)
            {
                viewCollect = ViewCollectCheckNew(userId);
            }
            canCollect = viewCollect;
            if (canCollect)
            {
                canCollect = CanCollectCheckNew(userId);
            }
            PrettyView = PrettyViewCheckNew(userId, ipAddress);
            var register = _context.RegisterEmployees.Where(e => e.Userid == userId && e.RoleId == (int)Roles.CasherRegister && e.Status == (int)StatusAdmin.Active).Select(e => new { e.RegisterId, e.Register.AllowedSessionTimeTo }).FirstOrDefault();
            object RegisterSession = null;
            if (register != null)
            {
                var openSession = _context.RegisterSessions.Where(e => e.RegisterId == register.RegisterId && e.Status == (int)RegisterStatus.Opened).FirstOrDefault();
                RegisterSession = new
                {
                    hasOppendSession = openSession != null,
                    CurrentSessionId = openSession?.RegisterSessionId,
                    oppendAt = openSession?.CreatedAt,
                    oppendBy = openSession == null ? "" : _context.Users.Where(e => e.UserId == openSession.CreatedBy).Select(e => e.FullName).FirstOrDefault(),
                    AllowedToTime = register.AllowedSessionTimeTo
                };

            }


            //List<object> menu = new List<object>();


            //List<object> Cashier = new List<object>();
            // Cashier.Add(new{ OpenSession = new { CanView = canViewRegister, CanClick = canOpenRegister } });
            // Cashier.Add(new { CloseSession = new { CanView = canViewRegister, CanClick = canClose } });
            // Cashier.Add(new { Collect = new { CanView = viewCollect, CanClick = canCollect } });
            // Cashier.Add(new { Pretty = new { CanView = PrettyView, CanClick = PrettyView } });
            //Cashier.Add(new { UnsettledSession = new { CanView = true, CanClick = true } });
            
         var menu=   _context.MenuCategories.Where(e => e.PortalId == portalId&&e.Status==(int)StatusAdmin.Active&&e.RoleId== roleId).Select(e => new
            {
                MenuItem=e.MenuCategoryName,
             e.MenuCategoryId,
             e.OrderNo,
                e.Status,
                Items = _context.MenuItems.Where(x => x.MenuCategoryId == e.MenuCategoryId&&x.IsActive==true).Select(x => new
                { 
                    x.Title,
                    
                    x.MenuItemId,
                    x.Url,
                    x.Icon,
                    x.Icon2,
                    x.IsActive,
                    x.OrderNo,
                    CanView = (x.MenuItemId==1|| x.MenuItemId == 2 ? canViewRegister: x.MenuItemId == 3? viewCollect:x.MenuItemId==4|| x.MenuItemId == 5 ? PrettyView:true),
                    CanClick = x.MenuItemId == 1?canOpenRegister: x.MenuItemId == 2 ? canClose: x.MenuItemId == 3 ? canCollect : x.MenuItemId == 4 || x.MenuItemId == 5 ? PrettyView : true
            }).OrderBy(x=>x.OrderNo).ToList(),
              

            });
            //Cashier.Add(new
            //{
            //    Type = 1,
            //    Title = "Open Session",
            //    IsActive = true,
            //    Icon = _config["Settings:BaseSite"] + "openSession.png",
            //    Icon2 = _config["Settings:v"] + "openSession.png",
            //    url = "https://markaziaposdev.azurewebsites.net/",
        
            //        CanView = canViewRegister,
            //        CanClick = canOpenRegister
                
            //});
            //Cashier.Add(new
            //{
            //    Type = 1,
            //    Title = "Close Session",
            //    IsActive = true,
            //    Icon = _config["Settings:BaseSite"] + "assets/images/navicon/closeregister.png",
            //    Icon2 = _config["Settings:BaseSite"] + "assets/images/navicon/closeregister2.png",
            //    url = "close-register",
           
            //        CanView = canViewRegister,
            //        CanClick = canClose
                
            //});
            //Cashier.Add(new
            //{
            //    Type = 1,
            //    Title = "Collect",
            //    IsActive = true,
            //    Icon = _config["Settings:BaseSite"] + "assets/images/navicon/collect.png",
            //    Icon2 = _config["Settings:BaseSite"] + "assets/images/navicon/collect2.png",
            //    url = "/collect",
        
            //        CanView = viewCollect,
            //        CanClick = canCollect
                
            //});
            //Cashier.Add(new
            //{
            //    Type = 1,
            //    Title = "Pretty",
            //    IsActive = true,
            //    Icon = _config["Settings:BaseSite"] + "assets/images/navicon/pettycash.png",
            //    Icon2 = _config["Settings:BaseSite"] + "assets/images/navicon/pettycash2.png",
            //    url = "/pettycash",
        
            //        CanView = PrettyView,
            //        CanClick = PrettyView
                
            //});
            //Cashier.Add(new
            //{
            //    Type = 1,
            //    Title = "Unsettled Session",
            //    IsActive = true,
            //    Icon = _config["Settings:BaseSite"] + "openSession.png",
            //    Icon2 = _config["Settings:BaseSite"] + "openSession.png",
            //    url = "https://markaziaposdev.azurewebsites.net/",
            
            //        CanView = true,
            //        CanClick = true
                
            //});

            //menu.Add(new { MenuItem = "Cashier", Cashier });



            return new { menu, RegisterSession, errors };
        }
        private bool CanViewNew(int userId, string ipAddress)
        {
            int? RegisterId = _context.RegisterEmployees.Where(e => e.Userid == userId && e.RoleId == (int)Roles.CasherRegister && e.Status == (int)StatusAdmin.Active).Select(e => e.RegisterId).FirstOrDefault();
            if (RegisterId == null || RegisterId == 0)
            {
                errors.Add("Noregister");
                return false;

            }
            //var isIpAddressAllowed = _context.RegisterIpAddresss.Where(e => e.RegisterId == RegisterId && ipAddress.Contains(e.IpAddress.Trim()) && e.Status == (int)StatusAdmin.Active).FirstOrDefault();
            //if (isIpAddressAllowed == null)
            //{
            //    errors.Add("isIpAddressAllowed");
            //    return false;


            //}
            var userHasPermissions = _context.UserPermissions.Where(e => e.UserId == userId && e.PermissionItemId == (int)Permissions.OpenCloseRegister).Any();
            if (!userHasPermissions)
            {
                errors.Add("userHasPermissions");
                return false;


            }

            var register = _context.Registers.Where(e => e.Id == RegisterId && e.Status == (int)StatusAdmin.Active).Select(e => new { e.BranchId, e.City, e.NumberOfSessionsPerDay,/* e.NumberOfSessionsPerWeek,*/ e.AllowedSessionTimeFrom, e.AllowedSessionTimeTo }).FirstOrDefault();
            if (register == null)
            {
                errors.Add("Noregister2");
                return false;

            }
            return true;
        }
        private bool CanOpenNew(int userId)
        {
            int? RegisterId = _context.RegisterEmployees.Where(e => e.Userid == userId && e.RoleId == (int)Roles.CasherRegister && e.Status == (int)StatusAdmin.Active).Select(e => e.RegisterId).FirstOrDefault();
            var register = _context.Registers.Where(e => e.Id == RegisterId && e.Status == (int)StatusAdmin.Active).Select(e => new { e.BranchId, e.City, e.NumberOfSessionsPerDay,/* e.NumberOfSessionsPerWeek,*/ e.AllowedSessionTimeFrom, e.AllowedSessionTimeTo }).FirstOrDefault();

            var existOpenSession = _context.RegisterSessions.Any(e => e.RegisterId == RegisterId && e.Status == (int)RegisterStatus.Opened);

            if (existOpenSession)
            {
                errors.Add("existOpenSession");
                return false;

            }
            //var hasForceCloseToday = _contextPOS.RegisterSessionsForceClose
            //.Where(e => e.RegisterID == RegisterId && e.CreatedAt.Date == DateTime.Now.Date).Any();
            //if (hasForceCloseToday)
            //{
            //    errors.Add("hasForceCloseToday");
            //    return false;
            //}

            var existSessionforTodayCount = _context.RegisterSessions.Where(e => e.RegisterId == RegisterId && e.CreatedAt.Value.Date == DateTime.Now.Date /*&& e.Status == (int)StatusAdmin.Active*/).Count();
            if (existSessionforTodayCount >= register.NumberOfSessionsPerDay)
            {
                errors.Add("existSessionforTodayCount");

                return false;

            }
            //DateTime weekFirstDate = _utilitiesHelper.StartOfWeek(DateTime.Now, DayOfWeek.Sunday);
            //DateTime weekEndDate = _utilitiesHelper.StartOfWeek(DateTime.Now, DayOfWeek.Saturday);

            //var existSessionforWeekCount = _context.RegisterSessions.Where(e => e.RegisterId == RegisterId && e.CreatedAt.Value.Date >= weekFirstDate && e.CreatedAt.Value.Date <= weekEndDate /*&& e.Status == (int)StatusAdmin.Active*/).Count();
            //if (existSessionforWeekCount >= register.NumberOfSessionsPerWeek)
            //{
            //    return false;
            //}
            if (DateTime.Now.TimeOfDay < register.AllowedSessionTimeFrom || DateTime.Now.TimeOfDay > register.AllowedSessionTimeTo)
            {
                return false;
            }
            // var closedBeforceToday = _context.RegisterSessionsForceClose.Where(e => e.RegisterID == RegisterId && e.CreatedAt.Date == DateTime.Now.Date).Any();
            //var CantClose = _context.RegisterSessions.Where(e => e.RegisterId == RegisterId /*&& e.CantClose == true &&*/&& e.CreatedAt.Value.Date == DateTime.Now.Date).Any();

            //if (CantClose)
            //{
            //    errors.Add("CantClose");
            //    return false;

            //}
            return true;

        }
        private bool CanCloseNew(int userId)
        {
            int? RegisterId = _context.RegisterEmployees.Where(e => e.Userid == userId && e.RoleId == (int)Roles.CasherRegister && e.Status == (int)StatusAdmin.Active).Select(e => e.RegisterId).FirstOrDefault();

            int? CreatedBy = _context.RegisterSessions.Where(e => e.RegisterId == RegisterId && e.Status == (int)RegisterStatus.Opened).Select(e => e.CreatedBy).FirstOrDefault();

            if (CreatedBy == null || CreatedBy == 0)
            {
                errors.Add("RegisterSessions");
                return false;

            }
            if (CreatedBy != userId)
            {
                errors.Add("RegisterSessions2");
                return false;
            }
            var CantClose = _context.RegisterSessions.Where(e => e.RegisterId == RegisterId && e.CantClose == true && e.CreatedAt.Value.Date == DateTime.Now.Date).Any();
            if (CantClose == true)
            {
                errors.Add("CantClose");
                return false;
            }
            return true;
        }
        private bool ViewCollectCheckNew(int userId)
        {
            int? RegisterId = _context.RegisterEmployees.Where(e => e.Userid == userId && e.RoleId == (int)Roles.CasherRegister && e.Status == (int)StatusAdmin.Active).Select(e => e.RegisterId).FirstOrDefault();
            var registerCollect = _context.Registers.Where(e => e.Id == RegisterId && e.Status == (int)StatusAdmin.Active && (e.CollectForAllBranches == true || e.CollectForOwnBranch == true)).Any();
            var CollectRegisterYes = _context.UserPermissions.Where(e => e.UserId == userId && e.PermissionItemId == (int)Permissions.CollectRegisterYes).Any();
            if (registerCollect == false || CollectRegisterYes == false)
            {
                errors.Add("registerCollect");
                return false;
            }
            return true;
        }
        private bool CanCollectCheckNew(int userId)
        {
            int? RegisterId = _context.RegisterEmployees.Where(e => e.Userid == userId && e.RoleId == (int)Roles.CasherRegister && e.Status == (int)StatusAdmin.Active).Select(e => e.RegisterId).FirstOrDefault();
            var AllowedSessionTimeTo = _context.Registers.Where(e => e.Id == RegisterId).Select(e => e.AllowedSessionTimeTo).FirstOrDefault();
            var openSession = _context.RegisterSessions.Where(e => e.RegisterId == RegisterId && (e.Status == (int)RegisterStatus.Opened|| e.Status == (int)RegisterStatus.Waiting)).FirstOrDefault();
            if (openSession == null)
            {
                errors.Add("noopenSession");
                return false;
            }
            if (openSession.CreatedBy != userId)
            {
                errors.Add("openSessionDiffrentUser");
                return false;
            }
            if (DateTime.Now.TimeOfDay > AllowedSessionTimeTo || DateTime.Now.Date != openSession?.CreatedAt.Value.Date)
            {
                errors.Add("TimeOfDay");
                return false;
            }
            return true;
        }
        private bool PrettyViewCheckNew(int userId, string ipAddress)
        {
            int? RegisterId = _context.RegisterEmployees.Where(e => e.Userid == userId && e.RoleId == (int)Roles.CasherRegister && e.Status == (int)StatusAdmin.Active).Select(e => e.RegisterId).FirstOrDefault();
            if (RegisterId == null || RegisterId == 0)
            {
                errors.Add("PrettyViewCheck");
                return false;

            }
            //var isIpAddressAllowed = _context.RegisterIpAddresss.Where(e => e.RegisterId == RegisterId && ipAddress.Contains(e.IpAddress.Trim()) && e.Status == (int)StatusAdmin.Active).FirstOrDefault();
            //if (isIpAddressAllowed == null)
            //{
            //    errors.Add("PrettyViewCheck isIpAddressAllowed");

            //    return false;


            //}

            var register = _context.Registers.Where(e => e.Id == RegisterId && e.Status == (int)StatusAdmin.Active).Select(e => new { e.BranchId, e.City, e.NumberOfSessionsPerDay, /*e.NumberOfSessionsPerWeek,*/ e.AllowedSessionTimeFrom, e.AllowedSessionTimeTo }).FirstOrDefault();
            if (register == null)
            {
                errors.Add("PrettyViewCheckRegister");

                return false;

            }
            return true;
        }
        public async ValueTask<object> GetUserPortals(int userId)
        {
            var portalsPermissions =new List<int>
            {
                125,//pos
                117,//credit
                119,//service
                95,//treasury
                121,//spare part
                123,//directpayment
                90,//admin,
                166,//Treasury Layer,
                180,//cm layer,
                193,//sparepart system
                195//vehicle system

            };
            var data = (from up in _context.UserPermissions
                        join permission in _context.PermissionItems on up.PermissionItemId equals permission.PermissionItemId
                        join subCat in _context.PermissionSubCategories on permission.PermissionSubCatId equals subCat.PermissionSubCatId
                        join cat in _context.PermissionCategories on subCat.PermissionCatId equals cat.PermissionCatId
                        join lookup in _context.Lookups on cat.PermissionPortalId equals lookup.LookupId
                        where up.UserId == userId && portalsPermissions.Contains(up.PermissionItemId)
                        select new
                        {
                            lookup.LookupId,
                            ImagePath = _config["Settings:AdminBaseUrl"] + lookup.LookupImage ?? "",
                            lookup.Description,
                            Translations = _context.LookupTranslations.Where(e => e.LookupId == lookup.LookupId).Select(e =>
                                e.LookupName
                            ).FirstOrDefault(),
                        }).Distinct().ToList();
            return data;


        }

    
    }
}
