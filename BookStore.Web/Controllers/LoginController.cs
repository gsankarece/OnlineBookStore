using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookStore.Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(JsonResult json)
        {
            return Json(new { success = true, errormessage = "", redirection = Url.Action("Index","Search") });
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}