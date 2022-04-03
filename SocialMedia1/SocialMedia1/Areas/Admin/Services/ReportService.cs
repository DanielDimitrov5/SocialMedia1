using SocialMedia1.Areas.Admin.Models;
using SocialMedia1.Data;
using SocialMedia1.Data.Models;

namespace SocialMedia1.Areas.Admin.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext context;

        public ReportService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task ApproveReportedPostAsync(string postId)
        {
            var post = context.Posts.FirstOrDefault(x => x.Id == postId);

            List<PostCommunityReport> report = context.PostCommunityReports.Where(x => x.PostId == postId).ToList();

            context.PostCommunityReports.RemoveRange(report);

            await context.SaveChangesAsync();
        }

        public async Task DeleteReportedPostAsync(string postId)
        {
            var post = context.Posts.FirstOrDefault(x => x.Id == postId);

            post.IsDeleted = true;

            List<PostCommunityReport> report = context.PostCommunityReports.Where(x => x.PostId == postId).ToList();

            context.PostCommunityReports.RemoveRange(report);

            await context.SaveChangesAsync();
        }

        public async Task<ICollection<ReportedPostViewModel>> GetAllReportedPostsAsync()
        {
            var reportedPosts = context.PostCommunityReports.Select(x => new ReportedPostViewModel
            {
                PostId = x.PostId,
                Content = x.Post.Content,
                AuthorId = x.Post.UserProfileId,
                GroupId = x.Post.GroupId,
                CreatedOn = x.Post.CreatedOn,
            }).ToList();

            List<ReportedPostViewModel> uniquePosts = new();

            foreach (var post in reportedPosts)
            {
                if (!uniquePosts.Any(x => x.PostId == post.PostId))
                {
                    var author = await context.UserProfiles.FindAsync(post.AuthorId);

                    post.Author = author.Nickname;
                    post.GroupName = context.Groups.FirstOrDefault(x => x.Id == post.GroupId)?.Name;

                    uniquePosts.Add(post);
                }

                uniquePosts.First(x => x.PostId == post.PostId).ReportsCount++;
            }

            return uniquePosts;
        }
    }
}
