using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.EmailStatusVM;
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
    public class EmailStatusController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExportService<EmailStatusExport> _exportService;
        private readonly IConfigurationService _configurationService;
        private readonly IFileHandlerService _fileHandlerService;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailStatusController(IUnitOfWork unitOfWork,
                                    IExportService<EmailStatusExport> exportService,
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
            string controllerName = ControllerContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;

            // Initialize the ViewModel with menus
            var viewModel = new CreateEmailStatusWithTabMenuViewModel
            {
                Menus = AdministrationTabMenuData.GetAdministrationTabMenuData()
                          .Select(menu =>
                          {
                              menu.IsActive = menu.Menu == controllerName ? "active" : string.Empty;
                              menu.IsSelected = menu.Menu == controllerName ? "true" : "false";
                              return menu;
                          })
                          .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmailStatusWithTabMenuViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // to get current user ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // to get current user info
                var user = await _userManager.FindByIdAsync(userId!);

                string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;

                viewModel.CreateEmailStatus.IsActive = true;
                viewModel.CreateEmailStatus.IsDeleted = false;
                viewModel.CreateEmailStatus.CreatedBy = user!.Id;
                viewModel.CreateEmailStatus.CreatedAt = DateTime.Now;

                var checkIfExists = await _unitOfWork.EmailStatus.GetAsync(x => x.Description == viewModel.CreateEmailStatus.Description && !x.IsDeleted);
                if (checkIfExists == null)
                {
                    var model = viewModel.CreateEmailStatus.ToEmailStatusModel();
                    await _unitOfWork.EmailStatus.AddAsync(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"{controllerName} already exists");
                    return View();
                }

            }

            return RedirectToAction(nameof(Index), new { addSuccess = true });
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest("Invalid ID");
            }

            // Fetch the model
            var model = await _unitOfWork.EmailStatus.GetAsync(x => x.Id == id);

            if (model == null)
            {
                string controllerName = ControllerContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;

                var notFoundViewModel = new NotFoundViewModel
                {
                    ErrorTitle = $"{controllerName} not found",
                    ErrorMessage = $"{controllerName} with ID {id} cannot be found!",
                    ControllerName = controllerName,
                    ControllerAction = "Index"
                };

                return View("NotFound", notFoundViewModel);
            }

            // Fetch current user info
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Forbid("User information could not be retrieved.");
            }

            // Initialize the ViewModel
            var viewModel = new EditEmailStatusWithTabMenuViewModel
            {
                EditEmailStatus = model.ToEditEmailStatusViewModel(),
                Menus = AdministrationTabMenuData.GetAdministrationTabMenuData()
                          .Select(menu =>
                          {
                              menu.IsActive = menu.Menu == ControllerContext.RouteData.Values["controller"]?.ToString() ? "active" : string.Empty;
                              menu.IsSelected = menu.Menu == ControllerContext.RouteData.Values["controller"]?.ToString() ? "true" : "false";
                              return menu;
                          })
                          .ToList()
            };

            viewModel.EditEmailStatus.UpdatedBy = user.Id;
            viewModel.EditEmailStatus.UpdatedAt = DateTime.UtcNow;

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditEmailStatusWithTabMenuViewModel viewModel)
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

                var modelFromDb = await _unitOfWork.EmailStatus.GetAsync(x => x.Id == viewModel.EditEmailStatus.Id);

                string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;

                var model = viewModel.EditEmailStatus.ToEmailStatusModel();

                model.CreatedAt = modelFromDb.CreatedAt;
                model.CreatedBy = modelFromDb.CreatedBy;

                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = user!.Id;

                await _unitOfWork.EmailStatus.UpdateAsync(model);
            }

            return RedirectToAction(nameof(Index), new { editSuccess = true });

        }
        [HttpGet]
        public async Task<IActionResult> Preview(int id)
        {
            var model = await _unitOfWork.EmailStatus.GetAsync(x => x.Id == id);
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

            var viewModel = model.ToPreviewEmailStatusViewModel();

            return View(viewModel);
        }
        [HttpGet]
        public IActionResult ListPartialView(EmailStatusListPartialViewModel listViewModel)
        {
            string controllerName = ControllerContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;

            // Update menus with active state based on the current controller
            listViewModel.Menus = AdministrationTabMenuData.GetAdministrationTabMenuData()
                                    .Select(menu =>
                                    {
                                        menu.IsActive = menu.Menu == controllerName ? "active" : string.Empty;
                                        menu.IsSelected = menu.Menu == controllerName ? "true" : "false";
                                        return menu;
                                    })
                                    .ToList();

            return PartialView("~/Views/EmailStatus/Partial/ListPartial.cshtml", listViewModel);
        }

        public async Task<IActionResult> DeleteMultipleModal(List<int> ids)
        {
            var modelIEnum = await _unitOfWork.EmailStatus.GetAllAsync(x => ids.Contains(x.Id));

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

            var deleteEmailStatusIEnumViewModel = modelIEnum.ToDeleteEmailStatusIEnumViewModel();

            return PartialView("~/Views/EmailStatus/Modal/DeleteMultipleModal.cshtml", deleteEmailStatusIEnumViewModel);
        }

        public async Task<IActionResult> DeleteMultiple(IEnumerable<DeleteEmailStatusViewModel> viewModelList)
        {
            if (viewModelList == null || !viewModelList.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            var ids = viewModelList.Select(x => x.Id).ToList();

            var modelList = await _unitOfWork.EmailStatus.GetAllAsync(x => ids.Contains(x.Id));

            if (modelList != null)
            {
                try
                {
                    await _unitOfWork.EmailStatus.RemoveRangeAsync(modelList);

                    return RedirectToAction(nameof(Index), new { deleteSuccess = true });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SoftDeleteMultiple(IEnumerable<DeleteEmailStatusViewModel> viewModelList)
        {
            if (viewModelList == null || !viewModelList.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            var ids = viewModelList.Select(x => x.Id).ToList();

            var modelList = await _unitOfWork.EmailStatus.GetAllAsync(x => ids.Contains(x.Id));

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
                    await _unitOfWork.EmailStatus.UpdateRangeAsync(modelList);

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
        public async Task<IActionResult> GetEmailStatusList([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            // Default order criteria
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            // Initialize order criteria
            (orderCriteria, orderAscendingDirection) = InitializeOrderCriteria(dtParameters, orderCriteria, orderAscendingDirection);

            // Query initialization
            var result = await _unitOfWork.EmailStatus.AsQueryableAsync(x => x.IsActive && x.IsDeleted == false);
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
            var jsonResult = new DtResult<EmailStatus>
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

        private IQueryable<EmailStatus> DefaultFilterSearch(IQueryable<EmailStatus> result, string searchBy)
        {
            result = result.Where(r => r.Code != null && r.Code.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.Description! != null && r.Description.ToUpper().Contains(searchBy.ToUpper())
                   );
            return result;
        }
        private IQueryable<EmailStatus> FilterSearchWithParameters(IQueryable<EmailStatus> query, string searchBy)
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

                var result = _unitOfWork.EmailStatus.AsQueryable(x => x.IsDeleted == false);

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

                var resultListForExport = resultList.ToEmailStatusListExport();

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
