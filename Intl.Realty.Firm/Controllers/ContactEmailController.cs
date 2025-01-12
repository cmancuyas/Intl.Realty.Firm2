using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.ContactEmailVM;
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

namespace Intl.Realty.Firm.Controllers
{
    [Authorize]
    public class ContactEmailController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExportService<ContactEmailExport> _exportService;
        private readonly IConfigurationService _configurationService;
        private readonly IFileHandlerService _fileHandlerService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ContactEmailController(IUnitOfWork unitOfWork,
                                    IExportService<ContactEmailExport> exportService,
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
            string controllerName = ControllerContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;

            // Initialize the ViewModel with menus
            var viewModel = new CreateContactEmailWithTabMenuViewModel
            {
                Menus = AdministrationTabMenuData.GetAdministrationTabMenuData()
                          .Select(menu =>
                          {
                              menu.IsActive = menu.Menu == controllerName ? "active" : string.Empty;
                              menu.IsSelected = menu.Menu == controllerName ? "true" : "false";
                              return menu;
                          })
                          .ToList(),
            };
            var emailCategoryIEnum = await _unitOfWork.EmailCategory.GetAllAsync(x => x.IsDeleted == false);
            viewModel.CreateContactEmail.EmailCategoryIEnum = SelectListConverter.CreateSelectList(emailCategoryIEnum.ToList(), x => x.Id, x => x.Description);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateContactEmailWithTabMenuViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Reload dropdown data if ModelState is invalid
                var emailCategoryIEnum = await _unitOfWork.EmailCategory.GetAllAsync(x => !x.IsDeleted);
                viewModel.CreateContactEmail.EmailCategoryIEnum = SelectListConverter.CreateSelectList(emailCategoryIEnum.ToList(), x => x.Id, x => x.Description);

                return View(viewModel);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId!);

                // Map input to model
                var newContactEmail = viewModel.CreateContactEmail.ToContactEmailModel();
                newContactEmail.IsActive = true;
                newContactEmail.IsDeleted = false;
                newContactEmail.CreatedBy = user!.Id;
                newContactEmail.CreatedAt = DateTime.Now;

                // Check for duplicates
                var existingContactEmail = await _unitOfWork.ContactEmail.GetAsync(x =>
                    x.EmailCategoryId == viewModel.CreateContactEmail.EmailCategoryId &&
                    x.Email == viewModel.CreateContactEmail.Email &&
                    !x.IsDeleted, includeProperties:"EmailCategory");

                if (existingContactEmail != null)
                {
                    ModelState.AddModelError(string.Empty, "A contact email with this category and email already exists.");
                    return View(viewModel);
                }

                // Add and save
                await _unitOfWork.ContactEmail.AddAsync(newContactEmail);
                await _unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index), new { addSuccess = true });
            }
            catch (Exception ex)
            {
                // Log the error
                ModelState.AddModelError(string.Empty, "An error occurred while saving the data.");
                Console.WriteLine(ex.Message); // Replace with proper logging
                return View(viewModel);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest("Invalid ID");
            }

            // Fetch the model
            var model = await _unitOfWork.ContactEmail.GetAsync(x => x.Id == id);

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
            var viewModel = new EditContactEmailWithTabMenuViewModel
            {
                EditContactEmail = model.ToEditContactEmailViewModel(),
                Menus = AdministrationTabMenuData.GetAdministrationTabMenuData()
                          .Select(menu =>
                          {
                              menu.IsActive = menu.Menu == ControllerContext.RouteData.Values["controller"]?.ToString() ? "active" : string.Empty;
                              menu.IsSelected = menu.Menu == ControllerContext.RouteData.Values["controller"]?.ToString() ? "true" : "false";
                              return menu;
                          })
                          .ToList()
            };

            var emailCategoryIEnum = await _unitOfWork.EmailCategory.GetAllAsync(x => x.IsDeleted == false);
            viewModel.EditContactEmail.EmailCategoryList = emailCategoryIEnum.ToList();
            viewModel.EditContactEmail.UpdatedBy = user.Id;
            viewModel.EditContactEmail.UpdatedAt = DateTime.UtcNow;

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditContactEmailWithTabMenuViewModel viewModel)
        {
            var emailCategoryIEnum = await _unitOfWork.EmailCategory.GetAllAsync(x => x.IsDeleted == false);
            viewModel.EditContactEmail.EmailCategoryList = emailCategoryIEnum.ToList();

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

                var modelFromDb = await _unitOfWork.ContactEmail.GetAsync(x => x.Id == viewModel.EditContactEmail.Id, includeProperties: "EmailCategory");

                string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;

                var model = viewModel.EditContactEmail.ToContactEmailModel();

                model.CreatedAt = modelFromDb.CreatedAt;
                model.CreatedBy = modelFromDb.CreatedBy;
                model.EmailCategory = modelFromDb?.EmailCategory;
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = user!.Id;

                await _unitOfWork.ContactEmail.UpdateAsync(model);
            }

            return RedirectToAction(nameof(Index), new { editSuccess = true });

        }
        [HttpGet]
        public async Task<IActionResult> Preview(int id)
        {
            var model = await _unitOfWork.ContactEmail.GetAsync(x => x.Id == id, includeProperties:"EmailCategory");
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

            var viewModel = model.ToPreviewContactEmailViewModel();

            return View(viewModel);
        }
        [HttpGet]
        public IActionResult ListPartialView(ContactEmailListPartialViewModel listViewModel)
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

            return PartialView("~/Views/ContactEmail/Partial/ListPartial.cshtml", listViewModel);
        }

        public async Task<IActionResult> DeleteMultipleModal(List<int> ids)
        {
            var modelIEnum = await _unitOfWork.ContactEmail.GetAllAsync(x => ids.Contains(x.Id));

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

            var deleteContactEmailIEnumViewModel = modelIEnum.ToDeleteContactEmailIEnumViewModel();

            return PartialView("~/Views/ContactEmail/Modal/DeleteMultipleModal.cshtml", deleteContactEmailIEnumViewModel);
        }

        public async Task<IActionResult> DeleteMultiple(IEnumerable<DeleteContactEmailViewModel> viewModelList)
        {
            if (viewModelList == null || !viewModelList.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            var ids = viewModelList.Select(x => x.Id).ToList();

            var modelList = await _unitOfWork.ContactEmail.GetAllAsync(x => ids.Contains(x.Id));

            if (modelList != null)
            {
                try
                {
                    await _unitOfWork.ContactEmail.RemoveRangeAsync(modelList);

                    return RedirectToAction(nameof(Index), new { deleteSuccess = true });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SoftDeleteMultiple(IEnumerable<DeleteContactEmailViewModel> viewModelList)
        {
            if (viewModelList == null || !viewModelList.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            var ids = viewModelList.Select(x => x.Id).ToList();

            var modelList = await _unitOfWork.ContactEmail.GetAllAsync(x => ids.Contains(x.Id));

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
                    await _unitOfWork.ContactEmail.UpdateRangeAsync(modelList);

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
        public async Task<IActionResult> GetContactEmailList([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            // Default order criteria
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            // Initialize order criteria
            (orderCriteria, orderAscendingDirection) = InitializeOrderCriteria(dtParameters, orderCriteria, orderAscendingDirection);

            // Query initialization
            var result = await _unitOfWork.ContactEmail.AsQueryableAsync(x => x.IsDeleted == false, includeProperties:"EmailCategory");
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
            var jsonResult = new DtResult<ContactEmail>
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

        private IQueryable<ContactEmail> DefaultFilterSearch(IQueryable<ContactEmail> result, string searchBy)
        {
            result = result.Where(r => r.EmailCategory!.Description != null && r.EmailCategory!.Description.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.Email! != null && r.Email.ToUpper().Contains(searchBy.ToUpper())
                   );
            return result;
        }
        private IQueryable<ContactEmail> FilterSearchWithParameters(IQueryable<ContactEmail> query, string searchBy)
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
                    case "emailCategory":
                        query = query.Where(r => r.EmailCategory!.Description != null && r.EmailCategory!.Description.ToUpper().Contains(value));
                        break;
                    case "email":
                        query = query.Where(r => r.Email != null && r.Email.ToUpper().Contains(value));
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

                var result = _unitOfWork.ContactEmail.AsQueryable(x => x.IsDeleted == false);

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

                var resultListForExport = resultList.ToContactEmailListExport();

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
