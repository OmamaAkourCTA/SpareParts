using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Users
{
    public class GetUsersModel
    {
        public string? Search { get; set; }
        public string?  BranchId { get; set; }
        public string? RoleId { get; set; }
        public int? sort { get; set; }
        public int? Status { get; set; }
        public int? Invitation { get; set; }
        public int? RegisterId { get; set; }



    }
}
