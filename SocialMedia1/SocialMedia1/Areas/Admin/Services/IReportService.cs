using SocialMedia1.Areas.Admin.Models;

namespace SocialMedia1.Areas.Admin.Services
{
    public interface IReportService
    {
        Task<ICollection<ReportedPostViewModel>> GetAllReportedPostsAsync();

        Task DeleteReportedPostAsync(string postId);

        Task ApproveReportedPostAsync(string postId);
    }
}
