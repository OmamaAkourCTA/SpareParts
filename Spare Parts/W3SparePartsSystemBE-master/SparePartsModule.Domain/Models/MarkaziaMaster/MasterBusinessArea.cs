using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.MarkaziaMaster
{
    public class MasterBusinessArea
    {
        [Key]
        public int BusinessAreaID { get; set; }
        public int? BANo { get; set; }
        public string? BAName { get; set; }
        public int? BAGroup { get; set; }
        public int? BACompany { get; set; }
        public int? BALegalEntity { get; set; }
        public int? BADivision { get; set; }
        public int? BADepartment { get; set; }
        public int? BASectionChannel { get; set; }
        public int? BALocation { get; set; }
        public int? BAUsersCount { get; set; }
        public bool BASales { get; set; }
        public int? BASalesBranchNo { get; set; }
        public int? Status { get; set; }
        public bool Cancelled { get; set; }

    }
}
