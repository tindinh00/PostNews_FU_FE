using Api.Responses;
using Client.Helpers;
using Client.Models;
using Client.Requests;
using Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Google.Apis.Requests.BatchRequest;

namespace Client.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Route("Staff/[Controller]/[Action]")]
    [Authorize(Roles = "Staff")]
    public class ProfileController : Controller
    {
        private readonly ClientService _clientService; // Service for making API requests

        public ProfileController(ClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            try
            {
                var userInfo = SessionHelper.GetObject<UserInfo>(HttpContext.Session, "UserInfo");
                var response = await _clientService.Get<AccountResponse>(ApiPaths.Accounts + "/" + userInfo.AccountId);
                if (response == null) throw new Exception("Not found your profile.");
                return View(response);
            }
            catch (Exception ex)
            {
                ToastHelper.ShowError(TempData, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(AccountRequest request)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Invalid account information.");
                var url = ApiPaths.Accounts + "/" + request.AccountId;
                var response = await _clientService.Put<ResponseModel>(url, request);
                if (response == null || response?.Success == false) throw new Exception("Update profile failed.");

                var userInfo = SessionHelper.GetObject<UserInfo>(HttpContext.Session, "UserInfo");
                userInfo.AccountName = request.AccountName;

                SessionHelper.SetObject<UserInfo>(HttpContext.Session, "UserInfo", userInfo);

                ToastHelper.ShowSuccess(TempData, "Update profile successfully.");

            }
            catch (Exception ex)
            {
                ToastHelper.ShowError(TempData, ex.Message);
            }
            return RedirectToAction(nameof(Update));
        }

    }
}
