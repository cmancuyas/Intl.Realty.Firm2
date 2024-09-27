using Microsoft.AspNetCore.Mvc;

namespace Intl.Realty.Firm.Controllers
{
    public class IRFLeadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
