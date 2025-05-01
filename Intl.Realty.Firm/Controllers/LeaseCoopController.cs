using Intl.Realty.Firm.Helper;
using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.FileUploadVM;
using Intl.Realty.Firm.Model.ViewModel.LeaseCoopVM;
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
using System.Data.Entity;
using System.Security.Claims;

namespace Intl.Realty.Firm.Controllers
{
    [Authorize]
    public class LeaseCoopController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExportService<LeaseCoopExport> _exportService;
        private readonly IConfigurationService _configurationService;
        private readonly IFileHandlerService _fileHandlerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDocumentTransactionService _documentTransactionService;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        private readonly string _backupFileDirectory = "\\wwwroot\\Files\\Uploads\\";
        private readonly string _transactionTypeName = "LeaseCoop";
        public LeaseCoopController(IUnitOfWork unitOfWork,
                                    IExportService<LeaseCoopExport> exportService,
                                    IConfigurationService configurationService,
                                    IFileHandlerService fileHandlerService,
                                    UserManager<ApplicationUser> userManager,
                                    IDocumentTransactionService documentTransactionService,
                                    INotificationService notificationService,
                                    IEmailService emailService
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
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateLeaseCoopWithTabMenuViewModel viewModel = new CreateLeaseCoopWithTabMenuViewModel();

            // to get current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // to get current user info
            var user = await _userManager.FindByIdAsync(userId!);

            viewModel.CreateLeaseCoopViewModel.CreatedByFullName = user!.FirstName + " " + user.LastName;

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
            viewModel.CreateLeaseCoopViewModel.TransactionType = transactionType;
            viewModel.CreateLeaseCoopViewModel.TransactionTypeId = transactionType.Id;

            var paddedTransactionType = transactionType.Id.ToString().PadLeft(2, '0');

            var currentYear = DateTime.Now.Year.ToString();
            var currentMonth = DateTime.Now.Month.ToString().PadLeft(2, '0');
            var currentDay = DateTime.Now.Day.ToString().PadLeft(2, '0');
            var paddedCurrentNumber = (numberRange.CurrentNumber + 1).ToString().PadLeft(5, '0');
            var paddedNumberRange = currentYear + currentMonth + currentDay + "-" + paddedTransactionType + "-" + paddedCurrentNumber;

            viewModel.CreateLeaseCoopViewModel.DealTransactionNumber = paddedNumberRange;

            var documentTypeAssignmentIEnum = await _unitOfWork.DocumentTypeAssignment.GetAllAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,DocumentType");
            var documentTypeIEnum = await _unitOfWork.DocumentType.GetAllAsync(x => x.IsDeleted == false);
            var documentTypeAssignmentList = documentTypeAssignmentIEnum.ToList();
            
            viewModel.CreateLeaseCoopViewModel.DocumentTypeAssignmentList = documentTypeAssignmentList.Where(x=>x.TransactionType.Id == transactionType.Id).ToList();

            var documentTypeList = documentTypeIEnum.ToList();
            viewModel.CreateLeaseCoopViewModel.DocumentTypeList = _documentTransactionService
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
        public async Task<IActionResult> Create(CreateLeaseCoopWithTabMenuViewModel viewModel, string actionType)
        {
            if (actionType == "save")
            {
                viewModel.CreateLeaseCoopViewModel.DealStatusId = 2; // Draft
            }
            else if (actionType == "submit")
            {
                viewModel.CreateLeaseCoopViewModel.DealStatusId = 1; // Submitted
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);
            var transactionTypeName = _transactionTypeName;

            var transactionType = await _unitOfWork.TransactionType.GetAsync(x => x.Description == transactionTypeName);
            viewModel.CreateLeaseCoopViewModel.TransactionType = transactionType;
            viewModel.CreateLeaseCoopViewModel.TransactionTypeId = transactionType.Id;
            viewModel.CreateLeaseCoopViewModel.CreatedByFullName = $"{user.FirstName} {user.LastName}";

            await UpdateTransactionNumber();
            await PopulateDocumentTypesAsync(viewModel, transactionTypeName);
            SetTabMenus(viewModel, "IRFDetails1");

            // If this is a "Submit", run validations
            if (actionType == "Submit" && !ModelState.IsValid)
            {
                var notification = _notificationService.Notify(
                    "Please review the form for errors. All required fields must be filled correctly.",
                    "Validation Failed",
                    NotificationType.error
                );

                TempData["Message"] = JsonConvert.SerializeObject(notification);
                return View("Index", viewModel);
            }

            // Create IRF Deal
            var irfDealModel = viewModel.CreateLeaseCoopViewModel.CreateIRFDealLeaseCoopViewModel?.ToIRFDealModel();
            irfDealModel!.CreatedAt = DateTime.Now;
            irfDealModel.CreatedBy = user.Id;
            irfDealModel.IsActive = true;
            irfDealModel.IsDeleted = false;

            await _unitOfWork.IRFDeal.AddAsync(irfDealModel);
            var newIRFDeal = await _unitOfWork.IRFDeal.GetAsync(x => x.Id == irfDealModel.Id);

            viewModel.CreateLeaseCoopViewModel.IRFDealId = newIRFDeal!.Id;
            viewModel.CreateLeaseCoopViewModel.CreateIRFDealLeaseCoopViewModel!.CreatedAt = newIRFDeal.CreatedAt;
            viewModel.CreateLeaseCoopViewModel.CreateIRFDealLeaseCoopViewModel.CreatedBy = newIRFDeal.CreatedBy;
            viewModel.CreateLeaseCoopViewModel.CreateIRFDealLeaseCoopViewModel.IsActive = newIRFDeal.IsActive;

            // Set DealStatus
            viewModel.CreateLeaseCoopViewModel.IsActive = true;
            viewModel.CreateLeaseCoopViewModel.IsDeleted = false;
            viewModel.CreateLeaseCoopViewModel.CreatedBy = user.Id;
            viewModel.CreateLeaseCoopViewModel.CreatedAt = DateTime.Now;

            var leaseCoopModel = viewModel.CreateLeaseCoopViewModel.ToLeaseCoopModel();
            await _unitOfWork.LeaseCoop.AddAsync(leaseCoopModel);

            // File Upload only on Submit
            if (actionType == "Submit" && viewModel.CreateLeaseCoopViewModel.CreateFileUploadListViewModel?.CreateFileUploadsViewModel != null)
            {
                var uploadPath = CreateFilePath(leaseCoopModel.Id);
                await CreateFileUploadData(
                    viewModel.CreateLeaseCoopViewModel.CreateFileUploadListViewModel.CreateFileUploadsViewModel,
                    leaseCoopModel.Id,
                    transactionType.Id,
                    viewModel.CreateLeaseCoopViewModel.DocumentTypeList,
                    uploadPath
                );
            }
            leaseCoopModel.TransactionType = viewModel.CreateLeaseCoopViewModel.TransactionType;

            // Notify Deal Admin only on Submit
            if (viewModel.CreateLeaseCoopViewModel.DealStatusId == 1)
            {
                await SendDealAdminNotification(user, leaseCoopModel);
            }

            return RedirectToAction(nameof(Index), new { addSuccess = true });
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

            var leaseCoop = await _unitOfWork.LeaseCoop.GetAsync(x => x.Id == dealId, includeProperties: "IRFDeal,TransactionType,DealStatus");
            var transactionType = leaseCoop.TransactionType;

            var mailRequest = EmailRequest.SendRequestToEditTheDeal(user!, userRoles, transactionType!.Description, dealAdminEmails, leaseCoop.DealTransactionNumber, dealLink!);

            await _emailService.SendEmailAsync(mailRequest);

            return RedirectToAction("Edit", new { id = dealId });
        }
        private string CreateFilePath(int leaseCoopId)
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

            uploadPath = Path.Combine(uploadPath, userIdPath + leaseCoopId + "\\");

            return uploadPath;
        }
        private async Task<bool> CreateFileUploadData(List<CreateFileUploadViewModel> createFileUploadsViewModel,
                                              int leaseCoopModelId,
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
                            LeaseCoopId = leaseCoopModelId,
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
            var model = await _unitOfWork.LeaseCoop.GetAsync(x => x.Id == id, includeProperties: "TransactionType,IRFDeal,DealStatus");

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
            EditLeaseCoopWithTabMenuViewModel viewModel = new EditLeaseCoopWithTabMenuViewModel();

            // to get current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // to get current user info
            var user = await _userManager.FindByIdAsync(userId!);

            viewModel.EditLeaseCoopViewModel = model.ToEditLeaseCoopViewModel();

            viewModel.EditLeaseCoopViewModel.DealStatus = model.DealStatus;

            viewModel.EditLeaseCoopViewModel.UpdatedByFullName = user!.FirstName + " " + user.LastName;
            viewModel.EditLeaseCoopViewModel.UpdatedBy = user!.Id;
            viewModel.EditLeaseCoopViewModel.UpdatedAt = DateTime.Now;

            var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.LeaseCoopId == model.Id, includeProperties: "LeaseCoop,TransactionType,DocumentType");

            var fileUploadsWithFiles = FilterFileUploadsWithFilesOnly(fileUploads.ToList());

            viewModel.EditLeaseCoopViewModel.FileUploads = fileUploadsWithFiles;

            var documentTypeAssignmentIEnum = await _unitOfWork.DocumentTypeAssignment.GetAllAsync(x => x.IsDeleted == false, includeProperties: "TransactionType,DocumentType");
            var documentTypeIEnum = await _unitOfWork.DocumentType.GetAllAsync(x => x.IsDeleted == false);
            var documentTypeAssignmentList = documentTypeAssignmentIEnum.ToList();

            viewModel.EditLeaseCoopViewModel.DocumentTypeAssignmentList = documentTypeAssignmentList.Where(x => x.TransactionType.Id == model.TransactionType.Id).ToList(); // DocumentTypeAssignmentList

            var documentTypeList = documentTypeIEnum.ToList();
            viewModel.EditLeaseCoopViewModel.DocumentTypeList = _documentTransactionService
                                                            .GetDocumentTypesFromDocumentTypeAssignment(model.TransactionType!.Description,
                                                                                                        documentTypeAssignmentList,
                                                                                                        documentTypeList);
            var dealStatusIEnum = await _unitOfWork.DealStatus.GetAllAsync(x => x.IsActive == true && !x.IsDeleted);
            viewModel.EditLeaseCoopViewModel.DealStatusList = dealStatusIEnum.ToList();

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
        public async Task<IActionResult> Edit(int id, EditLeaseCoopWithTabMenuViewModel viewModel)
        {
            if (id == 0)
                return NotFoundResultWithMessage("LeaseCoop", id);

            var dealStatusIEnum = await _unitOfWork.DealStatus.GetAllAsync(x => x.IsActive == true && !x.IsDeleted);
            viewModel.EditLeaseCoopViewModel.DealStatusList = dealStatusIEnum.ToList();

            if (!ModelState.IsValid)
                return View(viewModel);

            // ✅ Get current user ID and parse to GUID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out Guid parsedUserId))
                throw new InvalidOperationException("Invalid user ID - cannot convert to GUID.");

            var user = await _userManager.FindByIdAsync(userId!);

            // ✅ Load LeaseCoop model (EF-tracked)
            var leaseCoopModel = await _unitOfWork.LeaseCoop.GetAsync(
                x => x.Id == id,
                includeProperties: "IRFDeal,TransactionType,DealStatus",
                tracked: true);


            if (leaseCoopModel == null)
                return NotFoundResultWithMessage("LeaseCoop", id);

            // ✅ Load transaction type
            var transactionType = await _unitOfWork.TransactionType.GetAsync(x => x.Id == leaseCoopModel.TransactionTypeId);
            viewModel.EditLeaseCoopViewModel.TransactionType = transactionType;
            viewModel.EditLeaseCoopViewModel.TransactionTypeId = transactionType.Id;

            // ✅ Rebuild document type list
            var documentTypeAssignments = await _unitOfWork.DocumentTypeAssignment
                .GetAllAsync(x => !x.IsDeleted, includeProperties: "TransactionType,DocumentType");

            var documentTypes = await _unitOfWork.DocumentType.GetAllAsync(x => !x.IsDeleted);

            viewModel.EditLeaseCoopViewModel.DocumentTypeList = _documentTransactionService
                .GetDocumentTypesFromDocumentTypeAssignment(
                    transactionType.Description,
                    documentTypeAssignments.ToList(),
                    documentTypes.ToList());

            // ✅ Filter file uploads
            leaseCoopModel.FileUploads = FilterFileUploadsWithFilesOnly(leaseCoopModel.FileUploads);

            // ✅ Update IRFDeal (independently)
            var irfDealDb = await _unitOfWork.IRFDeal.GetAsync(x => x.Id == leaseCoopModel.IRFDealId, tracked: true);
            var editedData = viewModel.EditLeaseCoopViewModel.EditIRFDealLeaseCoopViewModel;

            if (irfDealDb != null && editedData != null)
            {
                irfDealDb.UpdateFromViewModel(editedData, parsedUserId);
                await _unitOfWork.IRFDeal.UpdateAsync(irfDealDb);
            }

            // ✅ Update main LeaseCoop properties
            var isAgent = await _userManager.IsInRoleAsync(user, "Agent");
            leaseCoopModel.DealStatusId = viewModel.EditLeaseCoopViewModel.DealStatusId;
            leaseCoopModel.Notes = viewModel.EditLeaseCoopViewModel.Notes;
            leaseCoopModel.DateSubmitted = DateTime.Now;
            leaseCoopModel.IsActive = true;
            leaseCoopModel.UpdatedAt = DateTime.Now;
            leaseCoopModel.UpdatedBy = parsedUserId;

            //leaseCoopModel.IRFDeal = null; // optional: avoid nav re-persisting

            // ✅ Persist changes (entity is tracked)
            await _unitOfWork.SaveChangesAsync();

            // ✅ Handle file uploads
            var uploadPath = CreateFilePath(leaseCoopModel.Id);
            var fileUploadVM = viewModel.EditLeaseCoopViewModel.CreateFileUploadListViewModel?.CreateFileUploadsViewModel;

            if (fileUploadVM != null)
            {
                await CreateFileUploadData(
                    fileUploadVM,
                    leaseCoopModel.Id,
                    transactionType.Id,
                    viewModel.EditLeaseCoopViewModel.DocumentTypeList,
                    uploadPath
                );
            }

            return RedirectToAction(nameof(Index), new { editSuccess = true });
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
            var model = await _unitOfWork.LeaseCoop.GetAsync(x => x.Id == id, includeProperties: "IRFDeal,TransactionType,DealStatus");
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

            var viewModel = model.ToPreviewLeaseCoopViewModel();
            var fileUploadIEnum = await _unitOfWork.FileUpload.GetAllAsync(x => x.LeaseCoopId == model.Id && x.IsActive == true && !x.IsDeleted);
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
        public async Task<IActionResult> ListPartialView(LeaseCoopListPartialViewModel listViewModel)
        {
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

            // Fetch model and map to view model
            listViewModel.LeaseCoopListViewModel = (await _unitOfWork.LeaseCoop
                .GetAllAsync(includeProperties: "IRFDeal,TransactionType,DealStatus"))
                .ToLeaseCoopListViewModel();

            return PartialView("~/Views/LeaseCoop/Partial/ListPartial.cshtml", listViewModel);
        }

        public async Task<IActionResult> DeleteMultipleModal(List<int> ids)
        {
            var modelIEnum = await _unitOfWork.LeaseCoop.GetAllAsync(x => ids.Contains(x.Id));

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

            var deleteLeaseCoopIEnumViewModel = modelIEnum.ToDeleteLeaseCoopIEnumViewModel();

            return PartialView("~/Views/LeaseCoop/Modal/DeleteMultipleModal.cshtml", deleteLeaseCoopIEnumViewModel);
        }

        public async Task<IActionResult> DeleteMultiple(IEnumerable<DeleteLeaseCoopViewModel> viewModelList)
        {
            if (viewModelList == null || !viewModelList.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            var ids = viewModelList.Select(x => x.Id).ToList();

            var modelList = await _unitOfWork.LeaseCoop.GetAllAsync(x => ids.Contains(x.Id));

            if (modelList != null)
            {
                try
                {
                    await _unitOfWork.LeaseCoop.RemoveRangeAsync(modelList);

                    return RedirectToAction(nameof(Index), new { deleteSuccess = true });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SoftDeleteMultiple(IEnumerable<DeleteLeaseCoopViewModel> viewModelList)
        {
            if (viewModelList == null || !viewModelList.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            var ids = viewModelList.Select(x => x.Id).ToList();

            var modelList = await _unitOfWork.LeaseCoop.GetAllAsync(x => ids.Contains(x.Id));

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
                    await _unitOfWork.LeaseCoop.UpdateRangeAsync(modelList);

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
                await _unitOfWork.FileUpload.RemoveAsync(fileUpload);

                var isSuccess = await _fileHandlerService.DeleteFile(fileUpload.FullPath);

                return RedirectToAction(nameof(Edit), new { id = fileUpload.LeaseCoopId });
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

        public async Task<JsonResult> GetDocumentTypesThatHasFileUploads(int leaseCoopId)
        {
            var fileUploads = await _unitOfWork.FileUpload.GetAllAsync(x => x.LeaseCoopId == leaseCoopId);
            var documentTypeIds = fileUploads.Select(x => x.DocumentTypeId).ToArray();

            var documentTypes = await _unitOfWork.DocumentType.GetAllAsync(x =>
                documentTypeIds.Contains(x.Id) &&
                x.IsActive &&
                !x.IsDeleted);
            return Json(documentTypes);
        }

        [HttpPost]
        public async Task<IActionResult> GetLeaseCoopList([FromBody] DtParameters dtParameters)
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

            // EF-backed query
            var result = _unitOfWork.LeaseCoop.AsQueryable(
                x => !x.IsDeleted,
                includeProperties: "IRFDeal,TransactionType,DealStatus"
            );

            // Role-based filtering
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
                    var teamMemberIds = _userManager.Users
                        .Where(u => u.TeamLeaderId == user!.Id)
                        .Select(u => u.Id)
                        .ToList(); // ✅ Sync, safe

                    if (teamMemberIds.Any())
                    {
                        result = result.Where(x => teamMemberIds.Contains(x.CreatedBy));
                    }
                }
            }

            var totalResultsCount = result.Count();

            // Search filtering
            result = searchBy!.Contains(":") || searchBy.Contains("+")
                ? FilterSearchWithParameters(result, searchBy)
                : DefaultFilterSearch(result, searchBy);

            // Sorting
            result = orderAscendingDirection
                ? result.OrderByDynamic(orderCriteria!, DtOrderDir.Asc)
                : result.OrderByDynamic(orderCriteria!, DtOrderDir.Desc);

            var filteredResultsCount = result.Count();

            // Paging
            var pagedData = result
                .Skip(dtParameters.Start)
                .Take(dtParameters.Length)
                .ToList();

            var resultList = pagedData;

            // Safe user lookup using _userManager (sync)
            var createdByIds = resultList.Select(x => x.CreatedBy).Distinct().ToList();

            var usersList = _userManager.Users
                .Where(u => createdByIds.Contains(u.Id)) // runs in-memory
                .ToList(); // ✅ Safe

            var usersDictionary = usersList.ToDictionary(u => u.Id);

            foreach (var item in resultList)
            {
                if (usersDictionary.TryGetValue(item.CreatedBy, out var createdByUser))
                {
                    item.CreatedByFullName = $"{createdByUser.FirstName} {createdByUser.LastName}";
                }
            }

            return Json(new DtResult<LeaseCoop>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = resultList
            });
        }


        private IQueryable<LeaseCoop> DefaultFilterSearch(IQueryable<LeaseCoop> result, string searchBy)
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
                   r.IRFDeal!.SellersLawyer! != null && r.IRFDeal.SellersLawyer.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.SellersLawyerAddress! != null && r.IRFDeal.SellersLawyerAddress.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.SellersPhoneNumber! != null && r.IRFDeal.SellersPhoneNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.BuyersLawyer! != null && r.IRFDeal.BuyersLawyer.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.BuyersLawyerAddress! != null && r.IRFDeal.BuyersLawyerAddress.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.IRFDeal!.BuyersPhoneNumber! != null && r.IRFDeal.BuyersPhoneNumber.ToUpper().Contains(searchBy.ToUpper())
                   );
            return result;
        }
        private IQueryable<LeaseCoop> FilterSearchWithParameters(IQueryable<LeaseCoop> query, string searchBy)
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
                    case "SellersLawyer":
                        query = query.Where(r => r.IRFDeal!.SellersLawyer != null && r.IRFDeal!.SellersLawyer.ToUpper().Contains(value));
                        break;
                    case "SellersLawyerAddress":
                        query = query.Where(r => r.IRFDeal!.SellersLawyerAddress != null && r.IRFDeal!.SellersLawyerAddress.ToUpper().Contains(value));
                        break;
                    case "SellersPhoneNumber":
                        query = query.Where(r => r.IRFDeal!.SellersPhoneNumber != null && r.IRFDeal!.SellersPhoneNumber.ToUpper().Contains(value));
                        break;
                    case "BuyersLawyer":
                        query = query.Where(r => r.IRFDeal!.BuyersLawyer != null && r.IRFDeal!.BuyersLawyer.ToUpper().Contains(value));
                        break;
                    case "BuyersLawyerAddress":
                        query = query.Where(r => r.IRFDeal!.BuyersLawyerAddress != null && r.IRFDeal!.BuyersLawyerAddress.ToUpper().Contains(value));
                        break;
                    case "BuyersPhoneNumber":
                        query = query.Where(r => r.IRFDeal!.BuyersPhoneNumber != null && r.IRFDeal!.BuyersPhoneNumber.ToUpper().Contains(value));
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

                var result = _unitOfWork.LeaseCoop.AsQueryable(x => x.IsDeleted == false, includeProperties: "TransactionType,IRFDeal,DealStatus");

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

                var leaseCoopViewModel = resultList.ToLeaseCoopListViewModel();

                var resultListForExport = leaseCoopViewModel.ToLeaseCoopListExport();

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

        #region Helper Methods
        private async Task UpdateTransactionNumber()
        {
            var filter = LinqExtensions.AndAlso<DealTransactionNumberRange>(
                x => !x.IsDeleted,
                x => x.Year == DateTime.Now.Year
            );

            var numberRange = await _unitOfWork.DealTransactionNumberRange.GetAsync(filter);
            if (numberRange != null)
            {
                numberRange.CurrentNumber += 1;
                await _unitOfWork.DealTransactionNumberRange.UpdateAsync(numberRange);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        private async Task PopulateDocumentTypesAsync(CreateLeaseCoopWithTabMenuViewModel viewModel, string transactionTypeName)
        {
            var assignments = await _unitOfWork.DocumentTypeAssignment.GetAllAsync(
                x => !x.IsDeleted,
                includeProperties: "TransactionType,DocumentType");

            var docTypes = await _unitOfWork.DocumentType.GetAllAsync(x => !x.IsDeleted);

            viewModel.CreateLeaseCoopViewModel.DocumentTypeList =
                _documentTransactionService.GetDocumentTypesFromDocumentTypeAssignment(
                    transactionTypeName,
                    assignments.ToList(),
                    docTypes.ToList()
                );
        }

        private void SetTabMenus(CreateLeaseCoopWithTabMenuViewModel viewModel, string controllerName)
        {
            var menus = CreateDealTransactionTabMenuData.GetCreateDealTransactionTabMenuData();

            foreach (var menu in menus.Where(x => x.Menu == controllerName))
            {
                menu.IsActive = "active";
                menu.IsSelected = "true";
            }

            viewModel.Menus = menus;
        }

        private async Task SendDealAdminNotification(ApplicationUser user, LeaseCoop leaseCoop)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var dealLink = Url.Action("Login", "Account", new { id = leaseCoop.Id, token }, Request.Scheme);
            var dealAdminEmails = await GetDealAdminEmails();
            var userRoles = await _userManager.GetRolesAsync(user);

            var emailRequest = EmailRequest.SendNotificationWhenDealIsSubmitted(
                user,
                userRoles,
                leaseCoop.TransactionType.Description,
                dealAdminEmails,
                leaseCoop.DealTransactionNumber,
                dealLink!
            );

            await _emailService.SendEmailAsync(emailRequest);
        }

        #endregion
    }
}
