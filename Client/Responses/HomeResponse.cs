using Api.Responses;

namespace Client.Responses
{
    public class HomeResponse
    {
        public List<CategoryResponse> Categories { get; set; }

        public List<TagResponse> Tags { get; set; }

        public List<NewsResponse> News { get; set; }
    }
}
