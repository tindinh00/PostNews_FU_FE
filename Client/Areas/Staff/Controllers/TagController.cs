using Api.Responses;
using Client.Helpers;
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
    public class TagController : Controller
    {
        private readonly ClientService _clientService;
        public TagController(ClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(PagingRequest<TagResponse> request)
        {
            try
            {
                var response = await _clientService.Get<ODataResponseModel<TagResponse>>(ApiPaths.Tags);
                if (response == null) throw new Exception();
                var searchTerm = (request.SearchTerm ?? "").ToLower();
                var categories = response.Value;
                categories = categories
                    .Where(x => x.TagName.ToLower().Contains(searchTerm))
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
        public async Task<IActionResult> GetTagById(int id)
        {
            try
            {
                var response = await _clientService.Get<TagResponse>(ApiPaths.Tags + "/" + id);
                if (response == null) throw new Exception("");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(TagRequest request)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Invalid tag information.");

                var response = await _clientService.Post<ResponseModel>(ApiPaths.Tags, request);
                if (response == null || response?.Success == false) throw new Exception("Create tag failed.");

                ToastHelper.ShowSuccess(TempData, "Create tag successfully.");

            }
            catch (Exception ex)
            {
                ToastHelper.ShowError(TempData, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(TagRequest request)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Invalid tag information.");
                var url = ApiPaths.Tags + "/" + request.TagId;
                var response = await _clientService.Put<ResponseModel>(url, request);
                if (response == null || response?.Success == false) throw new Exception("Update tag failed.");

                ToastHelper.ShowSuccess(TempData, "Update tag successfully.");

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
                var response = await _clientService.Delete<ResponseModel>(ApiPaths.Tags + "/" + id);
                if (response == null || response?.Success == false) throw new Exception("Cannot delete tag because it is used.");

                ToastHelper.ShowSuccess(TempData, "Delete tag successfully.");
            }
            catch (Exception ex)
            {
                ToastHelper.ShowWarning(TempData, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
