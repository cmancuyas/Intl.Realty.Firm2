using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.UserVM;
using Intl.Realty.Firm.Utility.MenuData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Intl.Realty.Firm.Controllers
{
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(RoleManager<IdentityRole<Guid>> roleManager,
                              UserManager<ApplicationUser> userManager,
                              ILogger<UserController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email!,
                UserName = user.UserName!,
                FirstName = user.FirstName,
                MiddleName = user.LastName,
                LastName = user.LastName,
                Suffix = user.Suffix,
                DepartmentId = user.DepartmentId,
                EmploymentStatusId = user.EmploymentStatusId,
                ProfilePictureId = user.ProfilePictureId,
                Claims = userClaims.Select(x => x.Type + " : " + x.Value).ToList(),
                Roles = userRoles.ToList()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel viewModel)
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
                user.MiddleName = viewModel.LastName;
                user.LastName = viewModel.LastName;
                user.Suffix = viewModel.Suffix;
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
        public IActionResult ListPartialView(UserListPartialViewModel listViewModel)
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

            return PartialView("~/Views/Role/Partial/ListPartial.cshtml", listViewModel);
        }
    }
}
