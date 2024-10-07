namespace SparePartsModule.Infrastructure.ViewModels
{
    public class PaginationResponseDto<T> where T : class
    {
        public int TotalPageCount { get; set; }
        public List<T> Data { get; set; }
        public PaginationResponseDto(int count, List<T> data)
        {
            TotalPageCount = count;
            Data = data;
        }
    }
}
