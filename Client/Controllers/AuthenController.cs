using Api.Responses;
using Client;
using Client.Constants;
using Client.Helpers;
using Client.Models;
using Client.Requests;
using Client.Responses;
using Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using static Google.Apis.Requests.BatchRequest;

namespace FCMS.Client.Controllers
{
    public class AuthenController : Controller
    {
        private readonly ClientService _clientService; 
        private readonly IConfiguration _configuration;

        public AuthenController(
            IConfiguration configuration,
            ClientService clientService
        )
        {
            _configuration = configuration;
            _clientService = clientService;
        }

        [HttpGet("SignIn")]
        public async Task<IActionResult> SignIn()
        {
            try
            {
                var userInfo = SessionHelper.GetObject<UserInfo>(HttpContext.Session, "UserInfo");

                if (userInfo != null)
                {
                    var role = (RoleEnum) userInfo.AccountRole;

                    switch (role)
                    {
                        case RoleEnum.Admin:
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        case RoleEnum.Staff:
                            return RedirectToAction("Index", "Home", new { area = "Staff" });
                        default:
                            return View("Views/Home/Index.cshtml");
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return View("Views/SignIn.cshtml");
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(LoginRequest loginRequest)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Please enter your email and password.");
                var response = await _clientService.Post<ResponseModel>(ApiPaths.Login, loginRequest);

                if(response == null || response?.Message == null) throw new Exception("Email or password are not correct.");
                var accessToken = response?.Message;
                if (String.IsNullOrEmpty(accessToken))
                {
                    throw new Exception("Login failed.");
                }

                SessionHelper.SetObject(HttpContext.Session, "AccessToken", accessToken);
                var tokenHelper = new TokenHelper();
                var allClaims = tokenHelper.GetAllClaims(accessToken);

                var roleStr = tokenHelper.FindClaimValueByKey(allClaims, "Role");
                var accountRole = -1;
                int.TryParse(roleStr, out accountRole);

                var userInfo = new UserInfo();
                userInfo.AccountRole = accountRole; // Store the user's role integer
                userInfo.AccountName = tokenHelper.FindClaimValueByKey(allClaims, ClaimTypes.GivenName);
                userInfo.AccountEmail = tokenHelper.FindClaimValueByKey(allClaims, ClaimTypes.Email);
                userInfo.AccountId = int.Parse(tokenHelper.FindClaimValueByKey(allClaims, ClaimTypes.NameIdentifier));
                var role = (RoleEnum)userInfo.AccountRole;


                var claims = new List<Claim>
                {
                    new (ClaimTypes.Name, userInfo.AccountEmail),
                    new (ClaimTypes.Role, role == RoleEnum.Admin ? RoleName.Admin : RoleName.Staff) ,
                };

                var claimsIdentity = new ClaimsIdentity(claims, "SessionAuthenticationScheme");
                await HttpContext.SignInAsync("SessionAuthenticationScheme", new ClaimsPrincipal(claimsIdentity));

                SessionHelper.SetObject<UserInfo>(HttpContext.Session, "UserInfo", userInfo);
                ToastHelper.ShowSuccess(TempData, "Login successful.");

                // Redirect the user based on their role

                switch (role)
                {
                    case RoleEnum.Admin:
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    case RoleEnum.Staff:
                        return RedirectToAction("Index", "Home", new { area = "Staff" });
                    default:
                        return View("Views/Home/Index.cshtml");
                }
            }
            catch (Exception ex)
            {
                ToastHelper.ShowError(TempData, ex.Message);
                return RedirectToAction(nameof(SignIn));
            }
        }

        [HttpGet("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            try
            {
                SessionHelper.Remove(HttpContext.Session, "UserInfo");
                SessionHelper.Remove(HttpContext.Session, "AccessToken");

                await HttpContext.SignOutAsync("SessionAuthenticationScheme");
            }
            catch (Exception ex)
            {
                ToastHelper.ShowError(TempData, ex.Message);
            }
            return RedirectToAction("Index", "Home"); 
        }
    }
}
