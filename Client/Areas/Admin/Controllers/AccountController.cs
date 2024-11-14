using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Client.Services;
using Client.Requests;
using Api.Responses;
using Client.Responses;
using Client.Helpers;

namespace Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[Controller]/[Action]")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {

        private readonly ClientService _clientService;

        public AccountController(ClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(PagingRequest<AccountResponse> request)
        {
            try
            {
                var response = await _clientService.Get<ODataResponseModel<AccountResponse>>(ApiPaths.Accounts);
                if (response == null) throw new Exception();
                var searchTerm = (request.SearchTerm ?? "").ToLower();
                var accounts = response.Value;
                accounts = accounts
                    .Where(x => x.AccountName.ToLower().Contains(searchTerm) ||
                                x.AccountEmail.ToLower().Contains(searchTerm)).ToList();
                request.TotalRecord = accounts.Count;

                request.TotalPages = (int)Math.Ceiling(request.TotalRecord / (double)request.PageSize);
                accounts = accounts.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();
                request.Items = accounts;
            }
            catch (Exception)
            {

            }
            return View(request);
        }


        [HttpGet]
        public async Task<IActionResult> GetAccountById(int id)
        {
            try
            {
                var response = await _clientService.Get<AccountResponse>(ApiPaths.Accounts + "/" + id);
                if (response == null) throw new Exception("");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountRequest request)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Invalid account information.");

                var response = await _clientService.Post<ResponseModel>(ApiPaths.Accounts, request);
                if (response == null || response?.Success == false) throw new Exception("Create account failed.");

                ToastHelper.ShowSuccess(TempData, "Create account successfully.");

            }
            catch (Exception ex)
            {
                ToastHelper.ShowError(TempData, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(AccountRequest request)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Invalid account information.");
                var url = ApiPaths.Accounts + "/" + request.AccountId;
                var response = await _clientService.Put<ResponseModel>(url, request);
                if (response == null || response?.Success == false) throw new Exception("Update account failed.");

                ToastHelper.ShowSuccess(TempData, "Update account successfully.");

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
                var response = await _clientService.Delete<ResponseModel>(ApiPaths.Accounts + "/" + id);
                if (response == null || response?.Success == false) throw new Exception("Delete account failed.");

                ToastHelper.ShowSuccess(TempData, "Delete account successfully.");
            }
            catch (Exception ex)
            {
                ToastHelper.ShowError(TempData, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
