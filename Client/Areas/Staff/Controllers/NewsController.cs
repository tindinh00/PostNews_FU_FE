using Api.Responses;
using Client.Helpers;
using Client.Models;
using Client.Requests;
using Client.Responses;
using Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Route("Staff/[Controller]/[Action]")]
    [Authorize(Roles = "Staff")]
    public class NewsController : Controller
    {
        private readonly ClientService _clientService;
        public NewsController(ClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(PagingRequest<NewsResponse> request)
        {
            try
            {
                var response = await _clientService.Get<ODataResponseModel<NewsResponse>>(ApiPaths.News);
                if (response == null) throw new Exception();
                var searchTerm = (request.SearchTerm ?? "").ToLower();
                var news = response.Value;
                news = news
                    .Where(x => x.NewsTitle.ToLower().Contains(searchTerm) || x.NewsContent.ToLower().Contains(searchTerm))
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
        public async Task<IActionResult> GetNewsById(string id)
        {
            try
            {
                var response = await _clientService.Get<NewsResponse>(ApiPaths.News + "/" + id);
                if (response == null) throw new Exception("");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsRequest request)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Invalid news information.");

                var userInfo = SessionHelper.GetObject<UserInfo>(HttpContext.Session, "UserInfo");
                request.CreatedById = (short) userInfo.AccountId;

                var response = await _clientService.Post<ResponseModel>(ApiPaths.News, request);
                if (response == null || response?.Success == false) throw new Exception("Create news failed.");
                ToastHelper.ShowSuccess(TempData, "Create news successfully.");
            }
            catch (Exception ex)
            {
                ToastHelper.ShowError(TempData, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(NewsRequest request)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Invalid news information.");
                var url = ApiPaths.News + "/" + request.NewsArticleId;
                var userInfo = SessionHelper.GetObject<UserInfo>(HttpContext.Session, "UserInfo");
                request.CreatedById = (short)userInfo.AccountId;
                var response = await _clientService.Put<ResponseModel>(url, request);
                if (response == null || response?.Success == false) throw new Exception("Update news failed.");

                ToastHelper.ShowSuccess(TempData, "Update news successfully.");

            }
            catch (Exception ex)
            {
                ToastHelper.ShowError(TempData, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _clientService.Delete<ResponseModel>(ApiPaths.News + "/" + id);
                if (response == null || response?.Success == false) throw new Exception("Cannot delete news because it is used.");

                ToastHelper.ShowSuccess(TempData, "Delete news successfully.");
            }
            catch (Exception ex)
            {
                ToastHelper.ShowWarning(TempData, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
