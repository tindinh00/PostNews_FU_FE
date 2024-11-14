using Api.Responses;

namespace Client.Requests
{
    public class NewsPagingRequest : PagingRequest<NewsResponse>
    {
        public int CategoryId { get; set; } = -1;
        public int TagId { get; set; } = -1;
    }
}
