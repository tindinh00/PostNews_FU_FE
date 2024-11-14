namespace Client.Requests
{
    public class PagingRequest<T>
    {
        public string? SearchTerm { get; set; } = "";
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; } = 0;
        public int TotalRecord { get; set; } = 0;
        public List<T>? Items { get; set; }
    }
}
