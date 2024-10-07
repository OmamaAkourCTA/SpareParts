using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.SPOrder
{
    public class GetSPOrdersModel
    {
        public string? OrderSeqNo { get; set; }
        public string? OrderNo { get; set; }
        [SwaggerParameter("GetSuppliers")]
        public string? SupplierId { get; set; }
        [SwaggerParameter("lookup type id 27")]
        public string? OrderType{ get; set; }
        [SwaggerParameter("lookup type id 28")]
        public string? OrderMethod { get; set; }
        [SwaggerParameter("lookup type id 29")]
        public string? OrderFreight { get; set; }
        [SwaggerParameter("date format yyyy-MM-dd")]
        public DateTime? CreationDateFrom { get; set; }
        [SwaggerParameter("date format yyyy-MM-dd")]
        public DateTime? CreationDateTo { get; set; }
        [SwaggerParameter("lookup type id 30")]
        public string? Status { get; set; }
        public string? ItemNo { get; set; }


        [SwaggerParameter("1 default creation date desc \r\n- 2/3 order seq\r\n- 4/5 OrderNo \r\n- 6/7 type \r\n- 8/9 supplier \r\n- 10/ 11 method \r\n- 12/ 13 currency \r\n- 14/15 frieght \r\n- 16/17 creation date \r\n- 18/19 status  ")]
        public int Sort { get; set; }
        public string? Currency { get; set; }
        public string? User { get; set; }

    
    }




    public class GetSPInquiryOrdersModel
    {
        public string? OrderSeqNo { get; set; }
        public string? OrderNo { get; set; }
        public int? OrderItemId { get; set; }
        [SwaggerParameter("GetSuppliers")]
        public string? SupplierId { get; set; }
        [SwaggerParameter("lookup type id 27")]
        public string? OrderType { get; set; }
        [SwaggerParameter("lookup type id 28")]
        public string? OrderMethod { get; set; }
        [SwaggerParameter("lookup type id 29")]
        public string? OrderFreight { get; set; }
        [SwaggerParameter("date format yyyy-MM-dd")]
        public DateTime? CreationDateFrom { get; set; }
        [SwaggerParameter("date format yyyy-MM-dd")]
        public DateTime? CreationDateTo { get; set; }
        [SwaggerParameter("lookup type id 30")]
        public string? Status { get; set; }
        public string? ItemNo { get; set; }


        [SwaggerParameter("1 default creation date desc \r\n- 2/3 order seq\r\n- 4/5 OrderNo \r\n- 6/7 type \r\n- 8/9 supplier \r\n- 10/ 11 method \r\n- 12/ 13 currency \r\n- 14/15 frieght \r\n- 16/17 creation date \r\n- 18/19 status  ")]
        public int Sort { get; set; }
        public string? Currency { get; set; }
        public string? User { get; set; }


    }
}
