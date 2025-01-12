using Intl.Realty.Firm.Helper;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.ReferAnAgentVM;
using Intl.Realty.Firm.Repository.IRepository;
using Intl.Realty.Firm.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Intl.Realty.Firm.Controllers
{
    [Authorize]
    public class ReferAnAgentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfigurationService _configurationService;
        private readonly IFileHandlerService _fileHandlerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public ReferAnAgentController(IUnitOfWork unitOfWork,
                                    IConfigurationService configurationService,
                                    IFileHandlerService fileHandlerService,
                                    UserManager<ApplicationUser> userManager,
                                    IEmailService emailService
                                                                        )
        {
            _unitOfWork = unitOfWork;
            _configurationService = configurationService;
            _fileHandlerService = fileHandlerService;
            _emailService = emailService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] ReferAnAgentViewModel viewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var agentName = Url.Action("Login", "Account",
                                            new { id = viewModel.Name, token = token }, Request.Scheme);

            var contactEmailList = await _unitOfWork.ContactEmail.GetAllAsync(x => x.IsActive == true && !x.IsDeleted, includeProperties: "EmailCategory");

            foreach (var contactEmail in contactEmailList)
            {
                var mailRequest = EmailRequest.SendRemailToReferAnAgent(contactEmail, viewModel, agentName);
                await _emailService.SendEmailAsync(mailRequest);
            }

            return RedirectToAction("Index");
        }
    }
}
