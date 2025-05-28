using Intl.Realty.Firm.Helper;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.FileUploadVM;
using Intl.Realty.Firm.Model.ViewModel.SaleListingVM;
using Intl.Realty.Firm.Models;
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
                                    IEmailService emailService
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
                                                                                                        documentTypeAssignmentList,
                                                                                                        documentTypeList);

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
            viewModel.CreateSaleListingViewModel.DealStatusId = actionType.ToLower() == "submit" ? 1 : 2;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);
            string transactionTypeName = _transactionTypeName;

            var transactionType = await _unitOfWork.TransactionType.GetAsync(x => x.Description == transactionTypeName);
            viewModel.CreateSaleListingViewModel.TransactionType = transactionType;
            viewModel.CreateSaleListingViewModel.TransactionTypeId = transactionType.Id;
            viewModel.CreateSaleListingViewModel.CreatedByFullName = $"{user.FirstName} {user.LastName}";

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 🔢 Concurrency-safe number range update
                var numberRange = await _unitOfWork.DealTransactionNumberRange
                    .GetAsync(x => !x.IsDeleted && x.Year == DateTime.Now.Year);

                if (numberRange != null)
                {
                    numberRange.CurrentNumber += 1;
                    await _unitOfWork.DealTransactionNumberRange.UpdateAsync(numberRange);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Prepare document types
                var docAssignments = await _unitOfWork.DocumentTypeAssignment.GetAllAsync(x => !x.IsDeleted, includeProperties: "TransactionType,DocumentType");
                var docTypes = await _unitOfWork.DocumentType.GetAllAsync(x => !x.IsDeleted);
                viewModel.CreateSaleListingViewModel.DocumentTypeList = _documentTransactionService
                    .GetDocumentTypesFromDocumentTypeAssignment(transactionTypeName, docAssignments.ToList(), docTypes.ToList());

                // Set tab menu
                var menus = CreateDealTransactionTabMenuData.GetCreateDealTransactionTabMenuData();
                foreach (var menu in menus.Where(x => x.Menu == "IRFDetails1"))
                {
                    menu.IsActive = "active";
                    menu.IsSelected = "true";
                }
                viewModel.Menus = menus;

                // 🛑 Validate before proceeding on submit
                if (actionType.ToLower() == "submit" && !ModelState.IsValid)
                {
                    return View(viewModel);
                }

                // ➕ Create IRF Deal
                var irfDealModel = viewModel.CreateSaleListingViewModel.CreateIRFDealSaleListingViewModel?.ToIRFDealModel();
                if (irfDealModel != null)
                {
                    irfDealModel.CreatedAt = DateTime.Now;
                    irfDealModel.CreatedBy = user.Id;
                    irfDealModel.IsActive = true;
                    irfDealModel.IsDeleted = false;

                    await _unitOfWork.IRFDeal.AddAsync(irfDealModel);
                    await _unitOfWork.SaveChangesAsync();
                }

                var newIRFDeal = await _unitOfWork.IRFDeal.GetAsync(x => x.Id == irfDealModel!.Id);

                // Set SaleListing fields
                viewModel.CreateSaleListingViewModel.IRFDealId = newIRFDeal!.Id;
                viewModel.CreateSaleListingViewModel.CreateIRFDealSaleListingViewModel!.CreatedAt = newIRFDeal.CreatedAt;
                viewModel.CreateSaleListingViewModel.CreateIRFDealSaleListingViewModel.CreatedBy = newIRFDeal.CreatedBy;
                viewModel.CreateSaleListingViewModel.CreateIRFDealSaleListingViewModel.IsActive = newIRFDeal.IsActive;

                viewModel.CreateSaleListingViewModel.IsActive = true;
                viewModel.CreateSaleListingViewModel.IsDeleted = false;
                viewModel.CreateSaleListingViewModel.CreatedBy = user.Id;
                viewModel.CreateSaleListingViewModel.CreatedAt = DateTime.Now;

                var saleListingModel = viewModel.CreateSaleListingViewModel.ToSaleListingModel();
                await _unitOfWork.SaleListing.AddAsync(saleListingModel);
                await _unitOfWork.SaveChangesAsync();

                // 📂 File uploads (optional)
                if (viewModel.CreateSaleListingViewModel.CreateFileUploadListViewModel?.CreateFileUploadsViewModel != null)
                {
                    var uploadPath = CreateFilePath(saleListingModel.Id);
                    await CreateFileUploadData(
                        viewModel.CreateSaleListingViewModel.CreateFileUploadListViewModel.CreateFileUploadsViewModel,
                        saleListingModel.Id,
                        transactionType.Id,
                        viewModel.CreateSaleListingViewModel.DocumentTypeList,
                        uploadPath
                    );
                }

                // 📩 Notification email (submit only)
                if (viewModel.CreateSaleListingViewModel.DealStatusId == 1)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var dealLink = Url.Action("Login", "Account", new { id = saleListingModel.Id, token }, Request.Scheme);
                    var dealAdminEmails = await GetDealAdminEmails();
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var mailRequest = EmailRequest.SendNotificationWhenDealIsSubmitted(
                        user, userRoles, transactionType.Description, dealAdminEmails,
                        saleListingModel.DealTransactionNumber, dealLink!);

                    await _emailService.SendEmailAsync(mailRequest);
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
            catch
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

            var saleListing = await _unitOfWork.SaleListing.GetAsync(x => x.Id == dealId, includeProperties: "IRFDeal,TransactionType,DealStatus");
            var transactionType = saleListing.TransactionType;

            var mailRequest = EmailRequest.SendRequestToEditTheDeal(user!, userRoles, transactionType!.Description, dealAdminEmails, saleListing.DealTransactionNumber, dealLink!);

            await _emailService.SendEmailAsync(mailRequest);

            return RedirectToAction("Edit", new { id = dealId });
        }
        private string CreateFilePath(int saleListingId)
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

            uploadPath = Path.Combine(uploadPath, userIdPath + saleListingId + "\\");

            return uploadPath;
        }
        private async Task<bool> CreateFileUploadData(List<CreateFileUploadViewModel> createFileUploadsViewModel,
                                              int saleListingModelId,
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
                            SaleListingId = saleListingModelId,
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
            try
            {
                var model = await _unitOfWork.SaleListing.GetAsync(x => x.Id == id, includeProperties: "TransactionType,IRFDeal,DealStatus");

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
                EditSaleListingWithTabMenuViewModel viewModel = new EditSaleListingWithTabMenuViewModel();

                // to get current user ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // to get current user info
                var user = await _userManager.FindByIdAsync(userId!);

                viewModel.EditSaleListingViewModel = model.ToEditSaleListingViewModel();

                viewModel.EditSaleListingViewModel.DealStatus = model.DealStatus;

                viewModel.EditSaleListingViewModel.UpdatedByFullName = user!.FirstName + " " + user.LastName;
                viewModel.EditSaleListingViewModel.UpdatedBy = user!.Id;
                viewModel.EditSaleListingViewModel.UpdatedAt = DateTime.Now;

                var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.SaleListingId == model.Id, includeProperties: "SaleListing,TransactionType,DocumentType");

                var fileUploadsWithFiles = FilterFileUploadsWithFilesOnly(fileUploads.ToList());

                viewModel.EditSaleListingViewModel.FileUploads = fileUploadsWithFiles;

                var documentTypeAssignmentIEnum = await _unitOfWork.DocumentTypeAssignment.GetAllAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,DocumentType");
                var documentTypeIEnum = await _unitOfWork.DocumentType.GetAllAsync(x => x.IsActive && x.IsDeleted == false);
                var documentTypeAssignmentList = documentTypeAssignmentIEnum.ToList();

                viewModel.EditSaleListingViewModel.DocumentTypeAssignmentList = documentTypeAssignmentList.Where(x => x.TransactionType.Id == model.TransactionType.Id).ToList(); // DocumentTypeAssignmentList

                var documentTypeList = documentTypeIEnum.ToList();
                viewModel.EditSaleListingViewModel.DocumentTypeList = _documentTransactionService
                                                                .GetDocumentTypesFromDocumentTypeAssignment(model.TransactionType!.Description,
                                                                                                            documentTypeAssignmentList,
                                                                                                            documentTypeList);
                var dealStatusIEnum = await _unitOfWork.DealStatus.GetAllAsync(x => x.IsActive == true && !x.IsDeleted);
                viewModel.EditSaleListingViewModel.DealStatusList = dealStatusIEnum.ToList();

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
                _logger.LogError(ex, "Failed to update SaleListing");
                return StatusCode(500, "An error occurred while saving the deal.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditSaleListingWithTabMenuViewModel viewModel)
        {
            if (id == 0)
                return NotFoundResultWithMessage("SaleListing", id);

            var dealStatusList = await _unitOfWork.DealStatus.GetAllAsync(x => x.IsActive && !x.IsDeleted);
            viewModel.EditSaleListingViewModel.DealStatusList = dealStatusList.ToList();

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

            // Load SaleListing model (tracked)
            var saleListingModel = await _unitOfWork.SaleListing.GetAsync(
                x => x.Id == id,
                includeProperties: "IRFDeal,TransactionType,DealStatus",
                tracked: true);

            if (saleListingModel == null)
                return NotFoundResultWithMessage("SaleListing", id);

            // Load transaction type
            var transactionType = await _unitOfWork.TransactionType.GetAsync(x => x.Id == saleListingModel.TransactionTypeId);
            viewModel.EditSaleListingViewModel.TransactionType = transactionType;
            viewModel.EditSaleListingViewModel.TransactionTypeId = transactionType.Id;

            // Rebuild document type list
            var documentTypeAssignments = await _unitOfWork.DocumentTypeAssignment
                .GetAllAsync(x => !x.IsDeleted, includeProperties: "TransactionType,DocumentType");
            var documentTypes = await _unitOfWork.DocumentType.GetAllAsync(x => !x.IsDeleted);

            viewModel.EditSaleListingViewModel.DocumentTypeList = _documentTransactionService
                .GetDocumentTypesFromDocumentTypeAssignment(
                    transactionType.Description,
                    documentTypeAssignments.ToList(),
                    documentTypes.ToList());

            // Filter file uploads for view consistency
            saleListingModel.FileUploads = FilterFileUploadsWithFilesOnly(saleListingModel.FileUploads);

            // Begin transaction
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Update IRFDeal if present
                var irfDealDb = await _unitOfWork.IRFDeal.GetAsync(x => x.Id == saleListingModel.IRFDealId, tracked: true);
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
                saleListingModel.DateSubmitted = DateTime.Now;
                saleListingModel.IsActive = true;
                saleListingModel.UpdatedAt = DateTime.Now;
                saleListingModel.UpdatedBy = parsedUserId;

                // Add concurrency check
                saleListingModel.RowVersion = viewModel.EditSaleListingViewModel.RowVersion;

                await _unitOfWork.SaveChangesAsync(); // All EF-tracked changes saved

                // Handle file uploads (optional)
                var uploadPath = CreateFilePath(saleListingModel.Id);
                var fileUploadVM = viewModel.EditSaleListingViewModel.CreateFileUploadListViewModel?.CreateFileUploadsViewModel;

                if (fileUploadVM != null)
                {
                    await CreateFileUploadData(
                        fileUploadVM,
                        saleListingModel.Id,
                        transactionType.Id,
                        viewModel.EditSaleListingViewModel.DocumentTypeList,
                        uploadPath
                    );
                }

                await transaction.CommitAsync();
                return RedirectToAction(nameof(Index), new { editSuccess = true });
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty, "This record was modified by another user after you loaded it. Please reload and try again.");
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
            var model = await _unitOfWork.SaleListing.GetAsync(x => x.Id == id, includeProperties: "IRFDeal,TransactionType,DealStatus");
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
            var fileUploadIEnum = await _unitOfWork.FileUpload.GetAllAsync(x => x.SaleListingId == model.Id && x.IsActive == true && !x.IsDeleted);
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
        public async Task<IActionResult> DeleteFile(int id)
        {
            var fileUpload = await _unitOfWork.FileUpload.GetAsync(x => x.Id == id);
            if (fileUpload != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId!);
                fileUpload.IsDeleted = true;
                fileUpload.DeletedAt = DateTime.UtcNow;
                fileUpload.DeletedBy = user.Id;
                await _unitOfWork.FileUpload.UpdateAsync(fileUpload);

                var isSuccess = await _fileHandlerService.DeleteFile(fileUpload.FullPath);

                return RedirectToAction(nameof(Edit), new { id = fileUpload.SaleListingId });
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

        public async Task<JsonResult> GetDocumentTypesThatHasFileUploads(int saleListingId)
        {
            var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.SaleListingId == saleListingId && x.IsDeleted == false && x.IsActive);
            var documentTypeIds = fileUploads.Select(x => x.DocumentTypeId).ToArray();

            var documentTypes = await _unitOfWork.DocumentType.GetAllAsync(x =>
                documentTypeIds.Contains(x.Id) &&
                x.IsActive &&
                !x.IsDeleted);
            return Json(documentTypes);
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

            // Initialize query
            var result = await _unitOfWork.SaleListing.AsQueryableAsync(
                x => !x.IsDeleted,
                includeProperties: "IRFDeal,TransactionType,DealStatus"
            );

            // 🔹 Apply role-based filtering
            if (!(isAdmin || isSuperAdmin))
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
