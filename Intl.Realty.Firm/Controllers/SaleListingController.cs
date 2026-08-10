using Intl.Realty.Firm.Helper;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.AdditionalDocumentVM;
using Intl.Realty.Firm.Model.ViewModel.FileUploadVM;
using Intl.Realty.Firm.Model.ViewModel.SaleListingVM;
using Intl.Realty.Firm.Models;
using Intl.Realty.Firm.Models.Auxiliary;
using Intl.Realty.Firm.Models.DataTable;
using Intl.Realty.Firm.Models.Enum;
using Intl.Realty.Firm.Repository.IRepository;
using Intl.Realty.Firm.Service;
using Intl.Realty.Firm.Service.IService;
using Intl.Realty.Firm.Utilities;
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
    public class SaleListingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SaleListingController> _logger;
        private readonly IExportService<SaleListingExport> _exportService;
        private readonly IConfigurationService _configurationService;
        private readonly IFileHandlerService _fileHandlerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDocumentTransactionService _documentTransactionService;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        private readonly IDealTransactionNumberService _numberService;
        private readonly string _backupFileDirectory = "\\wwwroot\\Files\\Uploads\\";
        private readonly string _transactionTypeName = "SaleListing";
        public SaleListingController(IUnitOfWork unitOfWork,
                                    ILogger<SaleListingController> logger,
                                    IExportService<SaleListingExport> exportService,
                                    IConfigurationService configurationService,
                                    IFileHandlerService fileHandlerService,
                                    UserManager<ApplicationUser> userManager,
                                    IDocumentTransactionService documentTransactionService,
                                    INotificationService notificationService,
                                    IEmailService emailService,
                                    IDealTransactionNumberService numberService
                                                                        )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _exportService = exportService;
            _configurationService = configurationService;
            _fileHandlerService = fileHandlerService;
            _userManager = userManager;
            _documentTransactionService = documentTransactionService;
            _notificationService = notificationService;
            _emailService = emailService;
            _numberService = numberService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateSaleListingWithTabMenuViewModel viewModel = new CreateSaleListingWithTabMenuViewModel();

            // to get current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // to get current user info
            var user = await _userManager.FindByIdAsync(userId!);

            viewModel.CreateSaleListingViewModel.CreatedByFullName = user!.FirstName + " " + user.LastName;

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
            viewModel.CreateSaleListingViewModel.TransactionType = transactionType;
            viewModel.CreateSaleListingViewModel.TransactionTypeId = transactionType.Id;

            var paddedTransactionType = transactionType.Id.ToString().PadLeft(2, '0');

            var currentYear = DateTime.Now.Year.ToString();
            var currentMonth = DateTime.Now.Month.ToString().PadLeft(2, '0');
            var currentDay = DateTime.Now.Day.ToString().PadLeft(2, '0');
            var paddedCurrentNumber = (numberRange.CurrentNumber + 1).ToString().PadLeft(5, '0');
            var paddedNumberRange = currentYear + currentMonth + currentDay + "-" + paddedTransactionType + "-" + paddedCurrentNumber;

            viewModel.CreateSaleListingViewModel.DealTransactionNumber = paddedNumberRange;

            var documentTypeAssignmentIEnum = await _unitOfWork.DocumentTypeAssignment.GetAllAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,DocumentType");
            var documentTypeIEnum = await _unitOfWork.DocumentType.GetAllAsync(x => x.IsDeleted == false);
            var documentTypeAssignmentList = documentTypeAssignmentIEnum.ToList();

            viewModel.CreateSaleListingViewModel.DocumentTypeAssignmentList = documentTypeAssignmentList.Where(x => x.TransactionType.Id == transactionType.Id).ToList(); ; // DocumentTypeAssignmentList

            var documentTypeList = documentTypeIEnum.ToList();
            viewModel.CreateSaleListingViewModel.DocumentTypeList = _documentTransactionService
                                                            .GetDocumentTypesFromDocumentTypeAssignment(transactionTypeName,
                                                                                                        documentTypeAssignmentList);

            viewModel.CreateSaleListingViewModel.SignageIEnum = SelectListConverter.CreateSelectListFromEnum<Signage>();
            viewModel.CreateSaleListingViewModel.DefaultSignage = 1;
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
        public async Task<IActionResult> Create(CreateSaleListingWithTabMenuViewModel viewModel, string actionType)
        {
            actionType = TransactionHelper.NormalizeActionType(actionType);

            if (viewModel?.CreateSaleListingViewModel == null)
            {
                _logger.LogError("Create SaleListing POST received null viewModel or CreateSaleListingViewModel.");
                return BadRequest("Invalid form submission.");
            }

            viewModel.CreateSaleListingViewModel.DealStatusId = TransactionHelper.IsSubmitAction(actionType) ? 1 : 2;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out Guid parsedUserId))
                throw new InvalidOperationException("Invalid user ID - cannot convert to GUID.");

            var user = await _userManager.FindByIdAsync(userId!);
            if (user == null)
            {
                _logger.LogError("Create SaleListing POST: current user {UserId} was not found.", userId);
                return Unauthorized();
            }

            string transactionTypeName = _transactionTypeName;

            var transactionType = await _unitOfWork.TransactionType.GetAsync(x => x.Description == transactionTypeName);
            if (transactionType == null)
            {
                ModelState.AddModelError(string.Empty, "Transaction type not found.");
                await PopulateCreateSaleListingViewModelAsync(viewModel, user);
                return View(viewModel);
            }

            viewModel.CreateSaleListingViewModel.TransactionType = transactionType;
            viewModel.CreateSaleListingViewModel.TransactionTypeId = transactionType.Id;
            viewModel.CreateSaleListingViewModel.CreatedByFullName = $"{user.FirstName} {user.LastName}";

            await PopulateCreateSaleListingViewModelAsync(viewModel, user);

            // submit = enforce validation
            // save = allow draft behavior
            if (!ModelState.IsValid && !TransactionHelper.IsSaveAction(actionType))
            {
                return View(viewModel);
            }

            // Validate additional documents only on submit
            if (!TransactionHelper.IsSaveAction(actionType))
            {
                var additionalDocumentErrors = ValidateAdditionalDocuments(viewModel.CreateSaleListingViewModel.AdditionalDocuments);
                if (additionalDocumentErrors.Any())
                {
                    foreach (var error in additionalDocumentErrors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }

                    await PopulateCreateSaleListingViewModelAsync(viewModel, user);
                    return View(viewModel);
                }
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                int nextNumber = await _numberService.GetNextTransactionNumberAsync(DateTime.Now.Year);

                viewModel.CreateSaleListingViewModel.DealTransactionNumber =
                    TransactionHelper.BuildDealTransactionNumber(
                        viewModel.CreateSaleListingViewModel.TransactionTypeId,
                        nextNumber);

                // Create IRF Deal
                var irfDealModel = viewModel.CreateSaleListingViewModel.CreateIRFDealSaleListingViewModel?.ToIRFDealModel();

                if (irfDealModel == null)
                {
                    _logger.LogError("Create SaleListing POST: IRF Deal model was null.");
                    ModelState.AddModelError(string.Empty, "IRF Deal details are required.");
                    await PopulateCreateSaleListingViewModelAsync(viewModel, user);
                    await transaction.RollbackAsync();
                    return View(viewModel);
                }

                irfDealModel.CreatedAt = DateTime.Now;
                irfDealModel.CreatedBy = parsedUserId;
                irfDealModel.IsActive = true;
                irfDealModel.IsDeleted = false;

                await _unitOfWork.IRFDeal.AddAsync(irfDealModel);
                await _unitOfWork.SaveChangesAsync();

                var newIRFDeal = await _unitOfWork.IRFDeal.GetAsync(x => x.Id == irfDealModel.Id);
                if (newIRFDeal == null)
                {
                    throw new InvalidOperationException("IRF Deal was not created successfully.");
                }

                // Set SaleListing fields
                viewModel.CreateSaleListingViewModel.IRFDealId = newIRFDeal.Id;
                viewModel.CreateSaleListingViewModel.IsActive = true;
                viewModel.CreateSaleListingViewModel.IsDeleted = false;
                viewModel.CreateSaleListingViewModel.CreatedBy = parsedUserId;
                viewModel.CreateSaleListingViewModel.CreatedAt = DateTime.Now;

                var saleListingModel = viewModel.CreateSaleListingViewModel.ToSaleListingModel();

                await _unitOfWork.SaleListing.AddAsync(saleListingModel);
                await _unitOfWork.SaveChangesAsync();

                var uploadPath = await CreateFilePathAsync(saleListingModel.Id);

                // Standard document uploads
                var fileUploadVM = viewModel.CreateSaleListingViewModel
                    .CreateFileUploadListViewModel?
                    .CreateFileUploadsViewModel;

                if (fileUploadVM != null)
                {
                    _logger.LogInformation("Upload request received for SaleListing {SaleListingId}", saleListingModel.Id);
                    await CreateFileUploadData(
                        fileUploadVM,
                        saleListingModel.Id,
                        transactionType.Id,
                        viewModel.CreateSaleListingViewModel.DocumentTypeList ?? Enumerable.Empty<DocumentType>(),
                        uploadPath
                    );
                }

                // Additional documents
                var additionalDocumentVM = viewModel.CreateSaleListingViewModel.AdditionalDocuments;
                if (additionalDocumentVM != null && additionalDocumentVM.Any())
                {
                    await CreateAdditionalDocumentData(
                        additionalDocumentVM,
                        saleListingModel.Id,
                        transactionType.Id,
                        uploadPath
                    );
                }

                // Email notification on submit
                if (viewModel.CreateSaleListingViewModel.DealStatusId == 1)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var dealLink = Url.Action("Login", "Account", new { id = saleListingModel.Id, token }, Request.Scheme);

                    var dealAdminEmails = await GetDealAdminEmails();
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var allRecipientEmails = dealAdminEmails.Select(e => e.Email).ToList();
                    if (!string.IsNullOrWhiteSpace(user.Email) &&
                        !allRecipientEmails.Contains(user.Email, StringComparer.OrdinalIgnoreCase))
                    {
                        allRecipientEmails.Add(user.Email);
                    }

                    var allRoleNames = dealAdminEmails.Select(e => e.RoleName).ToList();
                    string creatorRole = userRoles.FirstOrDefault() ?? "Deal Creator";
                    if (!allRoleNames.Contains(creatorRole, StringComparer.OrdinalIgnoreCase))
                    {
                        allRoleNames.Add(creatorRole);
                    }

                    var mailRequests = EmailRequest.SendNotificationWhenDealIsSubmitted(
                        user,
                        allRoleNames,
                        transactionType.Description,
                        allRecipientEmails,
                        saleListingModel.DealTransactionNumber,
                        dealLink!
                    );

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
                                break;
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
                            _logger.LogError(
                                lastException,
                                $"All {maxRetries} attempts to send email to {mailRequest.ToEmail} failed. Email Request: {System.Text.Json.JsonSerializer.Serialize(mailRequest)}");

                            failedEmails.Add(mailRequest);
                        }
                    }
                }

                await transaction.CommitAsync();
                return RedirectToAction(nameof(Index), new { addSuccess = true });
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty, "Transaction number was updated by another user. Please try again.");
                await PopulateCreateSaleListingViewModelAsync(viewModel, user);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Create SaleListing failed for user {UserId}", userId);
                throw;
            }
        }
        private async Task PopulateCreateSaleListingViewModelAsync(
            CreateSaleListingWithTabMenuViewModel viewModel,
            ApplicationUser user)
        {
            string transactionTypeName = _transactionTypeName;

            var transactionType = await _unitOfWork.TransactionType.GetAsync(x => x.Description == transactionTypeName);
            if (transactionType == null)
                return;

            viewModel.CreateSaleListingViewModel.TransactionType = transactionType;
            viewModel.CreateSaleListingViewModel.TransactionTypeId = transactionType.Id;
            viewModel.CreateSaleListingViewModel.CreatedByFullName = $"{user.FirstName} {user.LastName}";

            var yearValidation = LinqExtensions.AndAlso<DealTransactionNumberRange>(
                x => x.IsDeleted == false,
                x => x.Year == DateTime.Now.Year);

            var numberRange = await _unitOfWork.DealTransactionNumberRange.GetAsync(yearValidation);
            if (numberRange != null)
            {
                var paddedTransactionType = transactionType.Id.ToString().PadLeft(2, '0');
                var currentYear = DateTime.Now.Year.ToString();
                var currentMonth = DateTime.Now.Month.ToString().PadLeft(2, '0');
                var currentDay = DateTime.Now.Day.ToString().PadLeft(2, '0');
                var paddedCurrentNumber = (numberRange.CurrentNumber + 1).ToString().PadLeft(5, '0');

                viewModel.CreateSaleListingViewModel.DealTransactionNumber =
                    currentYear + currentMonth + currentDay + "-" + paddedTransactionType + "-" + paddedCurrentNumber;
            }

            var documentTypeAssignmentIEnum = await _unitOfWork.DocumentTypeAssignment
                .GetAllAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,DocumentType");

            var documentTypeAssignmentList = documentTypeAssignmentIEnum.ToList();

            viewModel.CreateSaleListingViewModel.DocumentTypeAssignmentList =
                documentTypeAssignmentList.Where(x => x.TransactionType.Id == transactionType.Id).ToList();

            viewModel.CreateSaleListingViewModel.DocumentTypeList = _documentTransactionService
                .GetDocumentTypesFromDocumentTypeAssignment(
                    transactionTypeName,
                    documentTypeAssignmentList);

            viewModel.CreateSaleListingViewModel.SignageIEnum =
                SelectListConverter.CreateSelectListFromEnum<Signage>();

            viewModel.CreateSaleListingViewModel.DefaultSignage = 1;

            var menus = CreateDealTransactionTabMenuData.GetCreateDealTransactionTabMenuData();
            string controllerName = "IRFDetails1";

            foreach (var menu in menus.Where(x => x.Menu == controllerName))
            {
                menu.IsActive = "active";
                menu.IsSelected = "true";
            }

            viewModel.Menus = menus;
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

            var saleListing = await _unitOfWork.SaleListing.GetAsync(x => x.Id == dealId, includeProperties: "IRFDeal,TransactionType,DealStatus");
            var transactionType = saleListing.TransactionType;

            var mailRequest = EmailRequest.SendRequestToEditTheDeal(user!, userRoles, transactionType!.Description, dealAdminEmails, saleListing.DealTransactionNumber, dealLink!);

            await _emailService.SendEmailAsync(mailRequest);

            return RedirectToAction("Edit", new { id = dealId });
        }
        private async Task<string> CreateFilePathAsync(int saleListingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidOperationException("User is not authenticated.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("Current user could not be found.");

            var defaultPathFromConfig = _configurationService.GetDefaultUploadPathFromConfig();
            var uploadPath = !string.IsNullOrWhiteSpace(defaultPathFromConfig)
                ? defaultPathFromConfig
                : _backupFileDirectory;

            uploadPath = Path.Combine(uploadPath, user.Id.ToString(), saleListingId.ToString());
            TransactionHelper.EnsureDirectoryExists(uploadPath);
            return uploadPath;
        }
        private async Task<bool> CreateFileUploadData(
                                List<CreateFileUploadViewModel> createFileUploadsViewModel,
                                int saleListingModelId,
                                int transactionTypeId,
                                IEnumerable<DocumentType> documentTypeList,
                                string uploadPath)
        {
            _logger.LogInformation("Upload request received for SaleListing {SaleListingId}, TransactionTypeId={TransactionTypeId}",
                saleListingModelId, transactionTypeId);

            if (createFileUploadsViewModel == null ||
                !createFileUploadsViewModel.Any(vm => TransactionHelper.HasUploadableFiles(vm?.Files)))
            {
                _logger.LogWarning("No files submitted for SaleListing {SaleListingId}", saleListingModelId);
                return true;
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);
            if (user == null)
            {
                _logger.LogError("CreateFileUploadData: current user {UserId} was not found.", userId);
                return false;
            }

            int index = 0;

            foreach (var createFileUploadViewModel in createFileUploadsViewModel)
            {
                if (createFileUploadViewModel == null)
                    continue;

                var docType = documentTypeList.FirstOrDefault(x => x.Id == createFileUploadViewModel.DocumentTypeId);
                if (docType == null)
                {
                    _logger.LogWarning(
                        "[UPLOAD] Skipped: DocumentTypeId={DocumentTypeId} not found for TransactionTypeId={TransactionTypeId}",
                        createFileUploadViewModel.DocumentTypeId, transactionTypeId);
                    continue;
                }

                var docTypeFolder = TransactionHelper.SanitizePathSegment(docType.Description);
                string currentUploadPath = Path.Combine(uploadPath, _transactionTypeName, docTypeFolder);
                TransactionHelper.EnsureDirectoryExists(currentUploadPath);

                var files = createFileUploadViewModel.Files;
                if (!TransactionHelper.HasUploadableFiles(files))
                {
                    _logger.LogInformation(
                        "[UPLOAD] Index={Index}, No files to save for DocumentTypeId={DocumentTypeId} ('{Description}')",
                        index, docType.Id, docType.Description);
                    index++;
                    continue;
                }

                foreach (var file in files!)
                {
                    if (file == null || file.Length <= 0)
                        continue;

                    try
                    {
                        _logger.LogInformation(
                            "[UPLOAD] Index={Index}, Saving File='{FileName}', For DocumentTypeId={DocumentTypeId} ('{Description}')",
                            index, file.FileName, docType.Id, docType.Description);

                        var (fileNameWithoutExtension, fileExtension, originalFileName) =
                            await _fileHandlerService.UploadFile(file, currentUploadPath);

                        if (string.IsNullOrWhiteSpace(fileNameWithoutExtension))
                        {
                            _logger.LogWarning("Upload returned empty file name for '{FileName}'", file.FileName);
                            continue;
                        }

                        var createFileUpload = new FileUpload
                        {
                            FileName = fileNameWithoutExtension,
                            FileExtension = fileExtension,
                            OriginalFileName = originalFileName,
                            Directory = currentUploadPath,
                            FullPath = Path.Combine(currentUploadPath, fileNameWithoutExtension + fileExtension),
                            FileSize = file.Length.ToString(),
                            IsActive = true,
                            CreatedAt = DateTime.Now,
                            CreatedBy = user.Id,
                            SaleListingId = saleListingModelId,
                            TransactionTypeId = transactionTypeId,
                            DocumentTypeId = docType.Id
                        };

                        await _unitOfWork.FileUpload.AddAsync(createFileUpload);
                        _logger.LogInformation(
                            "File uploaded successfully: {OriginalFileName} -> {StoredFileName}{Extension}",
                            originalFileName, fileNameWithoutExtension, fileExtension);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Upload failed for file {FileName} into directory {Directory}",
                            file.FileName, currentUploadPath);
                        throw;
                    }
                }
                index++;
            }

            return true;
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return NotFoundResultWithMessage("SaleListing", 0);
                }

                var model = await _unitOfWork.SaleListing.GetAsync(
                    x => x.Id == id,
                    includeProperties: "TransactionType,IRFDeal,DealStatus");

                string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;

                if (model == null)
                {
                    NotFoundViewModel notFoundViewModel = new NotFoundViewModel
                    {
                        ErrorTitle = $"{controllerName} not found",
                        ErrorMessage = $"{controllerName} with {id} cannot be found!",
                        ControllerName = controllerName,
                        ControllerAction = "Index"
                    };

                    return View("NotFound", notFoundViewModel);
                }

                EditSaleListingWithTabMenuViewModel viewModel = new EditSaleListingWithTabMenuViewModel();

                // Get current user
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId!);

                viewModel.EditSaleListingViewModel = model.ToEditSaleListingViewModel();
                viewModel.EditSaleListingViewModel.DealStatus = model.DealStatus;
                viewModel.EditSaleListingViewModel.UpdatedByFullName = $"{user!.FirstName} {user.LastName}";
                viewModel.EditSaleListingViewModel.UpdatedBy = user.Id;
                viewModel.EditSaleListingViewModel.UpdatedAt = DateTime.Now;

                // Load predefined uploaded files
                var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(
                    x => x.SaleListingId == model.Id && x.IsActive && !x.IsDeleted,
                    includeProperties: "SaleListing,TransactionType,DocumentType");

                var fileUploadsWithFiles = FilterFileUploadsWithFilesOnly(fileUploads.ToList());

                // FIX: assign filtered files, not raw files
                viewModel.EditSaleListingViewModel.FileUploads = fileUploadsWithFiles;

                // Load saved additional documents
                var additionalDocuments = await _unitOfWork.AdditionalDocument.GetAllAsync(
                    x => x.SaleListingId == model.Id && x.IsActive && !x.IsDeleted,
                    includeProperties: "SaleListing,TransactionType");

                viewModel.EditSaleListingViewModel.SavedAdditionalDocuments = additionalDocuments.ToList();

                // Load document type assignments
                var documentTypeAssignmentIEnum = await _unitOfWork.DocumentTypeAssignment.GetAllAsync(
                    x => x.IsDeleted == false,
                    includeProperties: "TransactionType,DocumentType");

                var documentTypeIEnum = await _unitOfWork.DocumentType.GetAllAsync(
                    x => x.IsActive && x.IsDeleted == false);

                var documentTypeAssignmentList = documentTypeAssignmentIEnum.ToList();

                viewModel.EditSaleListingViewModel.DocumentTypeAssignmentList =
                    documentTypeAssignmentList
                        .Where(x => x.TransactionType.Id == model.TransactionType!.Id)
                        .ToList();

                viewModel.EditSaleListingViewModel.DocumentTypeList = _documentTransactionService
                    .GetDocumentTypesFromDocumentTypeAssignment(
                        model.TransactionType!.Description,
                        documentTypeAssignmentList);

                // Load deal statuses
                var dealStatusIEnum = await _unitOfWork.DealStatus.GetAllAsync(
                    x => x.IsActive == true && !x.IsDeleted);

                viewModel.EditSaleListingViewModel.DealStatusList = dealStatusIEnum.ToList();

                // Signage dropdown
                viewModel.EditSaleListingViewModel.SignageIEnum =
                    SelectListConverter.CreateSelectListFromEnum<Signage>();

                viewModel.EditSaleListingViewModel.DefaultSignage = 1;

                // Menus
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load SaleListing Edit page");
                return StatusCode(500, "An error occurred while loading the deal.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditSaleListingWithTabMenuViewModel viewModel)
        {
            if (id == 0)
                return NotFoundResultWithMessage("SaleListing", id);

            var actionType = TransactionHelper.NormalizeActionType(Request.Form["actionType"]);

            if (viewModel?.EditSaleListingViewModel == null)
            {
                _logger.LogError("Edit SaleListing POST received null viewModel or EditSaleListingViewModel for id {Id}.", id);
                return BadRequest("Invalid form submission.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out Guid parsedUserId))
                throw new InvalidOperationException("Invalid user ID - cannot convert to GUID.");

            var user = await _userManager.FindByIdAsync(userId!);
            if (user == null)
            {
                _logger.LogError("Edit SaleListing POST: current user {UserId} was not found.", userId);
                return Unauthorized();
            }

            // Load tracked SaleListing
            var saleListingModel = await _unitOfWork.SaleListing.GetAsync(
                x => x.Id == id,
                includeProperties: "IRFDeal,TransactionType,DealStatus",
                tracked: true);

            if (saleListingModel == null)
                return NotFoundResultWithMessage("SaleListing", id);

            // Rebuild all data needed by the view
            await PopulateEditSaleListingViewModelAsync(viewModel, saleListingModel);

            // Existing model validation behavior:
            // save = allow draft behavior
            // submit = enforce validation
            if (!ModelState.IsValid && !TransactionHelper.IsSaveAction(actionType))
            {
                if (viewModel.EditSaleListingViewModel.DealStatus == null)
                    viewModel.EditSaleListingViewModel.DealStatus = saleListingModel.DealStatus;
                return View(viewModel);
            }

            // Load transaction type
            var transactionType = await _unitOfWork.TransactionType.GetAsync(x => x.Id == saleListingModel.TransactionTypeId);
            if (transactionType == null)
            {
                ModelState.AddModelError(string.Empty, "Transaction type not found.");
                if (viewModel.EditSaleListingViewModel.DealStatus == null)
                    viewModel.EditSaleListingViewModel.DealStatus = saleListingModel.DealStatus;
                return View(viewModel);
            }

            viewModel.EditSaleListingViewModel.TransactionType = transactionType;
            viewModel.EditSaleListingViewModel.TransactionTypeId = transactionType.Id;

            // Validate additional documents only on submit
            if (!TransactionHelper.IsSaveAction(actionType))
            {
                var additionalDocumentErrors = ValidateAdditionalDocuments(viewModel.EditSaleListingViewModel.AdditionalDocuments);

                if (additionalDocumentErrors.Any())
                {
                    foreach (var error in additionalDocumentErrors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }

                    await PopulateEditSaleListingViewModelAsync(viewModel, saleListingModel);
                    if (viewModel.EditSaleListingViewModel.DealStatus == null)
                        viewModel.EditSaleListingViewModel.DealStatus = saleListingModel.DealStatus;
                    return View(viewModel);
                }
            }

            // Keep view consistency for existing uploads
            saleListingModel.FileUploads = FilterFileUploadsWithFilesOnly(saleListingModel.FileUploads);

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Update IRFDeal
                var irfDealDb = await _unitOfWork.IRFDeal.GetAsync(
                    x => x.Id == saleListingModel.IRFDealId,
                    tracked: true);

                var editedIRFData = viewModel.EditSaleListingViewModel.EditIRFDealSaleListingViewModel;

                if (irfDealDb != null && editedIRFData != null)
                {
                    irfDealDb.UpdateFromViewModel(editedIRFData, parsedUserId);
                    irfDealDb.UpdatedAt = DateTime.Now;
                    irfDealDb.UpdatedBy = parsedUserId;

                    await _unitOfWork.IRFDeal.UpdateAsync(irfDealDb);
                }

                // Update SaleListing
                saleListingModel.DealStatusId = viewModel.EditSaleListingViewModel.DealStatusId;
                saleListingModel.Notes = viewModel.EditSaleListingViewModel.Notes;
                saleListingModel.HasRejectedOffers = viewModel.EditSaleListingViewModel.HasRejectedOffers;
                saleListingModel.DateSubmitted = DateTime.Now;
                saleListingModel.IsActive = true;
                saleListingModel.UpdatedAt = DateTime.Now;
                saleListingModel.UpdatedBy = parsedUserId;

                // Concurrency token
                saleListingModel.RowVersion = viewModel.EditSaleListingViewModel.RowVersion;

                await _unitOfWork.SaveChangesAsync();

                // Upload path
                var uploadPath = await CreateFilePathAsync(saleListingModel.Id);

                // Standard document uploads
                var fileUploadVM = viewModel.EditSaleListingViewModel
                    .CreateFileUploadListViewModel?
                    .CreateFileUploadsViewModel;

                if (fileUploadVM != null)
                {
                    _logger.LogInformation("Upload request received for SaleListing {SaleListingId}", saleListingModel.Id);
                    await CreateFileUploadData(
                        fileUploadVM,
                        saleListingModel.Id,
                        transactionType.Id,
                        viewModel.EditSaleListingViewModel.DocumentTypeList ?? Enumerable.Empty<DocumentType>(),
                        uploadPath
                    );
                }

                // Additional documents
                var additionalDocumentVM = viewModel.EditSaleListingViewModel.AdditionalDocuments;

                if (additionalDocumentVM != null && additionalDocumentVM.Any())
                {
                    await CreateAdditionalDocumentData(
                        additionalDocumentVM,
                        saleListingModel.Id,
                        transactionType.Id,
                        uploadPath
                    );
                }

                // Email notification on submit
                if (TransactionHelper.IsSubmitAction(actionType) && saleListingModel.DealStatusId == 1)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var dealLink = Url.Action("Login", "Account", new { id = saleListingModel.Id, token }, Request.Scheme);

                    var dealAdminEmails = await GetDealAdminEmails();
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var allRecipientEmails = dealAdminEmails.Select(e => e.Email).ToList();
                    if (!string.IsNullOrWhiteSpace(user.Email) &&
                        !allRecipientEmails.Contains(user.Email, StringComparer.OrdinalIgnoreCase))
                    {
                        allRecipientEmails.Add(user.Email);
                    }

                    var allRoleNames = dealAdminEmails.Select(e => e.RoleName).ToList();
                    string creatorRole = userRoles.FirstOrDefault() ?? "Deal Creator";
                    if (!allRoleNames.Contains(creatorRole, StringComparer.OrdinalIgnoreCase))
                    {
                        allRoleNames.Add(creatorRole);
                    }

                    var mailRequests = EmailRequest.SendNotificationWhenDealIsSubmitted(
                        user,
                        allRoleNames,
                        transactionType.Description,
                        allRecipientEmails,
                        saleListingModel.DealTransactionNumber,
                        dealLink!
                    );

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
                                break;
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
                            _logger.LogError(
                                lastException,
                                $"All {maxRetries} attempts to send email to {mailRequest.ToEmail} failed. Email Request: {System.Text.Json.JsonSerializer.Serialize(mailRequest)}");

                            failedEmails.Add(mailRequest);
                        }
                    }
                }

                await transaction.CommitAsync();
                return RedirectToAction(nameof(Index), new { editSuccess = true });
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();

                ModelState.AddModelError(
                    string.Empty,
                    "This record was modified by another user after you loaded it. Please reload and try again.");

                await PopulateEditSaleListingViewModelAsync(viewModel, saleListingModel);
                if (viewModel.EditSaleListingViewModel.DealStatus == null)
                    viewModel.EditSaleListingViewModel.DealStatus = saleListingModel.DealStatus;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Edit SaleListing failed for id {SaleListingId}, user {UserId}", id, userId);
                throw;
            }
        }
        private List<string> ValidateAdditionalDocuments(List<AdditionalDocumentInputViewModel>? additionalDocuments)
        {
            var errors = new List<string>();

            if (additionalDocuments == null || !additionalDocuments.Any())
                return errors;

            foreach (var item in additionalDocuments)
            {
                if (item == null)
                    continue;

                bool hasDocumentName = !string.IsNullOrWhiteSpace(item.DocumentName);
                bool hasFile = item.File != null && item.File.Length > 0;

                if (!hasDocumentName && !hasFile)
                    continue;

                if (!hasDocumentName)
                    errors.Add("Document name is required for additional documents.");

                if (!hasFile)
                    errors.Add($"File is required for '{item.DocumentName}'.");
            }

            return errors;
        }
        private async Task<bool> CreateAdditionalDocumentData(
            List<AdditionalDocumentInputViewModel> additionalDocuments,
            int saleListingId,
            int transactionTypeId,
            string uploadPath)
        {
            _logger.LogInformation(
                "Additional document upload request received for SaleListing {SaleListingId}, TransactionTypeId={TransactionTypeId}",
                saleListingId, transactionTypeId);

            if (additionalDocuments == null || !additionalDocuments.Any())
            {
                _logger.LogWarning("No additional documents submitted for SaleListing {SaleListingId}", saleListingId);
                return true;
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);
            if (user == null)
            {
                _logger.LogError("CreateAdditionalDocumentData: current user {UserId} was not found.", userId);
                return false;
            }

            var additionalFolder = TransactionHelper.SanitizePathSegment("AdditionalDocuments", "AdditionalDocuments");
            string additionalDocsPath = Path.Combine(uploadPath, _transactionTypeName, additionalFolder);
            TransactionHelper.EnsureDirectoryExists(additionalDocsPath);

            foreach (var item in additionalDocuments)
            {
                if (item == null)
                    continue;

                bool hasDocumentName = !string.IsNullOrWhiteSpace(item.DocumentName);
                bool hasFile = item.File != null && item.File.Length > 0;

                if (!hasDocumentName && !hasFile)
                    continue;

                if (!hasDocumentName)
                    throw new InvalidOperationException("Document name is required for additional documents.");

                if (!hasFile)
                    throw new InvalidOperationException($"File is required for additional document '{item.DocumentName}'.");

                try
                {
                    _logger.LogInformation(
                        "Uploading additional document '{DocumentName}' for SaleListing {SaleListingId}",
                        item.DocumentName, saleListingId);

                    var (fileNameWithoutExtension, fileExtension, originalFileName) =
                        await _fileHandlerService.UploadFile(item.File!, additionalDocsPath);

                    if (string.IsNullOrWhiteSpace(fileNameWithoutExtension))
                    {
                        _logger.LogWarning(
                            "Additional document upload returned empty file name for '{DocumentName}'",
                            item.DocumentName);
                        continue;
                    }

                    var entity = new AdditionalDocument
                    {
                        DocumentName = item.DocumentName.Trim(),
                        FileName = fileNameWithoutExtension,
                        FileExtension = fileExtension,
                        OriginalFileName = originalFileName,
                        Directory = additionalDocsPath,
                        FullPath = Path.Combine(additionalDocsPath, fileNameWithoutExtension + fileExtension),
                        FileSize = item.File!.Length.ToString(),

                        SaleListingId = saleListingId,
                        TransactionTypeId = transactionTypeId,

                        IsActive = true,
                        IsDeleted = false,
                        CreatedAt = DateTime.Now,
                        CreatedBy = user.Id
                    };

                    await _unitOfWork.AdditionalDocument.AddAsync(entity);
                    _logger.LogInformation(
                        "Additional document uploaded successfully: {OriginalFileName} -> {StoredFileName}{Extension}",
                        originalFileName, fileNameWithoutExtension, fileExtension);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Upload failed for additional document '{DocumentName}' into directory {Directory}",
                        item.DocumentName, additionalDocsPath);
                    throw;
                }
            }

            return true;
        }
        private async Task PopulateEditSaleListingViewModelAsync(
            EditSaleListingWithTabMenuViewModel viewModel,
            SaleListing saleListingModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            var transactionType = await _unitOfWork.TransactionType.GetAsync(x => x.Id == saleListingModel.TransactionTypeId);
            viewModel.EditSaleListingViewModel.TransactionType = transactionType;
            viewModel.EditSaleListingViewModel.TransactionTypeId = transactionType!.Id;

            var documentTypeAssignmentIEnum = await _unitOfWork.DocumentTypeAssignment
                .GetAllAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,DocumentType");

            var documentTypeAssignmentList = documentTypeAssignmentIEnum.ToList();

            viewModel.EditSaleListingViewModel.DocumentTypeAssignmentList =
                documentTypeAssignmentList
                    .Where(x => x.TransactionType.Id == saleListingModel.TransactionTypeId)
                    .ToList();

            viewModel.EditSaleListingViewModel.DocumentTypeList = _documentTransactionService
                .GetDocumentTypesFromDocumentTypeAssignment(
                    transactionType.Description,
                    documentTypeAssignmentList);

            var dealStatusIEnum = await _unitOfWork.DealStatus.GetAllAsync(x => x.IsActive && !x.IsDeleted);
            viewModel.EditSaleListingViewModel.DealStatusList = dealStatusIEnum.ToList();

            viewModel.EditSaleListingViewModel.SignageIEnum = SelectListConverter.CreateSelectListFromEnum<Signage>();
            viewModel.EditSaleListingViewModel.DefaultSignage = 1;

            viewModel.EditSaleListingViewModel.UpdatedByFullName = $"{user!.FirstName} {user.LastName}";
            viewModel.EditSaleListingViewModel.UpdatedBy = user.Id;
            viewModel.EditSaleListingViewModel.UpdatedAt = DateTime.Now;

            // Load predefined uploaded files
            var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(
                x => x.SaleListingId == saleListingModel.Id && x.IsActive && !x.IsDeleted,
                includeProperties: "SaleListing,TransactionType,DocumentType");

            viewModel.EditSaleListingViewModel.FileUploads = FilterFileUploadsWithFilesOnly(fileUploads.ToList());

            // Load saved additional documents
            var additionalDocuments = await _unitOfWork.AdditionalDocument.GetAllAsync(
                x => x.SaleListingId == saleListingModel.Id && x.IsActive && !x.IsDeleted,
                includeProperties: "SaleListing,TransactionType");

            viewModel.EditSaleListingViewModel.SavedAdditionalDocuments = additionalDocuments.ToList();

            var menus = EditDealTransactionTabMenuData.GetEditDealTransactionTabMenuData();
            string menuName = "IRFDetails1";

            foreach (var menu in menus.Where(x => x.Menu == menuName))
            {
                menu.IsActive = "active";
                menu.IsSelected = "true";
            }

            viewModel.Menus = menus;
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
            var model = await _unitOfWork.SaleListing.GetAsync(
                x => x.Id == id,
                includeProperties: "IRFDeal,TransactionType,DealStatus,FileUploads,AdditionalDocuments"
            );

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

            var viewModel = model.ToPreviewSaleListingViewModel();

            var buyingCommissionSignage = (Signage)model.IRFDeal.BuyingCommissionSign;
            viewModel.BuyingCommissionSignDescription = buyingCommissionSignage.GetDisplayName();

            var listingCommissionSignage = (Signage)model.IRFDeal.ListingCommissionSign;
            viewModel.ListingCommissionSignDescription = listingCommissionSignage.GetDisplayName();

            var user = _userManager.Users.FirstOrDefault(x => x.Id == viewModel.CreatedBy);
            if (user != null)
            {
                viewModel.CreatedByFullName = user.FirstName + " " + user.LastName;
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ListPartialView(SaleListingListPartialViewModel listViewModel)
        {
            // Get the current controller name
            string controllerName = ControllerContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;

            // Update menus with active state
            listViewModel.Menus = AdministrationTabMenuData.GetAdministrationTabMenuData()
                .Select(menu =>
                {
                    menu.IsActive = menu.Menu == controllerName ? "active" : string.Empty;
                    menu.IsSelected = menu.Menu == controllerName ? "true" : "false";
                    return menu;
                })
                .ToList();

            // Fetch SaleListing data with related properties
            var saleListings = await _unitOfWork.SaleListing.GetAllAsync(includeProperties: "IRFDeal,TransactionType,DealStatus");

            // Map data to the view model
            listViewModel.SaleListingListViewModel = saleListings.ToSaleListingListViewModel();

            // Return the partial view
            return PartialView("~/Views/SaleListing/Partial/ListPartial.cshtml", listViewModel);
        }


        public async Task<IActionResult> DeleteMultipleModal(List<int> ids)
        {
            var modelIEnum = await _unitOfWork.SaleListing.GetAllAsync(x => ids.Contains(x.Id));

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

            var deleteSaleListingIEnumViewModel = modelIEnum.ToDeleteSaleListingIEnumViewModel();

            return PartialView("~/Views/SaleListing/Modal/DeleteMultipleModal.cshtml", deleteSaleListingIEnumViewModel);
        }

        public async Task<IActionResult> DeleteMultiple(IEnumerable<DeleteSaleListingViewModel> viewModelList)
        {
            if (viewModelList == null || !viewModelList.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            var ids = viewModelList.Select(x => x.Id).ToList();

            var modelList = await _unitOfWork.SaleListing.GetAllAsync(x => ids.Contains(x.Id));

            if (modelList != null)
            {
                try
                {
                    await _unitOfWork.SaleListing.RemoveRangeAsync(modelList);

                    return RedirectToAction(nameof(Index), new { deleteSuccess = true });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SoftDeleteMultiple(IEnumerable<DeleteSaleListingViewModel> viewModelList)
        {
            if (viewModelList == null || !viewModelList.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            var ids = viewModelList.Select(x => x.Id).ToList();

            var modelList = await _unitOfWork.SaleListing.GetAllAsync(x => ids.Contains(x.Id));

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
                    await _unitOfWork.SaleListing.UpdateRangeAsync(modelList);

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
        public async Task<IActionResult> DownloadAdditionalDocument(int id)
        {
            var additionalDocument = await _unitOfWork.AdditionalDocument.GetAsync(x => x.Id == id && !x.IsDeleted);
            if (additionalDocument == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(additionalDocument.Directory) ||
                string.IsNullOrWhiteSpace(additionalDocument.FileName) ||
                string.IsNullOrWhiteSpace(additionalDocument.FileExtension))
            {
                return NotFound();
            }

            var result = await _fileHandlerService.DownloadFile(
                additionalDocument.Directory,
                additionalDocument.FileName,
                additionalDocument.FileExtension);

            return File(result.Item1, result.Item2, additionalDocument.OriginalFileName);
        }
        [HttpGet]
        public async Task<IActionResult> PreviewAdditionalDocument(int id)
        {
            var additionalDocument = await _unitOfWork.AdditionalDocument.GetAsync(
                x => x.Id == id && !x.IsDeleted);

            if (additionalDocument == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(additionalDocument.FullPath) ||
                !System.IO.File.Exists(additionalDocument.FullPath))
            {
                return NotFound();
            }

            var extension = additionalDocument.FileExtension?.ToLower();
            if (extension != ".pdf")
            {
                return RedirectToAction(nameof(DownloadAdditionalDocument), new { id });
            }

            var fs = new FileStream(additionalDocument.FullPath, FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAdditionalDocumentAjax(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            var additionalDocument = await _unitOfWork.AdditionalDocument.GetAsync(x => x.Id == id);

            if (additionalDocument == null)
            {
                return Json(new { success = false, message = "Document not found." });
            }

            additionalDocument.IsDeleted = true;
            additionalDocument.DeletedAt = DateTime.UtcNow;
            additionalDocument.DeletedBy = user!.Id;

            await _unitOfWork.AdditionalDocument.UpdateAsync(additionalDocument);

            if (!string.IsNullOrWhiteSpace(additionalDocument.FullPath) &&
                System.IO.File.Exists(additionalDocument.FullPath))
            {
                await _fileHandlerService.DeleteFile(additionalDocument.FullPath);
            }

            return Json(new { success = true });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFileAjax(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);
            var fileUpload = await _unitOfWork.FileUpload.GetAsync(x => x.Id == id);

            if (fileUpload != null)
            {
                fileUpload.IsDeleted = true;
                fileUpload.DeletedAt = DateTime.UtcNow;
                fileUpload.DeletedBy = user.Id;
                await _unitOfWork.FileUpload.UpdateAsync(fileUpload);
                await _fileHandlerService.DeleteFile(fileUpload.FullPath);
                return Json(new { success = true });
            }
            return Json(new { success = false });
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
        public async Task<JsonResult> GetDocumentTypesThatHasFileUploads(int saleListingId)
        {
            var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(
                x => x.SaleListingId == saleListingId
                && !x.IsDeleted && x.IsActive
            );

            // Only get DocumentTypeId (no null check needed, not nullable!)
            var documentTypeIds = fileUploads
                .Select(x => x.DocumentTypeId)
                .Distinct()
                .ToArray();

            return Json(documentTypeIds);
        }


        [HttpPost]
        public async Task<IActionResult> GetSaleListingList([FromBody] DtParameters dtParameters)
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
            var result = await _unitOfWork.SaleListing.AsQueryableAsync(
                x => !x.IsDeleted,
                includeProperties: "IRFDeal,TransactionType,DealStatus"
            );

            // 🔹 Apply role-based filtering
            if (!(isAdmin || isSuperAdmin || isSuperUser))
            {
                if (isAgent)
                {
                    result = result.Where(x => x.CreatedBy == user!.Id && ( x.DealStatus!.Id == 2 || x.DealStatus!.Id == 5 ) ); // Saved, Return to agent
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
            var resultList = result.ToSaleListingListViewModel();

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
            var jsonResult = new DtResult<SaleListing>
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


        private IQueryable<SaleListing> DefaultFilterSearch(IQueryable<SaleListing> result, string searchBy)
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
                   r.IRFDeal!.BuyerEmail != null && r.IRFDeal.BuyerEmail.ToUpper().Contains(searchBy.ToUpper())
                   );
            return result;
        }
        private IQueryable<SaleListing> FilterSearchWithParameters(IQueryable<SaleListing> query, string searchBy)
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

                var result = _unitOfWork.SaleListing.AsQueryable(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");

                if (!string.IsNullOrEmpty(searchBy))
                {
                    if (searchBy.Contains(":") || searchBy.Contains("+"))
                    {
                        result = FilterSearchWithParameters(result, searchBy);
                    }
                    else
                    {
                        result = DefaultFilterSearch(result, searchBy);
                    }
                    ;

                }

                result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

                var resultList = await result.ToListAsync();

                var saleListingViewModel = resultList.ToSaleListingListViewModel();

                var resultListForExport = saleListingViewModel.ToSaleListingListExport();

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
