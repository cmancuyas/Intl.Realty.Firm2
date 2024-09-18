using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.DocumentTypeAssignmentVM;
using Intl.Realty.Firm.Models;
using Intl.Realty.Firm.Models.DataTable;
using Intl.Realty.Firm.Repository.IRepository;
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

namespace DENR_FASPIS.Controllers
{
    public class DocumentTypeAssignmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExportService<DocumentTypeAssignmentExport> _exportService;
        private readonly IConfigurationService _configurationService;
        private readonly IFileHandlerService _fileHandlerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string _backupFileDirectory = "\\wwwroot\\Files\\Uploads\\";

        public DocumentTypeAssignmentController(IUnitOfWork unitOfWork,
                                    IExportService<DocumentTypeAssignmentExport> exportService,
                                    IConfigurationService configurationService,
                                    IFileHandlerService fileHandlerService,
                                    UserManager<ApplicationUser> userManager
                                                                        )
        {
            _unitOfWork = unitOfWork;
            _exportService = exportService;
            _configurationService = configurationService;
            _fileHandlerService = fileHandlerService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateDocumentTypeAssignmentWithTabMenuViewModel viewModel = new CreateDocumentTypeAssignmentWithTabMenuViewModel();

            var documentTypeIEnum = await _unitOfWork.DocumentType.GetAllAsync(x => x.IsDeleted == false);
            viewModel.CreateDocumentTypeAssignment.DocumentTypeIEnum = SelectListConverter.CreateSelectList(documentTypeIEnum.ToList(), x => x.Id, x => x.Description);

            var transactionTypeIEnum = await _unitOfWork.TransactionType.GetAllAsync(x => x.IsDeleted == false);
            viewModel.CreateDocumentTypeAssignment.TransactionTypeIEnum = SelectListConverter.CreateSelectList(transactionTypeIEnum.ToList(), x => x.Id, x => x.Description);

            return View(viewModel);
        }
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsAssignmentExists(int transactionTypeId, int documentTypeId)
        {
            var combinedExp = LinqExtensions.AndAlso<DocumentTypeAssignment>(x => x.TransactionTypeId == transactionTypeId,
                                                                              x => x.DocumentTypeId == documentTypeId);
            var checkIfExists = await _unitOfWork.DocumentTypeAssignment.GetAsync(combinedExp);
            if (combinedExp == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Document Type Assignment already exists");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDocumentTypeAssignmentWithTabMenuViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // to get current user ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // to get current user info
                var user = await _userManager.FindByIdAsync(userId!);

                string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;

                viewModel.CreateDocumentTypeAssignment.IsActive = true;
                viewModel.CreateDocumentTypeAssignment.IsDeleted = false;
                viewModel.CreateDocumentTypeAssignment.CreatedBy = user!.Id;
                viewModel.CreateDocumentTypeAssignment.CreatedAt = DateTime.UtcNow;

                var combinedExp = LinqExtensions.AndAlso<DocumentTypeAssignment>(x => x.TransactionTypeId == viewModel.CreateDocumentTypeAssignment.TransactionTypeId,
                                                                                  x => x.DocumentTypeId == viewModel.CreateDocumentTypeAssignment.DocumentTypeId);

                var checkIfExists = await _unitOfWork.DocumentTypeAssignment.GetAsync(combinedExp);
                if (checkIfExists == null)
                {
                    var model = viewModel.CreateDocumentTypeAssignment.ToDocumentTypeAssignmentModel();
                    await _unitOfWork.DocumentTypeAssignment.AddAsync(model);
                }
                else
                {
                    var documentTypeIEnum = await _unitOfWork.DocumentType.GetAllAsync(x => x.IsDeleted == false);
                    viewModel.CreateDocumentTypeAssignment.DocumentTypeIEnum = SelectListConverter.CreateSelectList(documentTypeIEnum.ToList(), x => x.Id, x => x.Description);

                    var transactionTypeIEnum = await _unitOfWork.TransactionType.GetAllAsync(x => x.IsDeleted == false);
                    viewModel.CreateDocumentTypeAssignment.TransactionTypeIEnum = SelectListConverter.CreateSelectList(transactionTypeIEnum.ToList(), x => x.Id, x => x.Description);
                    ModelState.AddModelError("", "Assignment already exists");
                    return View(viewModel);
                }

            }

            return RedirectToAction(nameof(Index), new { addSuccess = true });
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var model = await _unitOfWork.DocumentTypeAssignment.GetAsync(x => x.Id == id);

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
            EditDocumentTypeAssignmentWithTabMenuViewModel viewModel = new EditDocumentTypeAssignmentWithTabMenuViewModel();

            // to get current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // to get current user info
            var user = await _userManager.FindByIdAsync(userId!);

            viewModel.EditDocumentTypeAssignment = model.ToEditDocumentTypeAssignmentViewModel();

            var documentTypeIEnum = await _unitOfWork.DocumentType.GetAllAsync(x => x.IsDeleted == false);
            var transactionTypeIEnum = await _unitOfWork.TransactionType.GetAllAsync(x => x.IsDeleted == false);
            viewModel.EditDocumentTypeAssignment.DocumentTypeList = documentTypeIEnum.ToList();
            viewModel.EditDocumentTypeAssignment.TransactionTypeList = transactionTypeIEnum.ToList();

            viewModel.EditDocumentTypeAssignment.UpdatedBy = user!.Id;
            viewModel.EditDocumentTypeAssignment.UpdatedAt = DateTime.Now;

            var menus = AdministrationTabMenuData.GetAdministrationTabMenuData();

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
        public async Task<IActionResult> Edit(int id, EditDocumentTypeAssignmentWithTabMenuViewModel viewModel)
        {
            if (id == 0)
            {
                string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;
                NotFoundViewModel notFoundViewModel = new NotFoundViewModel();
                notFoundViewModel.ErrorTitle = ($"{controllerName} not found");
                notFoundViewModel.ErrorMessage = ($"{controllerName} with {id} cannot be found!");
                notFoundViewModel.ControllerName = controllerName;
                notFoundViewModel.ControllerAction = "Index";

                return View("NotFound", notFoundViewModel);
            }

            if (ModelState.IsValid)
            {
                // to get current user ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // to get current user info
                var user = await _userManager.FindByIdAsync(userId!);

                var modelFromDb = await _unitOfWork.DocumentTypeAssignment.GetAsync(x => x.Id == viewModel.EditDocumentTypeAssignment.Id);

                string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;

                var model = viewModel.EditDocumentTypeAssignment.ToDocumentTypeAssignmentModel();

                model.CreatedAt = modelFromDb.CreatedAt;
                model.CreatedBy = modelFromDb.CreatedBy;

                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = user!.Id;

                await _unitOfWork.DocumentTypeAssignment.UpdateAsync(model);
            }

            return RedirectToAction(nameof(Index), new { editSuccess = true });

        }
        [HttpGet]
        public async Task<IActionResult> Preview(int id)
        {
            var model = await _unitOfWork.DocumentTypeAssignment.GetAsync(x => x.Id == id);
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

            var viewModel = model.ToPreviewDocumentTypeAssignmentViewModel();

            viewModel.TransactionType = await _unitOfWork.TransactionType.GetAsync(x => x.Id == model.TransactionTypeId);
            viewModel.DocumentType = await _unitOfWork.DocumentType.GetAsync(x => x.Id == model.DocumentTypeId);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ListPartialView(DocumentTypeAssignmentListPartialViewModel listViewModel)
        {
            var menus = TransactionTabMenuData.GetTransactionTabMenuData();

            string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;

            foreach (var menu in menus.Where(x => x.Menu == controllerName))
            {
                menu.IsActive = "active";
                menu.IsSelected = "true";
            }

            listViewModel.Menus = menus;

            return PartialView("~/Views/DocumentTypeAssignment/Partial/ListPartial.cshtml", listViewModel);
        }
        public async Task<IActionResult> DeleteMultipleModal(List<int> ids)
        {
            var modelIEnum = await _unitOfWork.DocumentTypeAssignment.GetAllAsync(x => ids.Contains(x.Id));

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

            var deleteDocumentTypeAssignmentIEnumViewModel = modelIEnum.ToDeleteDocumentTypeAssignmentIEnumViewModel();

            return PartialView("~/Views/DocumentTypeAssignment/Modal/DeleteMultipleModal.cshtml", deleteDocumentTypeAssignmentIEnumViewModel);
        }

        public async Task<IActionResult> DeleteMultiple(IEnumerable<DeleteDocumentTypeAssignmentViewModel> viewModelList)
        {
            if (viewModelList == null || !viewModelList.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            var ids = viewModelList.Select(x => x.Id).ToList();

            var modelList = await _unitOfWork.DocumentTypeAssignment.GetAllAsync(x => ids.Contains(x.Id));

            if (modelList != null)
            {
                try
                {
                    await _unitOfWork.DocumentTypeAssignment.RemoveRangeAsync(modelList);

                    return RedirectToAction(nameof(Index), new { deleteSuccess = true });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SoftDeleteMultiple(IEnumerable<DeleteDocumentTypeAssignmentViewModel> viewModelList)
        {
            if (viewModelList == null || !viewModelList.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            var ids = viewModelList.Select(x => x.Id).ToList();

            var modelList = await _unitOfWork.DocumentTypeAssignment.GetAllAsync(x => ids.Contains(x.Id));

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
                    await _unitOfWork.DocumentTypeAssignment.UpdateRangeAsync(modelList);

                    return RedirectToAction(nameof(Index), new { deleteSuccess = true });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> GetDocumentTypeAssignmentList([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            // Default order criteria
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            // Initialize order criteria
            (orderCriteria, orderAscendingDirection) = InitializeOrderCriteria(dtParameters, orderCriteria, orderAscendingDirection);

            // Query initialization
            var result = await _unitOfWork.DocumentTypeAssignment.AsQueryableAsync(x => x.IsDeleted == false,includeProperties:"DocumentType,TransactionType");
            var totalResultsCount = result.Count();

            if (searchBy!.Contains(":") || searchBy.Contains("+"))
            {
                result = FilterSearchWithParameters(result, searchBy);
            }
            else
            {
                result = DefaultFilterSearch(result, searchBy);
            }

            // Apply ordering
            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria!, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria!, DtOrderDir.Desc);

            // Get the count of filtered and total results
            var filteredResultsCount = result.Count();

            // Prepare the result for DataTables
            var jsonResult = new DtResult<DocumentTypeAssignment>
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

        private IQueryable<DocumentTypeAssignment> DefaultFilterSearch(IQueryable<DocumentTypeAssignment> result, string searchBy)
        {
            result = result.Where(r => r.DocumentType!.Description != null && r.DocumentType.Description.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.TransactionType!.Description != null && r.TransactionType!.Description.ToUpper().Contains(searchBy.ToUpper())
                   );
            return result;
        }
        private IQueryable<DocumentTypeAssignment> FilterSearchWithParameters(IQueryable<DocumentTypeAssignment> query, string searchBy)
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
                    case "documentType":
                        query = query.Where(r => r.DocumentType!.Description != null && r.DocumentType.Description.ToUpper().Contains(value));
                        break;
                    case "transactionType":
                        query = query.Where(r => r.TransactionType!.Description != null && r.TransactionType!.Description.ToUpper().Contains(value));
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

                var result = _unitOfWork.DocumentTypeAssignment.AsQueryable(x => x.IsDeleted == false);

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

                var resultListForExport = resultList.ToDocumentTypeAssignmentListExport();

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
