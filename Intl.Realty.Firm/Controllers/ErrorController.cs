using Intl.Realty.Firm.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Intl.Realty.Firm.Controllers
{
    [Authorize]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        [AllowAnonymous]
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            var model = new NotFoundViewModel
            {
                ControllerName = "Home",
                ControllerAction = "Index"
            };

            switch (statusCode)
            {
                case 403:
                    model.ErrorTitle = "403 - Forbidden";
                    model.ErrorMessage = "You are not authorized to access this resource.";
                    break;
                case 404:
                    model.ErrorTitle = "404 - Page Not Found";
                    model.ErrorMessage = "The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.";
                    break;
                default:
                    model.ErrorTitle = $"Error {statusCode}";
                    model.ErrorMessage = "An unexpected error occurred.";
                    break;
            }

            return View("Error", model);
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var model = new NotFoundViewModel
            {
                ErrorTitle = "Application Error",
                ErrorMessage = "Something went wrong while processing your request.",
                ControllerName = "Home",
                ControllerAction = "Index"
            };

            _logger.LogError($"Path: {exceptionDetails?.Path} threw an exception: {exceptionDetails?.Error}");

            return View("Error", model);
        }

    }
}

