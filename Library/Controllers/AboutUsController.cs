using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
   public class AboutUsController : Controller
   {
      public IActionResult Index()
      {
         return View();
      }
   }
}
