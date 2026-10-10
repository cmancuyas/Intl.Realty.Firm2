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
    [Route("ReferAnAgent")]
    public class ReferAnAgentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfigurationService _configurationService;
        private readonly IFileHandlerService _fileHandlerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public ReferAnAgentController(
            IUnitOfWork unitOfWork,
            IConfigurationService configurationService,
            IFileHandlerService fileHandlerService,
            UserManager<ApplicationUser> userManager,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _configurationService = configurationService;
            _fileHandlerService = fileHandlerService;
            _userManager = userManager;
            _emailService = emailService;
        }

        // Short marketing URL: /ReferAnAgent
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            // Prefer the shorter URL when someone hits /ReferAnAgent/Index
            if (HttpContext.Request.Path.Equals("/ReferAnAgent/Index", StringComparison.OrdinalIgnoreCase))
                return RedirectPermanent("/ReferAnAgent");

            return View();
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] ReferAnAgentViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            if (user == null)
                return Unauthorized();

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var referralLink = Url.Action("Login", "Account",
                new { id = viewModel.Name, token = token }, Request.Scheme);

            var contactEmailList = await _unitOfWork.ContactEmail
                .GetAllAsync(x => x.IsActive && !x.IsDeleted, includeProperties: "EmailCategory");

            foreach (var contactEmail in contactEmailList)
            {
                var mailRequest = EmailRequest.SendRemailToReferAnAgent(contactEmail, viewModel, referralLink);
                await _emailService.SendEmailAsync(mailRequest);
            }

            var referral = new AgentReferral
            {
                ReferrerName = viewModel.Name,
                ReferrerPhone = string.Empty,
                ReferredAgentName = viewModel.ReferredAgentName,
                ReferredAgentPhone = viewModel.ReferredAgentPhone,
                Notes = viewModel.Notes,
                SubmittedByUserId = Guid.Parse(userId),
                SubmittedAt = DateTime.UtcNow,
                IsArchived = false
            };


            await _unitOfWork.AgentReferral.AddAsync(referral);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
