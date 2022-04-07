using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia1.Models;
using SocialMedia1.Services;
using System.Diagnostics;

namespace SocialMedia1.Controllers
{
    public class HomeController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUserProfileService userProfileService;
        private readonly IGroupService groupService;
        private readonly IIndexService indexService;

        public HomeController
            (RoleManager<IdentityRole> roleManager, ILogger<HomeController> logger, UserManager<IdentityUser> userManager, 
            IUserProfileService userProfileService, IGroupService groupService, IIndexService indexService)
        {
            this.roleManager = roleManager;
            _logger = logger;
            this.userManager = userManager;
            this.userProfileService = userProfileService;
            this.groupService = groupService;
            this.indexService = indexService;
        }

        [Authorize]
        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult Landing()
        {
            return View();
        }

        public IActionResult Index()
        {
            var userId = userManager.GetUserId(HttpContext.User);

            if (userId == null)
            {
                return Redirect("Home/Landing");
            }

            //await roleManager.CreateAsync(new IdentityRole
            //{
            //    Name = "Admin"
            //});

            //await userManager.AddToRoleAsync(await userManager.GetUserAsync(this.User), "Admin");

            var model = indexService.GetIndexView(userId);

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> FollowRequests()
        {
            var userId = await userManager.GetUserIdAsync(await userManager.GetUserAsync(User));

            var model = userProfileService.GetAllFollowRequests(userId);

            return View(model);
        }

        [Authorize]
        public IActionResult Groups()
        {
            var userId = userManager.GetUserId(HttpContext.User);

            var model = groupService.GetGroups(userId);

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}