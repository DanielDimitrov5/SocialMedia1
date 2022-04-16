using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia1.Areas.Admin.Services;

namespace SocialMedia1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = GlobalConstants.AdminRole)]
    public class ReportsController : Controller
    {
        private readonly IReportService reportService;

        public ReportsController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await reportService.GetAllReportedPostsAsync();

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await reportService.DeleteReportedPostAsync(id);

            return Redirect("/Admin/Reports");
        }

        public async Task<IActionResult> Approve(string id)
        {
            await reportService.ApproveReportedPostAsync(id);

            return Redirect("/Admin/Reports");
        }
    }
}
