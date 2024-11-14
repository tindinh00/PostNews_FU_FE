using Client.Helpers;
using Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace FCMS.Client.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Route("Staff/[Controller]/[Action]")]
    [Authorize(Roles = "Staff")]
    public class HomeController : Controller
    {

        private readonly ClientService _clientService;

        public HomeController(ClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

    }
}
