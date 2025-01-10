using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.DealStatusVM;
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
    public class DealStatusController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExportService<DealStatusExport> _exportService;
        private readonly IConfigurationService _configurationService;
        private readonly IFileHandlerService _fileHandlerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string _backupFileDirectory = "\\wwwroot\\Files\\Uploads\\";

        public DealStatusController(IUnitOfWork unitOfWork,
                                    IExportService<DealStatusExport> exportService,
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
        public IActionResult Create()
        {
            var menus = AdministrationTabMenuData.GetAdministrationTabMenuData().ToList();
            string controllerName = ControllerContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;

            // Update menu items based on the current controller
            foreach (var menu in menus)
            {
                menu.IsActive = menu.Menu == controllerName ? "active" : string.Empty;
                menu.IsSelected = menu.Menu == controllerName ? "true" : "false";
            }

            var viewModel = new CreateDealStatusWithTabMenuViewModel
            {
                Menus = menus
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDealStatusWithTabMenuViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // to get current user ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // to get current user info
                var user = await _userManager.FindByIdAsync(userId!);

                string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;

                viewModel.CreateDealStatus.IsActive = true;
                viewModel.CreateDealStatus.IsDeleted = false;
                viewModel.CreateDealStatus.CreatedBy = user!.Id;
                viewModel.CreateDealStatus.CreatedAt = DateTime.Now;

                var checkIfExists = await _unitOfWork.DealStatus.GetAsync(x => x.Description == viewModel.CreateDealStatus.Description);
                if (checkIfExists == null)
                {
                    var model = viewModel.CreateDealStatus.ToDealStatusModel();
                    await _unitOfWork.DealStatus.AddAsync(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"{controllerName} already exists");
                    return View();
                }

            }

            return RedirectToAction(nameof(Index), new { addSuccess = true });
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest("Invalid ID");
            }

            // Fetch the model
            var model = await _unitOfWork.DealStatus.GetAsync(x => x.Id == id);
            string controllerName = ControllerContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;

            if (model == null)
            {
                return View("NotFound", new NotFoundViewModel
                {
                    ErrorTitle = $"{controllerName} not found",
                    ErrorMessage = $"{controllerName} with ID {id} cannot be found!",
                    ControllerName = controllerName,
                    ControllerAction = "Index"
                });
            }

            // Fetch current user info
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            if (user == null)
            {
                return Forbid("User information could not be retrieved.");
            }

            // Initialize the ViewModel
            var viewModel = new EditDealStatusWithTabMenuViewModel
            {
                EditDealStatus = model.ToEditDealStatusViewModel()
            };

            viewModel.EditDealStatus.UpdatedBy = user.Id;
            viewModel.EditDealStatus.UpdatedAt = DateTime.UtcNow;

            // Fetch and configure menus
            var menus = AdministrationTabMenuData.GetAdministrationTabMenuData().ToList();

            foreach (var menu in menus)
            {
                menu.IsActive = menu.Menu == controllerName ? "active" : string.Empty;
                menu.IsSelected = menu.Menu == controllerName ? "true" : "false";
            }

            viewModel.Menus = menus;

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditDealStatusWithTabMenuViewModel viewModel)
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

                var modelFromDb = await _unitOfWork.DealStatus.GetAsync(x => x.Id == viewModel.EditDealStatus.Id);

                string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;

                var model = viewModel.EditDealStatus.ToDealStatusModel();

                model.CreatedAt = modelFromDb.CreatedAt;
                model.CreatedBy = modelFromDb.CreatedBy;

                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = user!.Id;

                await _unitOfWork.DealStatus.UpdateAsync(model);
            }

            return RedirectToAction(nameof(Index), new { editSuccess = true });

        }
        [HttpGet]
        public async Task<IActionResult> Preview(int id)
        {
            var model = await _unitOfWork.DealStatus.GetAsync(x => x.Id == id);
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

            var viewModel = model.ToPreviewDealStatusViewModel();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ListPartialView(DealStatusListPartialViewModel listViewModel)
        {
            var menus = TransactionTabMenuData.GetTransactionTabMenuData();

            string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;

            foreach (var menu in menus.Where(x => x.Menu == controllerName))
            {
                menu.IsActive = "active";
                menu.IsSelected = "true";
            }

            listViewModel.Menus = menus;

            return PartialView("~/Views/DealStatus/Partial/ListPartial.cshtml", listViewModel);
        }
        public async Task<IActionResult> DeleteMultipleModal(List<int> ids)
        {
            var modelIEnum = await _unitOfWork.DealStatus.GetAllAsync(x => ids.Contains(x.Id));

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

            var deleteDealStatusIEnumViewModel = modelIEnum.ToDeleteDealStatusIEnumViewModel();

            return PartialView("~/Views/DealStatus/Modal/DeleteMultipleModal.cshtml", deleteDealStatusIEnumViewModel);
        }

        public async Task<IActionResult> DeleteMultiple(IEnumerable<DeleteDealStatusViewModel> viewModelList)
        {
            if (viewModelList == null || !viewModelList.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            var ids = viewModelList.Select(x => x.Id).ToList();

            var modelList = await _unitOfWork.DealStatus.GetAllAsync(x => ids.Contains(x.Id));

            if (modelList != null)
            {
                try
                {
                    await _unitOfWork.DealStatus.RemoveRangeAsync(modelList);

                    return RedirectToAction(nameof(Index), new { deleteSuccess = true });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SoftDeleteMultiple(IEnumerable<DeleteDealStatusViewModel> viewModelList)
        {
            if (viewModelList == null || !viewModelList.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            var ids = viewModelList.Select(x => x.Id).ToList();

            var modelList = await _unitOfWork.DealStatus.GetAllAsync(x => ids.Contains(x.Id));

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
                    await _unitOfWork.DealStatus.UpdateRangeAsync(modelList);

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
        public async Task<IActionResult> GetDealStatusList([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            // Default order criteria
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            // Initialize order criteria
            (orderCriteria, orderAscendingDirection) = InitializeOrderCriteria(dtParameters, orderCriteria, orderAscendingDirection);

            // Query initialization
            var result = await _unitOfWork.DealStatus.AsQueryableAsync(x => x.IsDeleted == false);
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
            var jsonResult = new DtResult<DealStatus>
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

        private IQueryable<DealStatus> DefaultFilterSearch(IQueryable<DealStatus> result, string searchBy)
        {
            result = result.Where(r => r.Code != null && r.Code.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.Description! != null && r.Description.ToUpper().Contains(searchBy.ToUpper())
                   );
            return result;
        }
        private IQueryable<DealStatus> FilterSearchWithParameters(IQueryable<DealStatus> query, string searchBy)
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
                    case "code":
                        query = query.Where(r => r.Code != null && r.Code.ToUpper().Contains(value));
                        break;
                    case "description":
                        query = query.Where(r => r.Description != null && r.Description.ToUpper().Contains(value));
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

                var result = _unitOfWork.DealStatus.AsQueryable(x => x.IsDeleted == false);

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

                var resultListForExport = resultList.ToDealStatusListExport();

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
