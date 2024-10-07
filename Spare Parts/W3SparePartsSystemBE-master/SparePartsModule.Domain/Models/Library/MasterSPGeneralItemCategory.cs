using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
   public class MasterSPGeneralItemCategory
    {
        [Key]
        public int ItemCategoryID { get; set; }
        public string? ItemCategoryCode { get; set; }
        public string? ItemCategoryNameEn { get; set; }
        public string ItemCategoryNameAr { get; set; }
        public string? ItemCategoryDesc { get; set; }
        public int? ItemCategoryType { get; set; }
        public int? ItemCategoryMinMaxType { get; set; }
        public int? ItemCategorySerialType { get; set; }
        public int? ItemCategoryTariffID { get; set; }
        public int? ItemCategoryTaxType { get; set; }
        public int? ItemCategoryDiscountType { get; set; }
        public string? ItemCategoryComments { get; set; }
        public int? ItemCategoryGroup { get; set; }
        public int? Status { get; set; }
        public bool? Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int? EnterUser { get; set; }
        public DateTime? EnterDate { get; set; }
        public TimeSpan? EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
    }
}
