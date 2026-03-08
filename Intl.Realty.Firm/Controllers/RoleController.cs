using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel;
using Intl.Realty.Firm.Model.ViewModel.AccountVM;
using Intl.Realty.Firm.Model.ViewModel.RoleVM;
using Intl.Realty.Firm.Models.DataTable;
using Intl.Realty.Firm.Models;
using Intl.Realty.Firm.Utility.MenuData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using Intl.Realty.Firm.Utility.Utilities;
using Microsoft.EntityFrameworkCore;
using Intl.Realty.Firm.Service.IService;
using Intl.Realty.Firm.Utility.Mapper;

namespace Intl.Realty.Firm.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IExportService<RoleExport> _exportService;
        private readonly ILogger<UserController> _logger;

        public RoleController(RoleManager<ApplicationRole> roleManager,
                              UserManager<ApplicationUser> userManager,
                              IExportService<RoleExport> exportService,
                              ILogger<UserController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _exportService = exportService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole();
                role.Name = viewModel.RoleName;

                var roleExist = await _roleManager.RoleExistsAsync(viewModel.RoleName);
                if (!roleExist)
                {
                    var result = await _roleManager.CreateAsync(new ApplicationRole(viewModel.RoleName));

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Role");
                    }
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            return View(viewModel);
        }
        [HttpGet]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var roleModel = await _roleManager.FindByIdAsync(id.ToString());

            if (roleModel == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleModel!.Id} cannot be found";
                return View("NotFound");
            }

            var editRoleViewModel = new EditRoleViewModel
            {
                Id = roleModel.Id,
                RoleName = roleModel.Name!
            };

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, roleModel.Name!))
                {
                    //user.UserName = role.Name;
                    editRoleViewModel.Users.Add(user.UserName!);
                }
            }

            return View(editRoleViewModel);
        }
        [HttpPost]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> Edit(EditRoleViewModel viewModel)
        {
            var role = await _roleManager.FindByIdAsync(viewModel.Id.ToString());

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {viewModel.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = viewModel.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if ((result.Succeeded))
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                //viewModel.Users = await _userManager.Users.ToList();
                return View(viewModel);
            }
        }
        public async Task<IActionResult> Preview(Guid id)
        {
            var roleModel = await _roleManager.FindByIdAsync(id.ToString());

            if (roleModel == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleModel!.Id} cannot be found";
                return View("NotFound");
            }

            var previewRoleViewModel = new PreviewRoleViewModel
            {
                Id = roleModel.Id,
                RoleName = roleModel.Name!,
               NormalizedName = roleModel.NormalizedName!
            };

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, roleModel.Name!))
                {
                    //user.UserName = role.Name;
                    previewRoleViewModel.Users.Add(user);
                }
            }

            return View(previewRoleViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleid = roleId;
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id ={roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id.ToString(),
                    UserName = user.UserName!
                };
                if (await _userManager.IsInRoleAsync(user, role.Name!))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null!;
                var isInRole = await _userManager.IsInRoleAsync(user!, role.Name!);

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user!, role.Name!)))
                {
                    result = await _userManager.AddToRoleAsync(user!, role.Name!);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user!, role.Name!))
                {
                    result = await _userManager.RemoveFromRoleAsync(user!, role.Name!);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { Id = roleId });
                    }
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }
        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                try
                {
                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("ListRoles");
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
                {
                    _logger.LogError($"Error deleting role");

                    ErrorViewModel viewModel = new ErrorViewModel();
                    viewModel.ErrorTitle = $"{role.Name} role is in use";
                    viewModel.ErrorMessage = $"{role.Name} role cannot be deleted as there are users " +
                                             $"in this role. If you want to delete this role, please remove users from " +
                                             $"the role and try to delete";
                    return View("Error", viewModel);
                }
            }
        }
        [HttpGet]
        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.UserId = userId;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"(User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name!
                };

                if (await _userManager.IsInRoleAsync(user, role.Name!))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }
            return View(model);

        }
        [HttpGet]
        public IActionResult ListPartialView(RoleListPartialViewModel listViewModel)
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

            return PartialView("~/Views/Role/Partial/ListPartial.cshtml", listViewModel);
        }


        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            ViewBag.UserId = userId;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"(User with Id = {userId} cannot be found";
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
                        model.Where(x => x.IsSelected)
                            .Select(y => y.RoleName)
                        );
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("EditUser", new { Id = userId });

        }
        public async Task<IActionResult> GetRoleList([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            // Default order criteria
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            // Initialize order criteria
            (orderCriteria, orderAscendingDirection) = InitializeOrderCriteria(dtParameters, orderCriteria, orderAscendingDirection);

            // Query initialization
            var result = _roleManager.Roles;

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
            var totalResultsCount = _roleManager.Roles.ToList().Count();

            // Prepare the result for DataTables
            var jsonResult = new DtResult<ApplicationRole>
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

        private IQueryable<ApplicationRole> DefaultFilterSearch(IQueryable<ApplicationRole> result, string searchBy)
        {
            result = result.Where(r => r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper()) ||
                   r.Name! != null && r.Name.ToUpper().Contains(searchBy.ToUpper())
                   );
            return result;
        }
        private IQueryable<ApplicationRole> FilterSearchWithParameters(IQueryable<ApplicationRole> query, string searchBy)
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
                    case "name":
                        query = query.Where(r => r.Name != null && r.Name.ToUpper().Contains(value));
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

                var result = _roleManager.Roles;

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

                var resultListForExport = resultList.ToRoleListExport();

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
