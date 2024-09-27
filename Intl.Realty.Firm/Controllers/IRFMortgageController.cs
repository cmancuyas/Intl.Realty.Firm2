using Microsoft.AspNetCore.Mvc;

namespace Intl.Realty.Firm.Controllers
{
    public class IRFMortgageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
