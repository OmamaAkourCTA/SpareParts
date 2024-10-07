using SparePartsModule.Infrastructure.ViewModels.Models;

namespace SparePartsModule.Infrastructure.ViewModels
{
    public class PaginationDatabaseResponseDto<T> where T : class
    {
        public int TotalPageCount { get; set; }
        public int TotalRecordCount { get; set; }
        public int TotalAllRecordCount { get; set; }
        public object Info { get; set; }
        public List<T> Data { get; set; }
        public PaginationDatabaseResponseDto(PaginationModel pagination, IQueryable<T> query)
        {
            var count = query.Count();
            decimal pageNo = (decimal)count / pagination.PageSize;

            TotalPageCount = (int)Math.Ceiling(pageNo);
            var skip = pagination.PageNo * pagination.PageSize;
            TotalRecordCount = count;
            query = query.Skip(skip).Take(pagination.PageSize);
            Data = query.ToList();
        }
        public PaginationDatabaseResponseDto(PaginationModel pagination, List<T>? query,int x)
        {
            var count = query.Count();
            decimal pageNo = (decimal)count / pagination.PageSize;

            TotalPageCount = (int)Math.Ceiling(pageNo);
            var skip = pagination.PageNo * pagination.PageSize;
            TotalRecordCount = count;
            query = query.Skip(skip).Take(pagination.PageSize).ToList();


            Data = query;
        }
    }
}
