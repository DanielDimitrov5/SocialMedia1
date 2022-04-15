using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia1.Models;
using SocialMedia1.Services.Groups;
using System.Diagnostics;
using SocialMedia1.Services.Users;
using SocialMedia1.Services.Common;
using SocialMedia1.Models.Common;
using SocialMedia1.Services.Common.MailSender;

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
        private readonly IEmailSender sender;

        public HomeController
            (RoleManager<IdentityRole> roleManager, ILogger<HomeController> logger, UserManager<IdentityUser> userManager,
            IUserProfileService userProfileService, IGroupService groupService, IIndexService indexService, IEmailSender sender)
        {
            this.roleManager = roleManager;
            _logger = logger;
            this.userManager = userManager;
            this.userProfileService = userProfileService;
            this.groupService = groupService;
            this.indexService = indexService;
            this.sender = sender;
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

            //string content = System.IO.File.ReadAllText(@"C:\Users\USER\Desktop\emailSender.html");

            //await sender.SendEmailAsync(GlobalConstants.email, "Dani", "dani_dimitrov2003@abv.bg", "Здр", content);

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

            var model = await userProfileService.GetAllFollowRequests(userId);

            return View(model);
        }

        [Authorize]
        public IActionResult Groups()
        {
            var userId = userManager.GetUserId(HttpContext.User);

            var model = groupService.GetGroups(userId);

            return View(model);
        }

        [Authorize]
        [HttpGet("/Feedback")]
        public IActionResult Feedback()
        {
            return View();
        }

        [Authorize]
        [HttpPost("/Feedback")]
        public async Task<IActionResult> Feedback(FeedbackInputModel model)
        {
            var userId = await userManager.GetUserIdAsync(await userManager.GetUserAsync(User));

            var name = await userProfileService.GetUsernameAsync(userId);

            bool isSuccessful = await sender.SendEmailAsync(GlobalConstants.SecondEmail, model.Email, GlobalConstants.MainEmail, "feedback", model.Message);

            if (isSuccessful)
            {
                TempData["isSuccessfully"] = "1";

                await sender.AutoSendEmail(model.Email, name);
            }

            return Redirect("/");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Chat()
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