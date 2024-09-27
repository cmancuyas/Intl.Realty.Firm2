using Microsoft.AspNetCore.Mvc;

namespace Intl.Realty.Firm.Controllers
{
    public class IRFBrandedWebsiteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
