using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.UserVM;
using Intl.Realty.Firm.Models.DataTable;
using Intl.Realty.Firm.Models;
using Intl.Realty.Firm.Utility.MenuData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Intl.Realty.Firm.Utility.Utilities;
using Intl.Realty.Firm.Service.IService;
using Intl.Realty.Firm.Utility.Mapper;
using Intl.Realty.Firm.Repository.IRepository;

namespace Intl.Realty.Firm.Controllers
{
    public class UserController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserController> _logger;
        private readonly IExportService<UserExport> _exportService;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(RoleManager<ApplicationRole> roleManager,
                              UserManager<ApplicationUser> userManager,
                              ILogger<UserController> logger,
                              IExportService<UserExport> exportService,
                              IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _exportService = exportService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email!,
                UserName = user.UserName!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DepartmentId = user.DepartmentId,
                EmploymentStatusId = user.EmploymentStatusId,
                ProfilePictureId = user.ProfilePictureId,
                Claims = userClaims.Select(x => x.Type + " : " + x.Value).ToList(),
                Roles = userRoles.ToList()
            };
            var departmentIEnum = await _unitOfWork.Department.GetAllAsync(x => x.IsDeleted == false);
            viewModel.DepartmentList = departmentIEnum.ToList();

            var employmentStatusIEnum = await _unitOfWork.EmploymentStatus.GetAllAsync(x => x.IsDeleted == false);
            viewModel.EmploymentStatusList = employmentStatusIEnum.ToList();

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.Id.ToString());
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {viewModel.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = viewModel.Email;
                user.UserName = viewModel.UserName;
                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                user.DepartmentId = viewModel.DepartmentId;
                user.EmploymentStatusId = viewModel.EmploymentStatusId;
                user.ProfilePictureId = viewModel.ProfilePictureId;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(viewModel);
            }

        }
        [HttpGet]
        public async Task<IActionResult> Preview(Guid id)
        {
            var model = await _userManager.FindByIdAsync(id.ToString());

            if (model == null)
            {
                return NotFound();
            }

            var viewModel = model.ToPreviewUserViewModel();

            var department = await _unitOfWork.Department.GetAsync(x => x.Id == model.DepartmentId);
            if (department == null)
            {
                viewModel.DepartmentName = "No Department Assigned";
            }
            var employmentStatus = await _unitOfWork.EmploymentStatus.GetAsync(x => x.Id == model.EmploymentStatusId);

            viewModel.EmploymentStatusName = employmentStatus.Description;

            var userRoles = await _userManager.GetRolesAsync(model);
            viewModel.Roles = userRoles.ToList();
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult ListPartialView(UserListPartialWithTabMenuViewModel listViewModel)
        {
            var users = _userManager.Users.ToList();

            listViewModel.Users = users;

            var menus = AdministrationTabMenuData.GetAdministrationTabMenuData();

            string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;

            foreach (var menu in menus.Where(x => x.Menu == controllerName))
            {
                menu.IsActive = "active";
                menu.IsSelected = "true";
            }

            listViewModel.Menus = menus;

            return PartialView("~/Views/User/Partial/ListPartial.cshtml", listViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> GetUserList([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            // Default order criteria
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            // Initialize order criteria
            (orderCriteria, orderAscendingDirection) = InitializeOrderCriteria(dtParameters, orderCriteria, orderAscendingDirection);

            // Query initialization
            var result = _userManager.Users;

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
            var totalResultsCount = _userManager.Users.Count();

            // Prepare the result for DataTables
            var jsonResult = new DtResult<ApplicationUser>
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

        private IQueryable<ApplicationUser> DefaultFilterSearch(IQueryable<ApplicationUser> result, string searchBy)
        {
            result = result.Where(r => r.FirstName != null && r.FirstName.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.LastName! != null && r.LastName.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.Email! != null && r.Email.ToUpper().Contains(searchBy.ToUpper())
                   );
            return result;
        }
        private IQueryable<ApplicationUser> FilterSearchWithParameters(IQueryable<ApplicationUser> query, string searchBy)
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
                    case "firstname":
                        query = query.Where(r => r.FirstName != null && r.FirstName.ToUpper().Contains(value));
                        break;
                    case "lastname":
                        query = query.Where(r => r.LastName != null && r.LastName.ToUpper().Contains(value));
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

                var result = _userManager.Users;

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

                var resultListForExport = resultList.ToUserListExport();

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
