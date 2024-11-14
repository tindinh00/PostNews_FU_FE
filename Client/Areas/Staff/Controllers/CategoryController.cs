using Api.Responses;
using Client.Helpers;
using Client.Requests;
using Client.Responses;
using Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Google.Apis.Requests.BatchRequest;

namespace Client.Areas.Mentor.Controllers
{
    [Area("Staff")]
    [Route("Staff/[Controller]/[Action]")]
    [Authorize(Roles = "Staff")]
    public class CategoryController : Controller
    {
        private readonly ClientService _clientService;
        public CategoryController(ClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(PagingRequest<CategoryResponse> request)
        {
            try
            {
                var response = await _clientService.Get<ODataResponseModel<CategoryResponse>>(ApiPaths.Categories);
                if (response == null) throw new Exception();
                var searchTerm = (request.SearchTerm ?? "").ToLower();
                var categories = response.Value;
                categories = categories
                    .Where(x => x.CategoryName.ToLower().Contains(searchTerm))
                    .ToList();
                request.TotalRecord = categories.Count;

                request.TotalPages = (int)Math.Ceiling(request.TotalRecord / (double)request.PageSize);
                categories = categories.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                request.Items = categories;
            }
            catch (Exception)
            {

            }
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var response = await _clientService.Get<CategoryResponse>(ApiPaths.Categories + "/" + id);
                if (response == null) throw new Exception("");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Invalid category information.");

                var response = await _clientService.Post<ResponseModel>(ApiPaths.Categories, request);
                if (response == null || response?.Success == false) throw new Exception("Create category failed.");

                ToastHelper.ShowSuccess(TempData, "Create category successfully.");

            }
            catch (Exception ex)
            {
                ToastHelper.ShowError(TempData, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryRequest request)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Invalid category information.");
                var url = ApiPaths.Categories + "/" + request.CategoryId;
                var response = await _clientService.Put<ResponseModel>(url, request);
                if (response == null || response?.Success == false) throw new Exception("Update category failed.");

                ToastHelper.ShowSuccess(TempData, "Update category successfully.");

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
                var response = await _clientService.Delete<ResponseModel>(ApiPaths.Categories + "/" + id);
                if (response == null || response?.Success == false) throw new Exception("Cannot delete category because it is used.");

                ToastHelper.ShowSuccess(TempData, "Delete category successfully.");
            }
            catch (Exception ex)
            {
                ToastHelper.ShowWarning(TempData, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
