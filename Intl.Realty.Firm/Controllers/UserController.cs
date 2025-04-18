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
using Microsoft.AspNetCore.Authorization;
using Intl.Realty.Firm.Model.ViewModel.AccountVM;
using Intl.Realty.Firm.Model.Model.Identity;
using System.Security.Claims;
using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.CustomerVM;
using Microsoft.IdentityModel.Tokens;

namespace Intl.Realty.Firm.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
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
            // Retrieve the user by ID
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return View("NotFound", new NotFoundViewModel
                {
                    ErrorTitle = "User not found",
                    ErrorMessage = $"User with ID = {id} cannot be found.",
                    ControllerName = "User",
                    ControllerAction = "Index"
                });
            }

            // Initialize the view model
            var viewModel = new EditUserWithTabMenuViewModel
            {
                EditUser = new EditUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email!,
                    UserName = user.Email!,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DepartmentId = user.DepartmentId,
                    EmploymentStatusId = user.EmploymentStatusId,
                    EmailConfirmed = user.EmailConfirmed,
                    ProfileImageId = user.ProfileImageId,
                    Claims = (await _userManager.GetClaimsAsync(user))
                        .Select(x => $"{x.Type} : {x.Value}")
                        .ToList(),
                    Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                    DepartmentList = (await _unitOfWork.Department.GetAllAsync(x => !x.IsDeleted)).ToList(),
                    EmploymentStatusList = (await _unitOfWork.EmploymentStatus.GetAllAsync(x => !x.IsDeleted)).ToList()
                },
                Menus = AdministrationTabMenuData.GetEditUserTabMenuData()
                    .Select(menu =>
                    {
                        menu.IsActive = menu.Menu == "EditUser" ? "active" : string.Empty;
                        menu.IsSelected = menu.Menu == "EditUser" ? "true" : "false";
                        return menu;
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserWithTabMenuViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.EditUser.Id.ToString());

            var departmentIEnum = await _unitOfWork.Department.GetAllAsync(x => x.IsDeleted == false);
            viewModel.EditUser.DepartmentList = departmentIEnum.ToList();

            var employmentStatusIEnum = await _unitOfWork.EmploymentStatus.GetAllAsync(x => x.IsDeleted == false);
            viewModel.EditUser.EmploymentStatusList = employmentStatusIEnum.ToList();

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {viewModel.EditUser.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = viewModel.EditUser.Email;
                user.UserName = viewModel.EditUser.Email;
                user.FirstName = viewModel.EditUser.FirstName;
                user.LastName = viewModel.EditUser.LastName;
                user.DepartmentId = viewModel.EditUser.DepartmentId;
                user.EmploymentStatusId = viewModel.EditUser.EmploymentStatusId;
                user.ProfileImageId = viewModel.EditUser.ProfileImageId;
                user.EmailConfirmed = viewModel.EditUser.EmailConfirmed;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
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
            var userModel = await _userManager.FindByIdAsync(id.ToString());

            if (userModel == null)
            {
                return NotFound();
            }

            var viewModel = userModel.ToPreviewUserViewModel();

            var department = await _unitOfWork.Department.GetAsync(x => x.Id == userModel.DepartmentId);
            if (department == null)
            {
                viewModel.DepartmentName = "No Department Assigned";
            }
            var employmentStatus = await _unitOfWork.EmploymentStatus.GetAsync(x => x.Id == userModel.EmploymentStatusId);

            viewModel.EmploymentStatusName = employmentStatus.Description;

            var userRoles = await _userManager.GetRolesAsync(userModel);
            var userClaims = await _userManager.GetClaimsAsync(userModel);
            viewModel.Roles = userRoles.ToList();
            viewModel.Claims = userClaims.Select(x => x.Type + " : " + x.Value).ToList();

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteMultipleModal([FromQuery] List<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("No users selected.");
            }

            // Fetch current user (optional)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userManager.FindByIdAsync(userId!);

            // Fetch users using UserManager
            var users = await Task.WhenAll(ids.Select(async id => await _userManager.FindByIdAsync(id.ToString())));
            users = users.Where(u => u != null).ToArray();

            if (!users.Any())  // Ensure data is present
            {
                string controllerName = this.ControllerContext.RouteData.Values["controller"]!.ToString()!;
                var notFoundViewModel = new NotFoundViewModel
                {
                    ErrorTitle = $"{controllerName} not found",
                    ErrorMessage = $"{controllerName} cannot be found!",
                    ControllerName = controllerName,
                    ControllerAction = "Index"
                };

                return View("NotFound", notFoundViewModel);
            }

            // Convert users to ViewModel
            var deleteUserViewModelList = users.Select(u => new DeleteUserViewModel
            {
                Id = u!.Id,
                DeletedAt = DateTime.Now,
                DeletedBy = currentUser!.Id
            }).ToList();

            return PartialView("~/Views/User/Modal/DeleteMultipleModal.cshtml", deleteUserViewModelList);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteMultipleModal(IEnumerable<DeleteUserViewModel> viewModelList)
        {
            if (viewModelList == null || !viewModelList.Any())
            {
                return Json(new { success = false, message = "No users selected for deletion." });
            }

            try
            {
                var errors = new List<string>();

                foreach (var viewModel in viewModelList)
                {
                    var user = await _userManager.FindByIdAsync(viewModel.Id.ToString());
                    if (user != null)
                    {
                        var result = await _userManager.DeleteAsync(user);
                        if (!result.Succeeded)
                        {
                            errors.Add($"Failed to delete user {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                    else
                    {
                        errors.Add($"User with ID {viewModel.Id} not found.");
                    }
                }

                if (errors.Any())
                {
                    return Json(new { success = false, message = "Some users could not be deleted.", errors });
                }

                return Json(new { success = true, message = "Selected users have been deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting users");
                return Json(new { success = false, message = "An error occurred while deleting users.", error = ex.Message });
            }
        }

        [HttpGet]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(Guid id)
        {
            // Validate the user ID
            if (id == Guid.Empty)
            {
                ViewBag.ErrorMessage = "Invalid User ID provided.";
                return View("NotFound");
            }

            // Fetch the user by ID
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with ID = {id} cannot be found.";
                return View("NotFound");
            }

            // Fetch all roles from RoleManager
            var roles = _roleManager.Roles.ToList();
            if (!roles.Any())
            {
                ViewBag.ErrorMessage = "No roles found in the system.";
                return View("NotFound");
            }

            // Initialize the view model
            var viewModel = new ManageUserRolesViewModel
            {
                UserId = user.Id,
                Roles = new List<UserRolesViewModel>()
            };

            // Process each role sequentially
            foreach (var role in roles)
            {
                // Check if the user is in the current role
                var isSelected = await _userManager.IsInRoleAsync(user, role.Name!);

                // Add the role to the view model
                viewModel.Roles.Add(new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name!,
                    IsSelected = isSelected
                });
            }

            // Return the populated view model to the view
            return View(viewModel);
        }



        [HttpPost]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());

            if (user == null)
            {
                ViewBag.ErrorMessage = $"(User with Id = {model.UserId.ToString()} cannot be found";
                return View("NotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(
                        user,
                        model.Roles.Where(x => x.IsSelected)
                            .Select(y => y.RoleName)
                        );
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Index", new { Id = model.UserId.ToString() });

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                ViewBag.ErrorMessage = ($"User with Id = {id.ToString()} cannot be found");
                return View("NotFound");
            }

            var existingUserClaims = await _userManager.GetClaimsAsync(user);

            ManageUserClaimsViewModel viewModel = new ManageUserClaimsViewModel();

            viewModel.UserId = user.Id.ToString();

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type,
                };

                if (existingUserClaims.Any(x => x.Type == claim.Type && x.Value == "true"))
                {
                    userClaim.IsSelected = true;
                };

                viewModel.Claims.Add(userClaim);
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(ManageUserClaimsViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(model);
            }

            result = await _userManager.AddClaimsAsync(user,
                        model.Claims
                            //.Where(c => c.IsSelected)
                            .Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false"))
                            .ToArray());

            return RedirectToAction("Index", new { Id = model.UserId });
        }
        [HttpGet]
        public IActionResult ListPartialView(UserListPartialWithTabMenuViewModel listViewModel)
        {
            // Fetch all users
            listViewModel.Users = _userManager.Users.ToList();

            // Fetch the administration tab menu data
            listViewModel.Menus = AdministrationTabMenuData.GetAdministrationTabMenuData()
                .Select(menu =>
                {
                    // Mark the active menu based on the current controller
                    menu.IsActive = menu.Menu == ControllerContext.RouteData.Values["controller"]?.ToString() ? "active" : string.Empty;
                    menu.IsSelected = menu.Menu == ControllerContext.RouteData.Values["controller"]?.ToString() ? "true" : "false";
                    return menu;
                })
                .ToList();

            // Return the partial view with the view model
            return PartialView("~/Views/User/Partial/ListPartial.cshtml", listViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GetUserList([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            // Ensure default pagination size is 10
            if (dtParameters.Length <= 0) dtParameters.Length = 10;

            (orderCriteria, orderAscendingDirection) = InitializeOrderCriteria(dtParameters, orderCriteria, orderAscendingDirection);

            var query = _userManager.Users.AsQueryable();

            Console.WriteLine($"Initial Total Users: {query.Count()}");

            if (!string.IsNullOrEmpty(searchBy))
            {
                query = searchBy.Contains(":") || searchBy.Contains("+")
                    ? FilterSearchWithParameters(query, searchBy)
                    : DefaultFilterSearch(query, searchBy);
            }

            Console.WriteLine($"After Filtering: {query.Count()} records remain.");

            query = orderAscendingDirection
                ? query.OrderByDynamic(orderCriteria!, DtOrderDir.Asc)
                : query.OrderByDynamic(orderCriteria!, DtOrderDir.Desc);

            var totalResultsCount = await _userManager.Users.CountAsync();
            var filteredResultsCount = await query.CountAsync();

            var paginatedUsers = await query
                .Skip(dtParameters.Start)
                .Take(dtParameters.Length)
                .Where(u => !string.IsNullOrEmpty(u.Id.ToString()))
                .Select(u => new
                {
                    id = u.Id,
                    firstName = u.FirstName ?? "N/A",
                    lastName = u.LastName ?? "N/A",
                    email = u.Email ?? "N/A",
                    emailConfirmed = u.EmailConfirmed,
                })
                .ToListAsync();

            Console.WriteLine($"Pagination: Start={dtParameters.Start}, Length={dtParameters.Length}, Returned={paginatedUsers.Count}");

            var jsonResult = new DtResult<object>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = paginatedUsers
            };

            return Json(jsonResult);
        }



        private IQueryable<ApplicationUser> DefaultFilterSearch(IQueryable<ApplicationUser> result, string searchBy)
        {
            var upperSearch = searchBy.ToUpper();

            result = result.Where(r =>
                (r.FirstName != null && r.FirstName.ToUpper().Contains(upperSearch)) ||
                (r.LastName != null && r.LastName.ToUpper().Contains(upperSearch)) ||
                (r.Email != null && r.Email.ToUpper().Contains(upperSearch)) ||
                (r.EmailConfirmed && "CONFIRMED".Contains(upperSearch)) ||
                (!r.EmailConfirmed && "NOT YET CONFIRMED".Contains(upperSearch))
            );

            return result;
        }

        private IQueryable<ApplicationUser> FilterSearchWithParameters(IQueryable<ApplicationUser> query, string searchBy)
        {
            if (string.IsNullOrWhiteSpace(searchBy))
                return query;

            var filters = searchBy.Split('+', StringSplitOptions.RemoveEmptyEntries);

            foreach (var filter in filters)
            {
                var keyValue = filter.Split(':', 2); // split on the first ':' only
                if (keyValue.Length != 2) continue;

                var key = keyValue[0].Trim().ToLower();
                var value = keyValue[1].Trim().ToUpperInvariant();

                switch (key)
                {
                    case "firstname":
                        query = query.Where(r => r.FirstName != null && r.FirstName.ToUpper().Contains(value));
                        break;

                    case "lastname":
                        query = query.Where(r => r.LastName != null && r.LastName.ToUpper().Contains(value));
                        break;

                    case "email":
                        query = query.Where(r => r.Email != null && r.Email.ToUpper().Contains(value));
                        break;

                    case "emailconfirmed":
                        if (value == "CONFIRMED")
                        {
                            query = query.Where(r => r.EmailConfirmed == true);
                        }
                        else if (value == "NOT YET CONFIRMED" || value == "NOT CONFIRMED" || value == "UNCONFIRMED")
                        {
                            query = query.Where(r => r.EmailConfirmed == false);
                        }
                        break;

                    default:
                        // Optionally handle unknown keys
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
