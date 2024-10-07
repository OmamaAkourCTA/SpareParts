using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Users
{
    public class GetRolesModel
    {
        public string? Name { get; set; }
        public bool? Indoor { get; set; }
        public bool? Outdoor { get; set; }
        public int? NoofUsers { get; set; }
        public int? Status { get; set; }
        public int? Sort { get; set; } = 1;


    }
}
