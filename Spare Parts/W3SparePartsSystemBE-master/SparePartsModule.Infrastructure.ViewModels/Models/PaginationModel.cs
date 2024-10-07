using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models
{
    public class PaginationModel
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }

        public PaginationModel()
        {
            PageNo = 0;
            PageSize = 10;
        }
    }
}
