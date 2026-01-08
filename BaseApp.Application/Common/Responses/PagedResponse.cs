namespace BaseApp.Application.Common.Responses
{
    public class PagedResponse<T>
    {
        public bool Success { get; set; } = true;
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public string? Message { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);

        public static PagedResponse<T> Create(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords, string? message = null)
        {
            return new PagedResponse<T>
            {
                Data = data,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Message = message
            };
        }
    }
}
