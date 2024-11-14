namespace Client.Controllers
{
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    [Route("/[Controller]/[Action]")]
    public class ErrorController : Controller
    {
        [HttpGet("/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            Console.WriteLine("statusCode: " + statusCode);
            switch (statusCode)
            {
                case 404:
                    return RedirectToAction(nameof(Error404));
                case 403:
                    return RedirectToAction(nameof(Error403));
                case 429:
                    return RedirectToAction(nameof(Error429));
                default:
                    return RedirectToAction(nameof(Error500));
            }
        }

        [HttpGet]
        public IActionResult Error403()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error404()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error429()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error500()
        {
            return View();
        }
    }
}
