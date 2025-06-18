using Intl.Realty.Firm.Helper;
using Intl.Realty.Firm.Model.Menu;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.Model.Identity;
using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.FileUploadVM;
using Intl.Realty.Firm.Model.ViewModel.SaleCoopVM;
using Intl.Realty.Firm.Models;
using Intl.Realty.Firm.Models.Auxiliary;
using Intl.Realty.Firm.Models.DataTable;
using Intl.Realty.Firm.Repository.IRepository;
using Intl.Realty.Firm.Service.IService;
using Intl.Realty.Firm.Utility.Mapper;
using Intl.Realty.Firm.Utility.MenuData;
using Intl.Realty.Firm.Utility.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Intl.Realty.Firm.Controllers
{
    [Authorize]
    public class SaleCoopController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExportService<SaleCoopExport> _exportService;
        private readonly IConfigurationService _configurationService;
        private readonly IFileHandlerService _fileHandlerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDocumentTransactionService _documentTransactionService;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        private readonly ILogger<SaleCoopController> _logger;
        private readonly string _backupFileDirectory = "\\wwwroot\\Files\\Uploads\\";
        private readonly string _transactionTypeName = "SaleCoop";
        public SaleCoopController(IUnitOfWork unitOfWork,
                                    IExportService<SaleCoopExport> exportService,
                                    IConfigurationService configurationService,
                                    IFileHandlerService fileHandlerService,
                                    UserManager<ApplicationUser> userManager,
                                    IDocumentTransactionService documentTransactionService,
                                    INotificationService notificationService,
                                    IEmailService emailService,
                                    ILogger<SaleCoopController> logger
                                                                        )
        {

            _unitOfWork = unitOfWork;
            _exportService = exportService;
            _configurationService = configurationService;
            _fileHandlerService = fileHandlerService;
            _userManager = userManager;
            _documentTransactionService = documentTransactionService;
            _notificationService = notificationService;
            _emailService = emailService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateSaleCoopWithTabMenuViewModel viewModel = new CreateSaleCoopWithTabMenuViewModel();

            // to get current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // to get current user info
            var user = await _userManager.FindByIdAsync(userId!);

            viewModel.CreateSaleCoopViewModel.CreatedByFullName = user!.FirstName + " " + user.LastName;

            string transactionTypeName = _transactionTypeName; // 1 = Sale Listing

            var dealAdminEmails = await GetDealAdminEmails();

            if (dealAdminEmails == null || dealAdminEmails.Count() == 0)
            {
                var notification = _notificationService.Notify("Go to Administrator > Account Management > Internal Emails > Add New (Role) or Contact the Administrator to fix this issue", "Internal Email for Deal Admin not found", NotificationType.error);
                TempData["Message"] = JsonConvert.SerializeObject(notification);
                return RedirectToAction(nameof(Index));
            }

            var yearValidation = LinqExtensions.AndAlso<DealTransactionNumberRange>(x => x.IsDeleted == false, x => x.Year == DateTime.Now.Year);

            var numberRange = await _unitOfWork.DealTransactionNumberRange.GetAsync(yearValidation);
            if (numberRange == null)
            {
                var notification = _notificationService.Notify("Number Range for current year is not found. Contact administrator to fix this issue", "Number Range not found", NotificationType.error);

                TempData["Message"] = JsonConvert.SerializeObject(notification);
                return RedirectToAction(nameof(Index));
            }

            var transactionType = await _unitOfWork.TransactionType.GetAsync(x => x.Description == transactionTypeName);
            viewModel.CreateSaleCoopViewModel.TransactionType = transactionType;
            viewModel.CreateSaleCoopViewModel.TransactionTypeId = transactionType.Id;

            var paddedTransactionType = transactionType.Id.ToString().PadLeft(2, '0');

            var currentYear = DateTime.Now.Year.ToString();
            var currentMonth = DateTime.Now.Month.ToString().PadLeft(2, '0');
            var currentDay = DateTime.Now.Day.ToString().PadLeft(2, '0');
            var paddedCurrentNumber = (numberRange.CurrentNumber + 1).ToString().PadLeft(5, '0');
            var paddedNumberRange = currentYear + currentMonth + currentDay + "-" + paddedTransactionType + "-" + paddedCurrentNumber;

            viewModel.CreateSaleCoopViewModel.DealTransactionNumber = paddedNumberRange;

            var documentTypeAssignmentIEnum = await _unitOfWork.DocumentTypeAssignment.GetAllAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,DocumentType");
            var documentTypeIEnum = await _unitOfWork.DocumentType.GetAllAsync(x => x.IsDeleted == false);
            var documentTypeAssignmentList = documentTypeAssignmentIEnum.ToList();

            viewModel.CreateSaleCoopViewModel.DocumentTypeAssignmentList = documentTypeAssignmentList.Where(x => x.TransactionType.Id == transactionType.Id).ToList(); ; // DocumentTypeAssignmentList

            var documentTypeList = documentTypeIEnum.ToList();
            viewModel.CreateSaleCoopViewModel.DocumentTypeList = _documentTransactionService
                                                            .GetDocumentTypesFromDocumentTypeAssignment(transactionTypeName,
                                                                                                        documentTypeAssignmentList);

            var menus = CreateDealTransactionTabMenuData.GetCreateDealTransactionTabMenuData();

            string controllerName = "IRFDetails1";

            foreach (var menu in menus.Where(x => x.Menu == controllerName))
            {
                menu.IsActive = "active";
                menu.IsSelected = "true";
            }

            viewModel.Menus = menus;


            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSaleCoopWithTabMenuViewModel viewModel, string actionType)
        {
            viewModel.CreateSaleCoopViewModel.DealStatusId = actionType.ToLower() == "submit" ? 1 : 2;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);
            string transactionTypeName = _transactionTypeName;

            var transactionType = await _unitOfWork.TransactionType.GetAsync(x => x.Description == transactionTypeName);
            viewModel.CreateSaleCoopViewModel.TransactionType = transactionType;
            viewModel.CreateSaleCoopViewModel.TransactionTypeId = transactionType.Id;
            viewModel.CreateSaleCoopViewModel.CreatedByFullName = $"{user.FirstName} {user.LastName}";

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Concurrency-safe deal number increment
                var numberRange = await _unitOfWork.DealTransactionNumberRange
                    .GetAsync(x => !x.IsDeleted && x.Year == DateTime.Now.Year);

                if (numberRange != null)
                {
                    numberRange.CurrentNumber += 1;
                    await _unitOfWork.DealTransactionNumberRange.UpdateAsync(numberRange);
                    await _unitOfWork.SaveChangesAsync(); // save now to avoid conflict
                }

                var documentTypeAssignments = await _unitOfWork.DocumentTypeAssignment.GetAllAsync(
                    x => !x.IsDeleted,
                    includeProperties: "TransactionType,DocumentType"
                );
                var documentTypes = await _unitOfWork.DocumentType.GetAllAsync(x => !x.IsDeleted);

                viewModel.CreateSaleCoopViewModel.DocumentTypeList = _documentTransactionService
                    .GetDocumentTypesFromDocumentTypeAssignment(transactionTypeName, documentTypeAssignments.ToList());

                viewModel.Menus = CreateDealTransactionTabMenuData.GetCreateDealTransactionTabMenuData().Select(m =>
                {
                    m.IsActive = m.Menu == "IRFDetails1" ? "active" : "";
                    m.IsSelected = m.Menu == "IRFDetails1" ? "true" : "false";
                    return m;
                }).ToList();

                if (actionType.ToLower() == "submit" && !ModelState.IsValid)
                {
                    var notification = _notificationService.Notify(
                        "Please review the form for errors. All required fields must be filled correctly.",
                        "Validation Failed",
                        NotificationType.error
                    );
                    TempData["Message"] = JsonConvert.SerializeObject(notification);
                    return View("Index", viewModel);
                }

                // IRF Deal creation
                var irfDeal = viewModel.CreateSaleCoopViewModel.CreateIRFDealSaleCoopViewModel?.ToIRFDealModel();
                irfDeal!.CreatedAt = DateTime.Now;
                irfDeal.CreatedBy = user.Id;
                irfDeal.IsActive = true;
                irfDeal.IsDeleted = false;
                await _unitOfWork.IRFDeal.AddAsync(irfDeal);
                await _unitOfWork.SaveChangesAsync();

                var newIRFDeal = await _unitOfWork.IRFDeal.GetAsync(x => x.Id == irfDeal.Id);
                viewModel.CreateSaleCoopViewModel.IRFDealId = newIRFDeal!.Id;
                viewModel.CreateSaleCoopViewModel.CreateIRFDealSaleCoopViewModel!.CreatedAt = newIRFDeal.CreatedAt;
                viewModel.CreateSaleCoopViewModel.CreateIRFDealSaleCoopViewModel.CreatedBy = newIRFDeal.CreatedBy;
                viewModel.CreateSaleCoopViewModel.CreateIRFDealSaleCoopViewModel.IsActive = newIRFDeal.IsActive;

                // Finalize SaleCoop creation
                viewModel.CreateSaleCoopViewModel.IsActive = true;
                viewModel.CreateSaleCoopViewModel.IsDeleted = false;
                viewModel.CreateSaleCoopViewModel.CreatedBy = user.Id;
                viewModel.CreateSaleCoopViewModel.CreatedAt = DateTime.Now;

                var saleCoopModel = viewModel.CreateSaleCoopViewModel.ToSaleCoopModel();
                await _unitOfWork.SaleCoop.AddAsync(saleCoopModel);
                await _unitOfWork.SaveChangesAsync();

                // Handle uploads (submit only)
                if (actionType.ToLower() == "submit" &&
                    viewModel.CreateSaleCoopViewModel.CreateFileUploadListViewModel?.CreateFileUploadsViewModel != null)
                {
                    var uploadPath = CreateFilePath(saleCoopModel.Id);
                    await CreateFileUploadData(
                        viewModel.CreateSaleCoopViewModel.CreateFileUploadListViewModel.CreateFileUploadsViewModel,
                        saleCoopModel.Id,
                        transactionType.Id,
                        viewModel.CreateSaleCoopViewModel.DocumentTypeList,
                        uploadPath
                    );
                }
                // Notify Deal Admin and Deal Creator (on submit)
                if (viewModel.CreateSaleCoopViewModel.DealStatusId == 1)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var dealLink = Url.Action("Login", "Account", new { id = saleCoopModel.Id, token }, Request.Scheme);
                    var dealAdminEmails = await GetDealAdminEmails();
                    var userRoles = await _userManager.GetRolesAsync(user);

                    // Build recipient lists (avoid duplicate emails/roles)
                    var allRecipientEmails = dealAdminEmails.Select(e => e.Email).ToList();
                    if (!allRecipientEmails.Contains(user.Email, StringComparer.OrdinalIgnoreCase))
                        allRecipientEmails.Add(user.Email);

                    var allRoleNames = dealAdminEmails.Select(e => e.RoleName).ToList();
                    string creatorRole = userRoles.FirstOrDefault() ?? "Deal Creator";
                    if (!allRoleNames.Contains(creatorRole, StringComparer.OrdinalIgnoreCase))
                        allRoleNames.Add(creatorRole);

                    // Generate all individualized mail requests
                    var mailRequests = EmailRequest.SendNotificationWhenDealIsSubmitted(
                        user, allRoleNames, transactionType.Description, allRecipientEmails,
                        saleCoopModel.DealTransactionNumber, dealLink!
                    );

                    // ---- FAIL-SAFE SENDING ----
                    const int maxRetries = 2;
                    const int retryDelayMilliseconds = 2000;
                    var failedEmails = new List<MailRequest>();

                    foreach (var mailRequest in mailRequests)
                    {
                        bool sent = false;
                        Exception? lastException = null;

                        for (int attempt = 1; attempt <= maxRetries; attempt++)
                        {
                            try
                            {
                                await _emailService.SendEmailAsync(mailRequest);
                                sent = true;
                                break; // Success, exit retry loop
                            }
                            catch (Exception ex)
                            {
                                lastException = ex;
                                _logger.LogWarning(ex, $"Attempt {attempt} to send email to {mailRequest.ToEmail} failed.");
                                if (attempt < maxRetries)
                                    await Task.Delay(retryDelayMilliseconds);
                            }
                        }

                        if (!sent)
                        {
                            _logger.LogError(lastException, $"All {maxRetries} attempts to send email to {mailRequest.ToEmail} failed. Email Request: {System.Text.Json.JsonSerializer.Serialize(mailRequest)}");
                            failedEmails.Add(mailRequest);
                            // Optionally: notify admin/support here
                        }
                    }

                    // Optionally: handle or alert on failedEmails here
                    // if (failedEmails.Any()) { /* ... */ }
                }



                await transaction.CommitAsync();
                return RedirectToAction(nameof(Index), new { addSuccess = true });
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "Transaction number was updated by another user. Please try again.");
                return View("Index", viewModel);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        private async Task<List<InternalEmail>> GetDealAdminEmails()
        {
            List<InternalEmail> internalEmails = new List<InternalEmail>();
            var combinedQuery = LinqExtensions.AndAlso<InternalEmail>(x => x.IsDeleted == false, x => x.RoleName == "DealAdmin");
            var emails = await _unitOfWork.InternalEmail.GetAllAsync(combinedQuery);

            return emails.ToList();
        }
        public async Task<IActionResult> SendRequestToDealAdmin(int dealId)
        {
            // to get current user info
            var user = await _userManager.GetUserAsync(User);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user!);

            var dealLink = Url.Action("Login", "Account",
                                                        new { token = token }, Request.Scheme);

            var dealAdminEmails = await GetDealAdminEmails();

            var userRoles = await _userManager.GetRolesAsync(user!);

            var saleCoop = await _unitOfWork.SaleCoop.GetAsync(x => x.Id == dealId, includeProperties: "IRFDeal,TransactionType,DealStatus");
            var transactionType = saleCoop.TransactionType;

            var mailRequest = EmailRequest.SendRequestToEditTheDeal(user!, userRoles, transactionType!.Description, dealAdminEmails, saleCoop.DealTransactionNumber, dealLink!);

            await _emailService.SendEmailAsync(mailRequest);

            return RedirectToAction("Edit", new { id = dealId });
        }
        private string CreateFilePath(int saleCoopId)
        {
            // to get current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // to get current user info
            var user = _userManager.FindByIdAsync(userId!);

            var uploadPath = string.Empty;
            var defaultPathFromConfig = _configurationService.GetDefaultUploadPathFromConfig();
            if (defaultPathFromConfig != null)
            {
                uploadPath = defaultPathFromConfig;
            }
            else
            {
                uploadPath = _backupFileDirectory;
            }
            var userIdPath = user!.Id.ToString() + "\\";

            uploadPath = Path.Combine(uploadPath, userIdPath + saleCoopId + "\\");

            return uploadPath;
        }
        private async Task<bool> CreateFileUploadData(List<CreateFileUploadViewModel> createFileUploadsViewModel,
                                              int saleCoopModelId,
                                              int transactionTypeId,
                                              IEnumerable<DocumentType> documentTypeList,
                                              string uploadPath)
        {

            // to get current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // to get current user info
            var user = await _userManager.FindByIdAsync(userId!);

            string updatedUploadPath = string.Empty;
            FileUpload createFileUpload = new FileUpload();
            int index = 0;
            foreach (var createFileUploadViewModel in createFileUploadsViewModel)
            {
                var createFileUploadDocumentType = documentTypeList.Where(x => x.Id == createFileUploadViewModel.DocumentTypeId).FirstOrDefault();
                if (createFileUploadDocumentType != null)
                {
                    updatedUploadPath = Path.Combine(uploadPath, _transactionTypeName, createFileUploadDocumentType.Description + "\\");
                    createFileUpload.DocumentTypeId = createFileUploadDocumentType.Id;
                }
                else
                {
                    _logger.LogInformation($"Document Type Id not found for: Transaction Type - {transactionTypeId}, Document Type - {createFileUpload.DocumentTypeId}");
                    // Log or handle the case where DocumentType is not found
                    continue; // Skip this iteration
                }

                var files = createFileUploadViewModel.Files;

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var (fileNameWithoutExtension, fileExtension, originalFileName) = await _fileHandlerService.UploadFile(file, updatedUploadPath);
                        createFileUpload = new FileUpload()
                        {
                            FileName = fileNameWithoutExtension,
                            FileExtension = fileExtension,
                            OriginalFileName = originalFileName,
                            Directory = updatedUploadPath,
                            FullPath = updatedUploadPath + fileNameWithoutExtension + fileExtension,
                            FileSize = file.Length.ToString(),
                            IsActive = true,
                            CreatedAt = DateTime.Now,
                            CreatedBy = user!.Id,
                            SaleCoopId = saleCoopModelId,
                            TransactionTypeId = transactionTypeId,
                            DocumentTypeId = createFileUploadDocumentType!.Id

                        };
                        await _unitOfWork.FileUpload.AddAsync(createFileUpload);
                    }
                }
                index++;
            }
            return true;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var model = await _unitOfWork.SaleCoop.GetAsync(x => x.Id == id, includeProperties: "TransactionType,IRFDeal,DealStatus");

            string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;

            if (model == null)
            {
                NotFoundViewModel notFoundViewModel = new NotFoundViewModel();
                notFoundViewModel.ErrorTitle = ($"{controllerName} not found");
                notFoundViewModel.ErrorMessage = ($"{controllerName} with {id} cannot be found!");
                notFoundViewModel.ControllerName = controllerName;
                notFoundViewModel.ControllerAction = "Index";

                return View("NotFound", notFoundViewModel);
            }
            EditSaleCoopWithTabMenuViewModel viewModel = new EditSaleCoopWithTabMenuViewModel();

            // to get current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // to get current user info
            var user = await _userManager.FindByIdAsync(userId!);

            viewModel.EditSaleCoopViewModel = model.ToEditSaleCoopViewModel();

            viewModel.EditSaleCoopViewModel.DealStatus = model.DealStatus;

            viewModel.EditSaleCoopViewModel.UpdatedByFullName = user!.FirstName + " " + user.LastName;
            viewModel.EditSaleCoopViewModel.UpdatedBy = user!.Id;
            viewModel.EditSaleCoopViewModel.UpdatedAt = DateTime.Now;

            var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.SaleCoopId == model.Id, includeProperties: "SaleCoop,TransactionType,DocumentType");

            var fileUploadsWithFiles = FilterFileUploadsWithFilesOnly(fileUploads.ToList());

            viewModel.EditSaleCoopViewModel.FileUploads = fileUploadsWithFiles;

            var documentTypeAssignmentIEnum = await _unitOfWork.DocumentTypeAssignment.GetAllAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,DocumentType");
            var documentTypeIEnum = await _unitOfWork.DocumentType.GetAllAsync(x => x.IsDeleted == false);
            var documentTypeAssignmentList = documentTypeAssignmentIEnum.ToList();

            viewModel.EditSaleCoopViewModel.DocumentTypeAssignmentList = documentTypeAssignmentList.Where(x => x.TransactionType.Id == model.TransactionType.Id).ToList(); ; // DocumentTypeAssignmentList

            var documentTypeList = documentTypeIEnum.ToList();
            viewModel.EditSaleCoopViewModel.DocumentTypeList = _documentTransactionService
                                                            .GetDocumentTypesFromDocumentTypeAssignment(model.TransactionType!.Description,
                                                                                                        documentTypeAssignmentList);
            var dealStatusIEnum = await _unitOfWork.DealStatus.GetAllAsync(x => x.IsActive == true && !x.IsDeleted);
            viewModel.EditSaleCoopViewModel.DealStatusList = dealStatusIEnum.ToList();

            var menus = EditDealTransactionTabMenuData.GetEditDealTransactionTabMenuData();

            string menuName = "IRFDetails1";

            foreach (var menu in menus.Where(x => x.Menu == menuName))
            {
                menu.IsActive = "active";
                menu.IsSelected = "true";
            }

            viewModel.Menus = menus;

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditSaleCoopWithTabMenuViewModel viewModel)
        {
            if (id == 0)
                return NotFoundResultWithMessage("SaleCoop", id);

            var dealStatusList = await _unitOfWork.DealStatus.GetAllAsync(x => x.IsActive && !x.IsDeleted);
            viewModel.EditSaleCoopViewModel.DealStatusList = dealStatusList.ToList();

            var actionType = Request.Form["actionType"];

            if (!ModelState.IsValid)
            {
                if (actionType == "save")
                {
                    // Skip validation errors on Save
                    // Optionally log validation issues for auditing
                }
                else
                {
                    return View(viewModel); // enforce on Submit
                }
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out Guid parsedUserId))
                throw new InvalidOperationException("Invalid user ID - cannot convert to GUID.");

            var user = await _userManager.FindByIdAsync(userId!);

            // Load tracked entity
            var saleCoopModel = await _unitOfWork.SaleCoop.GetAsync(
                x => x.Id == id,
                includeProperties: "IRFDeal,TransactionType,DealStatus",
                tracked: true);

            if (saleCoopModel == null)
                return NotFoundResultWithMessage("SaleCoop", id);

            var transactionType = await _unitOfWork.TransactionType.GetAsync(x => x.Id == saleCoopModel.TransactionTypeId);
            viewModel.EditSaleCoopViewModel.TransactionType = transactionType;
            viewModel.EditSaleCoopViewModel.TransactionTypeId = transactionType.Id;

            var documentTypeAssignments = await _unitOfWork.DocumentTypeAssignment
                .GetAllAsync(x => !x.IsDeleted, includeProperties: "TransactionType,DocumentType", tracked: true);
            var documentTypes = await _unitOfWork.DocumentType.GetAllAsync(x => !x.IsDeleted);

            viewModel.EditSaleCoopViewModel.DocumentTypeList = _documentTransactionService
                .GetDocumentTypesFromDocumentTypeAssignment(transactionType.Description, documentTypeAssignments.ToList());

            saleCoopModel.FileUploads = FilterFileUploadsWithFilesOnly(saleCoopModel.FileUploads);

            // Begin transaction
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Update IRFDeal
                var irfDealDb = await _unitOfWork.IRFDeal.GetAsync(x => x.Id == saleCoopModel.IRFDealId, tracked: true);
                var editedIRFDeal = viewModel.EditSaleCoopViewModel.EditIRFDealSaleCoopViewModel;

                if (irfDealDb != null && editedIRFDeal != null)
                {
                    irfDealDb.UpdateFromViewModel(editedIRFDeal, parsedUserId);
                    irfDealDb.UpdatedAt = DateTime.Now;
                    irfDealDb.UpdatedBy = parsedUserId;
                    await _unitOfWork.IRFDeal.UpdateAsync(irfDealDb);
                }

                // Update SaleCoop
                saleCoopModel.DealStatusId = viewModel.EditSaleCoopViewModel.DealStatusId;
                saleCoopModel.Notes = viewModel.EditSaleCoopViewModel.Notes;
                saleCoopModel.DateSubmitted = DateTime.Now;
                saleCoopModel.IsActive = true;
                saleCoopModel.UpdatedAt = DateTime.Now;
                saleCoopModel.UpdatedBy = parsedUserId;

                // Optional: assign RowVersion for concurrency check
                saleCoopModel.RowVersion = viewModel.EditSaleCoopViewModel.RowVersion;

                await _unitOfWork.SaveChangesAsync(); // commits changes for tracked entities

                // Handle file uploads
                var uploadPath = CreateFilePath(saleCoopModel.Id);
                var fileUploadVM = viewModel.EditSaleCoopViewModel.CreateFileUploadListViewModel?.CreateFileUploadsViewModel;

                if (fileUploadVM != null)
                {
                    await CreateFileUploadData(fileUploadVM, saleCoopModel.Id, transactionType.Id, viewModel.EditSaleCoopViewModel.DocumentTypeList, uploadPath);
                }

                // ===== EMAIL NOTIFICATION (if submitted) =====
                if (actionType == "submit" && saleCoopModel.DealStatusId == 1)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var dealLink = Url.Action("Login", "Account", new { id = saleCoopModel.Id, token }, Request.Scheme);

                    var dealAdminEmails = await GetDealAdminEmails();
                    var userRoles = await _userManager.GetRolesAsync(user);

                    // Build recipient lists (avoid duplicates)
                    var allRecipientEmails = dealAdminEmails.Select(e => e.Email).ToList();
                    if (!allRecipientEmails.Contains(user.Email, StringComparer.OrdinalIgnoreCase))
                        allRecipientEmails.Add(user.Email);

                    var allRoleNames = dealAdminEmails.Select(e => e.RoleName).ToList();
                    string creatorRole = userRoles.FirstOrDefault() ?? "Deal Creator";
                    if (!allRoleNames.Contains(creatorRole, StringComparer.OrdinalIgnoreCase))
                        allRoleNames.Add(creatorRole);

                    // Send personalized email to each recipient
                    var mailRequests = EmailRequest.SendNotificationWhenDealIsSubmitted(
                        user, allRoleNames, transactionType.Description, allRecipientEmails,
                        saleCoopModel.DealTransactionNumber, dealLink!
                    );

                    // ---- FAIL-SAFE SENDING ----
                    const int maxRetries = 2;
                    const int retryDelayMilliseconds = 2000;
                    var failedEmails = new List<MailRequest>();

                    foreach (var mailRequest in mailRequests)
                    {
                        bool sent = false;
                        Exception? lastException = null;

                        for (int attempt = 1; attempt <= maxRetries; attempt++)
                        {
                            try
                            {
                                await _emailService.SendEmailAsync(mailRequest);
                                sent = true;
                                break; // Success, exit retry loop
                            }
                            catch (Exception ex)
                            {
                                lastException = ex;
                                _logger.LogWarning(ex, $"Attempt {attempt} to send email to {mailRequest.ToEmail} failed.");
                                if (attempt < maxRetries)
                                    await Task.Delay(retryDelayMilliseconds);
                            }
                        }

                        if (!sent)
                        {
                            _logger.LogError(lastException, $"All {maxRetries} attempts to send email to {mailRequest.ToEmail} failed. Email Request: {System.Text.Json.JsonSerializer.Serialize(mailRequest)}");
                            failedEmails.Add(mailRequest);
                            // Optionally: notify admin/support here
                        }
                    }

                    // Optionally: handle or alert on failedEmails here
                    // if (failedEmails.Any()) { /* ... */ }
                }



                await transaction.CommitAsync();
                return RedirectToAction(nameof(Index), new { editSuccess = true });
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty, "This record was modified by another user after you started editing. Please reload and try again.");
                return View(viewModel);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private IActionResult NotFoundResultWithMessage(string entityName, int id)
        {
            var viewModel = new NotFoundViewModel
            {
                ErrorTitle = $"{entityName} not found",
                ErrorMessage = $"{entityName} with ID {id} cannot be found!",
                ControllerName = entityName,
                ControllerAction = "Index"
            };
            return View("NotFound", viewModel);
        }
        private List<FileUpload>? FilterFileUploadsWithFilesOnly(List<FileUpload>? fileUploads)
        {
            List<FileUpload> fileUploadsWithFiles = new List<FileUpload>();
            if (fileUploads != null)
            {
                foreach (var fileUpload in fileUploads)
                {
                    var isExists = _fileHandlerService.CheckIfFileExists(fileUpload.FullPath);
                    if (isExists.Result == true)
                    {
                        fileUploadsWithFiles.Add(fileUpload);
                    }
                }
            }

            return fileUploadsWithFiles;

        }
        [HttpGet]
        public async Task<IActionResult> Preview(int id)
        {
            var model = await _unitOfWork.SaleCoop.GetAsync(x => x.Id == id, includeProperties: "IRFDeal,TransactionType,DealStatus");
            if (model == null)
            {
                string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;
                NotFoundViewModel notFoundViewModel = new NotFoundViewModel();
                notFoundViewModel.ErrorTitle = ($"{controllerName} not found");
                notFoundViewModel.ErrorMessage = ($"{controllerName} with {id} cannot be found!");
                notFoundViewModel.ControllerName = controllerName;
                notFoundViewModel.ControllerAction = "Index";

                return View("NotFound", notFoundViewModel);
            }

            var viewModel = model.ToPreviewSaleCoopViewModel();
            var fileUploadIEnum = await _unitOfWork.FileUpload.GetAllAsync(x => x.SaleCoopId == model.Id && x.IsActive == true && !x.IsDeleted);
            viewModel.FileUploads = fileUploadIEnum.ToList();
            var users = _userManager.Users;

            var user = users.Where(x => x.Id == viewModel.CreatedBy).FirstOrDefault();
            if (user != null)
            {
                viewModel.CreatedByFullName = user!.FirstName + " " + user.LastName;
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ListPartialView(SaleCoopListPartialViewModel listViewModel)
        {
            // Get the name of the current controller
            string controllerName = ControllerContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;

            // Activate menus for the current controller
            listViewModel.Menus = ActivateMenus(controllerName);

            // Fetch SaleCoop data with related properties
            var saleCoopData = await _unitOfWork.SaleCoop.GetAllAsync(includeProperties: "IRFDeal,TransactionType,DealStatus");

            // Map data to the view model
            listViewModel.SaleCoopListViewModel = saleCoopData?.ToSaleCoopListViewModel() ?? new List<SaleCoopViewModel>();

            // Render the partial view
            return PartialView("~/Views/SaleCoop/Partial/ListPartial.cshtml", listViewModel);
        }

        // Private method for menu activation logic
        private List<TabMenu> ActivateMenus(string controllerName)
        {
            return AdministrationTabMenuData.GetAdministrationTabMenuData()
                .Select(menu =>
                {
                    menu.IsActive = menu.Menu == controllerName ? "active" : string.Empty;
                    menu.IsSelected = menu.Menu == controllerName ? "true" : "false";
                    return menu;
                })
                .ToList();
        }


        public async Task<IActionResult> DeleteMultipleModal(List<int> ids)
        {
            var modelIEnum = await _unitOfWork.SaleCoop.GetAllAsync(x => ids.Contains(x.Id));

            if (modelIEnum == null)
            {
                string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;
                NotFoundViewModel notFoundViewModel = new NotFoundViewModel();
                notFoundViewModel.ErrorTitle = ($"{controllerName} not found");
                notFoundViewModel.ErrorMessage = ($"{controllerName} cannot be found!");
                notFoundViewModel.ControllerName = controllerName;
                notFoundViewModel.ControllerAction = "Index";

                return View("NotFound", notFoundViewModel);
            }

            var deleteSaleCoopIEnumViewModel = modelIEnum.ToDeleteSaleCoopIEnumViewModel();

            return PartialView("~/Views/SaleCoop/Modal/DeleteMultipleModal.cshtml", deleteSaleCoopIEnumViewModel);
        }

        public async Task<IActionResult> DeleteMultiple(IEnumerable<DeleteSaleCoopViewModel> viewModelList)
        {
            if (viewModelList == null || !viewModelList.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            var ids = viewModelList.Select(x => x.Id).ToList();

            var modelList = await _unitOfWork.SaleCoop.GetAllAsync(x => ids.Contains(x.Id));

            if (modelList != null)
            {
                try
                {
                    await _unitOfWork.SaleCoop.RemoveRangeAsync(modelList);

                    return RedirectToAction(nameof(Index), new { deleteSuccess = true });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SoftDeleteMultiple(IEnumerable<DeleteSaleCoopViewModel> viewModelList)
        {
            if (viewModelList == null || !viewModelList.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            var ids = viewModelList.Select(x => x.Id).ToList();

            var modelList = await _unitOfWork.SaleCoop.GetAllAsync(x => ids.Contains(x.Id));

            if (modelList != null)
            {
                // to get current user ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // to get current user info
                var user = await _userManager.FindByIdAsync(userId!);

                try
                {
                    foreach (var item in modelList)
                    {
                        item.IsDeleted = true;
                        item.DeletedBy = user!.Id;
                        item.DeletedAt = DateTime.Now;
                    }
                    await _unitOfWork.SaleCoop.UpdateRangeAsync(modelList);

                    return RedirectToAction(nameof(Index), new { deleteSuccess = true });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Download(int id)
        {
            var fileUpload = await _unitOfWork.FileUpload.GetAsync(x => x.Id == id);
            if (fileUpload != null)
            {
                var result = await _fileHandlerService.DownloadFile(fileUpload.Directory, fileUpload.FileName, fileUpload.FileExtension);
                return File(result.Item1, result.Item2, result.Item3);
            }

            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> DeleteFile(int id)
        {
            // to get current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // to get current user info
            var user = await _userManager.FindByIdAsync(userId!);

            var fileUpload = await _unitOfWork.FileUpload.GetAsync(x => x.Id == id);
            if (fileUpload != null)
            {
                fileUpload.IsDeleted = true;
                fileUpload.DeletedAt = DateTime.UtcNow;
                fileUpload.DeletedBy = user.Id;

                await _unitOfWork.FileUpload.UpdateAsync(fileUpload);

                var isSuccess = await _fileHandlerService.DeleteFile(fileUpload.FullPath);

                return RedirectToAction(nameof(Edit), new { id = fileUpload.SaleCoopId });
            }

            return NotFound();
        }
        public async Task<IActionResult> PreviewFile(int id)
        {
            var fileUpload = await _unitOfWork.FileUpload.GetAsync(x => x.Id == id);

            if (fileUpload != null)
            {
                FileStream fs = new FileStream(fileUpload.FullPath, FileMode.Open, FileAccess.Read);
                return File(fs, "application/pdf");

            }
            return NotFound();
        }
        public async Task<JsonResult> GetFileDetails(int id)
        {
            var record = JsonConvert.SerializeObject(await _unitOfWork.FileUpload.GetAsync(x => x.Id == id));
            return Json(record);
        }

        [HttpGet]
        public async Task<JsonResult> GetDocumentTypesThatHasFileUploads(int saleCoopId)
        {
            var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(
                x => x.SaleCoopId == saleCoopId
                && x.IsActive && !x.IsDeleted
            );
            var documentTypeIds = fileUploads
                .Select(x => x.DocumentTypeId)
                .Distinct()
                .ToArray();

            return Json(documentTypeIds);
        }


        [HttpPost]
        public async Task<IActionResult> GetSaleCoopList([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            // Default sorting
            var orderCriteria = "Id";
            var orderAscendingDirection = true;
            (orderCriteria, orderAscendingDirection) = InitializeOrderCriteria(dtParameters, orderCriteria, orderAscendingDirection);

            // Get current user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            // Get user roles
            var isAdmin = await _userManager.IsInRoleAsync(user!, "Admin");
            var isSuperAdmin = await _userManager.IsInRoleAsync(user!, "SuperAdmin");
            var isAgent = await _userManager.IsInRoleAsync(user!, "Agent");
            var isDealAdmin = await _userManager.IsInRoleAsync(user!, "DealAdmin");
            var isTeamLeader = await _userManager.IsInRoleAsync(user!, "TeamLeader");
            var isSuperUser = await _userManager.IsInRoleAsync(user!, "SuperUser");

            // Initialize query
            var result = await _unitOfWork.SaleCoop.AsQueryableAsync(
                x => !x.IsDeleted,
                includeProperties: "IRFDeal,TransactionType,DealStatus"
            );

            // 🔹 Apply role-based filtering
            if (!(isAdmin || isSuperAdmin || isSuperUser))
            {
                if (isAgent)
                {
                    result = result.Where(x => x.CreatedBy == user!.Id && x.DealStatus!.Id == 2); // Saved
                }
                else if (isDealAdmin)
                {
                    result = result.Where(x => x.DealStatus!.Id == 1); // Submitted
                }
                else if (isTeamLeader)
                {
                    // Get team members of the TeamLeader
                    var teamMemberIds = await _userManager.Users
                        .Where(u => u.TeamLeaderId == user!.Id)
                        .Select(u => u.Id)
                        .ToListAsync();

                    if (teamMemberIds.Any())
                    {
                        result = result.Where(x => teamMemberIds.Contains(x.CreatedBy));
                    }
                }
            }

            var totalResultsCount = result.Count();

            // 🔹 Apply search filtering
            result = searchBy!.Contains(":") || searchBy.Contains("+")
                ? FilterSearchWithParameters(result, searchBy)
                : DefaultFilterSearch(result, searchBy);

            // 🔹 Apply ordering
            result = orderAscendingDirection
                ? result.OrderByDynamic(orderCriteria!, DtOrderDir.Asc)
                : result.OrderByDynamic(orderCriteria!, DtOrderDir.Desc);

            var filteredResultsCount = result.Count();

            // Convert results to ViewModel
            var resultList = result.ToSaleCoopListViewModel();

            // Optimize user lookup
            var usersDictionary = await _userManager.Users
                .ToDictionaryAsync(u => u.Id);

            foreach (var item in resultList)
            {
                if (usersDictionary.TryGetValue(item.CreatedBy, out var createdByUser))
                {
                    item.CreatedByFullName = $"{createdByUser.FirstName} {createdByUser.LastName}";
                }
            }

            // 🔹 Prepare JSON response for DataTables
            var jsonResult = new DtResult<SaleCoop>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = await result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToListAsync()
            };

            return Json(jsonResult);
        }

        private IQueryable<SaleCoop> DefaultFilterSearch(IQueryable<SaleCoop> result, string searchBy)
        {
            result = result.Where(r => r.DealTransactionNumber != null && r.DealTransactionNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.CreatedByFullName != null && r.CreatedByFullName.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.UpdatedByFullName != null && r.UpdatedByFullName.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.DeletedByFullName != null && r.DeletedByFullName.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.TransactionType != null && r.TransactionType.Description.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.PropertyAddress! != null && r.IRFDeal.PropertyAddress.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.BuyerName! != null && r.IRFDeal.BuyerName.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.LandLordName! != null && r.IRFDeal.LandLordName.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.ListingAgentName! != null && r.IRFDeal.ListingAgentName.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.ListingBrokerage! != null && r.IRFDeal.ListingBrokerage.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.ListingBrokerageFax! != null && r.IRFDeal.ListingBrokerageFax.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.BuyerAgentName! != null && r.IRFDeal.BuyerAgentName.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.BuyerBrokerage! != null && r.IRFDeal.BuyerBrokerage.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.BuyerBrokerageFax! != null && r.IRFDeal.BuyerBrokerageFax.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.SellerLawyer! != null && r.IRFDeal.SellerLawyer.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.SellerLawyerAddress! != null && r.IRFDeal.SellerLawyerAddress.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.SellerEmail! != null && r.IRFDeal.SellerEmail.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.BuyerLawyer! != null && r.IRFDeal.BuyerLawyer.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.BuyerLawyerAddress! != null && r.IRFDeal.BuyerLawyerAddress.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.BuyerEmail! != null && r.IRFDeal.BuyerEmail.ToUpper().Contains(searchBy.ToUpper())
                   );
            return result;
        }
        private IQueryable<SaleCoop> FilterSearchWithParameters(IQueryable<SaleCoop> query, string searchBy)
        {
            var filters = searchBy.Split('+'); // Split the input by '+'

            foreach (var filter in filters)
            {
                var keyValue = filter.Split(':'); // Split each filter by ':'
                if (keyValue.Length != 2) continue;

                var key = keyValue[0].Trim().ToLower();
                var value = keyValue[1].Trim().ToUpper();

                switch (key)
                {
                    case "createdby":
                        query = query.Where(r => r.CreatedByFullName != null && r.CreatedByFullName.ToUpper().Contains(value));
                        break;
                    case "updatedby":
                        query = query.Where(r => r.UpdatedByFullName != null && r.UpdatedByFullName.ToUpper().Contains(value));
                        break;
                    case "deletedby":
                        query = query.Where(r => r.DeletedByFullName != null && r.DeletedByFullName.ToUpper().Contains(value));
                        break;
                    case "transactiontype":
                        query = query.Where(r => r.TransactionType != null && r.TransactionType.Description.ToUpper().Contains(value));
                        break;
                    case "propertyaddress":
                        query = query.Where(r => r.IRFDeal!.PropertyAddress != null && r.IRFDeal!.PropertyAddress.ToUpper().Contains(value));
                        break;
                    case "BuyerName":
                        query = query.Where(r => r.IRFDeal!.BuyerName != null && r.IRFDeal!.BuyerName.ToUpper().Contains(value));
                        break;
                    case "LandLordName":
                        query = query.Where(r => r.IRFDeal!.LandLordName != null && r.IRFDeal!.LandLordName.ToUpper().Contains(value));
                        break;
                    case "ListingAgentName":
                        query = query.Where(r => r.IRFDeal!.ListingAgentName != null && r.IRFDeal!.ListingAgentName.ToUpper().Contains(value));
                        break;
                    case "ListingBrokerage":
                        query = query.Where(r => r.IRFDeal!.ListingBrokerage != null && r.IRFDeal!.ListingBrokerage.ToUpper().Contains(value));
                        break;
                    case "ListingBrokerageFax":
                        query = query.Where(r => r.IRFDeal!.ListingBrokerageFax != null && r.IRFDeal!.ListingBrokerageFax.ToUpper().Contains(value));
                        break;
                    case "BuyerAgentName":
                        query = query.Where(r => r.IRFDeal!.BuyerAgentName != null && r.IRFDeal!.BuyerAgentName.ToUpper().Contains(value));
                        break;
                    case "BuyerBrokerage":
                        query = query.Where(r => r.IRFDeal!.BuyerBrokerage != null && r.IRFDeal!.BuyerBrokerage.ToUpper().Contains(value));
                        break;
                    case "BuyerBrokerageFax":
                        query = query.Where(r => r.IRFDeal!.BuyerBrokerageFax != null && r.IRFDeal!.BuyerBrokerageFax.ToUpper().Contains(value));
                        break;
                    case "SellerLawyer":
                        query = query.Where(r => r.IRFDeal!.SellerLawyer != null && r.IRFDeal!.SellerLawyer.ToUpper().Contains(value));
                        break;
                    case "SellerLawyerAddress":
                        query = query.Where(r => r.IRFDeal!.SellerLawyerAddress != null && r.IRFDeal!.SellerLawyerAddress.ToUpper().Contains(value));
                        break;
                    case "SellerPhoneNumber":
                        query = query.Where(r => r.IRFDeal!.SellerEmail != null && r.IRFDeal!.SellerEmail.ToUpper().Contains(value));
                        break;
                    case "BuyerLawyer":
                        query = query.Where(r => r.IRFDeal!.BuyerLawyer != null && r.IRFDeal!.BuyerLawyer.ToUpper().Contains(value));
                        break;
                    case "BuyerLawyerAddress":
                        query = query.Where(r => r.IRFDeal!.BuyerLawyerAddress != null && r.IRFDeal!.BuyerLawyerAddress.ToUpper().Contains(value));
                        break;
                    case "BuyerPhoneNumber":
                        query = query.Where(r => r.IRFDeal!.BuyerEmail != null && r.IRFDeal!.BuyerEmail.ToUpper().Contains(value));
                        break;
                    default:
                        // If there's an unrecognized filter key, you can choose to ignore it or handle it as needed
                        break;
                }
            }

            return query;
        }

        private (string, bool) InitializeOrderCriteria(DtParameters dtParameters, string orderCriteria, bool orderAscendingDirection)
        {
            var orderCriteriaTemp = "Id";
            var orderAscendingDirectionTemp = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                try
                {
                    orderCriteriaTemp = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderAscendingDirectionTemp = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
                }
                catch (Exception ex)
                {
                    try
                    {
                        orderCriteriaTemp = dtParameters.Columns[dtParameters.Order[1].Column].Data;
                        orderAscendingDirectionTemp = dtParameters.Order[1].Dir.ToString().ToLower() == "asc";
                    }
                    catch (Exception ex1)
                    {
                        orderCriteriaTemp = "Id";
                        orderAscendingDirectionTemp = true;
                    }
                }
            }

            if (orderCriteriaTemp == null)
            {
                orderCriteriaTemp = "Id";
                orderAscendingDirectionTemp = true;
            }

            if (orderCriteriaTemp != null)
            {
                orderCriteria = orderCriteriaTemp;
                orderAscendingDirection = orderAscendingDirectionTemp;
            }

            return (orderCriteria, orderAscendingDirection);
        }

        [HttpPost]
        public async Task<IActionResult> ExportTable([FromQuery] string format, [FromForm] string dtParametersJson)
        {
            var dtParameters = new DtParameters();
            if (!string.IsNullOrEmpty(dtParametersJson))
            {
                dtParameters = JsonConvert.DeserializeObject<DtParameters>(dtParametersJson);
            }
            if (dtParameters != default)
            {
                var searchBy = dtParameters.Search?.Value;

                var orderCriteria = "Id";
                var orderAscendingDirection = true;

                (orderCriteria, orderAscendingDirection) = InitializeOrderCriteria(dtParameters, orderCriteria, orderAscendingDirection);

                var result = _unitOfWork.SaleCoop.AsQueryable(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");

                if (!string.IsNullOrEmpty(searchBy))
                {
                    if (searchBy.Contains(":") || searchBy.Contains("+"))
                    {
                        result = FilterSearchWithParameters(result, searchBy);
                    }
                    else
                    {
                        result = DefaultFilterSearch(result, searchBy);
                    };

                }

                result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

                var resultList = await result.ToListAsync();

                var saleCoopViewModel = resultList.ToSaleCoopListViewModel();

                var resultListForExport = saleCoopViewModel.ToSaleCoopListExport();

                switch (format)
                {
                    case ExportFormat.Excel:
                        return File(
                            await _exportService.ExportToExcel(resultListForExport),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "data.xlsx");

                    case ExportFormat.Csv:
                        return File(_exportService.ExportToCsv(resultListForExport),
                            "application/csv",
                            "data.csv");

                    case ExportFormat.Html:
                        return File(_exportService.ExportToHtml(resultListForExport),
                            "application/csv",
                            "data.html");

                    case ExportFormat.Json:
                        return File(_exportService.ExportToJson(resultListForExport),
                            "application/json",
                            "data.json");

                    case ExportFormat.Xml:
                        return File(_exportService.ExportToXml(resultListForExport),
                            "application/xml",
                            "data.xml");

                    case ExportFormat.Yaml:
                        return File(_exportService.ExportToYaml(resultListForExport),
                            "application/yaml",
                            "data.yaml");
                }
            }
            return null!;
        }
    }
}
