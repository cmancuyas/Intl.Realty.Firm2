using Microsoft.AspNetCore.Mvc;

namespace Intl.Realty.Firm.Controllers
{
    public class BusinessCardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
