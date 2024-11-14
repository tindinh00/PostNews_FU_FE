using Api.Responses;
using Client.Helpers;
using Client.Requests;
using Client.Responses;
using Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class NewsController : Controller
    {
        private readonly ClientService _clientService;
        public NewsController(ClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(NewsPagingRequest request)
        {
            try
            {
                var response = await _clientService.Get<ODataResponseModel<NewsResponse>>(ApiPaths.News);
                if (response == null) throw new Exception();
                var searchTerm = (request.SearchTerm ?? "").ToLower();
                var news = response.Value;
                news = news
                    .Where(x => x.NewsTitle.ToLower().Contains(searchTerm) || x.NewsContent.ToLower().Contains(searchTerm))
                    .Where(x => request.CategoryId != null && request.CategoryId != -1 ? x.CategoryId == request.CategoryId : true)
                    .ToList();
                request.TotalRecord = news.Count;

                request.TotalPages = (int)Math.Ceiling(request.TotalRecord / (double)request.PageSize);
                news = news.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();               
                request.Items = news;

                var categories = await _clientService.Get<ODataResponseModel<CategoryResponse>>(ApiPaths.Categories);
                var tags = await _clientService.Get<ODataResponseModel<TagResponse>>(ApiPaths.Tags);

                ViewBag.Categories = categories.Value;
                ViewBag.Tags = tags.Value;

            }
            catch (Exception)
            {

            }
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var news = await _clientService.Get<ODataResponseModel<NewsResponse>>(ApiPaths.News);
                var response = await _clientService.Get<NewsResponse>(ApiPaths.News + "/" + id);
                var categories = await _clientService.Get<ODataResponseModel<CategoryResponse>>(ApiPaths.Categories);
                var tags = await _clientService.Get<ODataResponseModel<TagResponse>>(ApiPaths.Tags);
                ViewBag.Categories = categories.Value;
                ViewBag.Tags = tags.Value;
                ViewBag.RelatedNews = news.Value.Take(5).ToList();

                if (response == null) throw new Exception();
                return View(response);
            }
            catch (Exception ex)
            {
                ToastHelper.ShowError(TempData, "Cannot find the news.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
