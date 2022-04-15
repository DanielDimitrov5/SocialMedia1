using SocialMedia1.Areas.Admin.Models;
using SocialMedia1.Services;

namespace SocialMedia1.Areas.Admin.Services
{
    public interface IReportService : IService
    {
        Task<ICollection<ReportedPostViewModel>> GetAllReportedPostsAsync();

        Task DeleteReportedPostAsync(string postId);

        Task ApproveReportedPostAsync(string postId);
    }
}
