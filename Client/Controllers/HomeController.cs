using Api.Responses;
using Client.Responses;
using Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClientService _clientService;

        public HomeController(ClientService clientService)
        {
            _clientService = clientService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var homeResponse = new HomeResponse();
                var tags = await _clientService.Get<ODataResponseModel<TagResponse>>(ApiPaths.Tags);
                var categories = await _clientService.Get<ODataResponseModel<CategoryResponse>>(ApiPaths.Categories);
                var news = await _clientService.Get<ODataResponseModel<NewsResponse>>(ApiPaths.News);

                homeResponse.Tags = tags.Value;
                homeResponse.Categories = categories.Value;
                homeResponse.News = news.Value;

                return View(homeResponse);
            }
            catch (Exception)
            {

                return View();
            }
        }
    }
}
