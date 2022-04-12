using Microsoft.EntityFrameworkCore;
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
            var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == postId);

            if (post == null)
            {
                return;
            }

            List<PostCommunityReport> report = await context.PostCommunityReports.Where(x => x.PostId == postId).ToListAsync();

            context.PostCommunityReports.RemoveRange(report);

            await context.SaveChangesAsync();
        }

        public async Task DeleteReportedPostAsync(string postId)
        {
            var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == postId);

            if (post == null)
            {
                return;
            }

            post.IsDeleted = true;

            List<PostCommunityReport> report = await context.PostCommunityReports.Where(x => x.PostId == postId).ToListAsync();

            context.PostCommunityReports.RemoveRange(report);

            await context.SaveChangesAsync();
        }

        public async Task<ICollection<ReportedPostViewModel>> GetAllReportedPostsAsync()
        {
            var reportedPosts = await context.PostCommunityReports.Select(x => new ReportedPostViewModel
            {
                PostId = x.PostId,
                Content = x.Post.Content,
                AuthorId = x.Post.UserProfileId,
                GroupId = x.Post.GroupId,
                CreatedOn = x.Post.CreatedOn,
            }).ToListAsync();

            List<ReportedPostViewModel> uniquePosts = new();

            foreach (var post in reportedPosts)
            {
                if (!uniquePosts.Any(x => x.PostId == post.PostId))
                {
                    var author = await context.UserProfiles.FindAsync(post.AuthorId);

                    post.Author = author.Nickname;

                    var group = await context.Groups.FirstOrDefaultAsync(x => x.Id == post.GroupId);

                    post.GroupName = group?.Name;

                    uniquePosts.Add(post);
                }

                uniquePosts.First(x => x.PostId == post.PostId).ReportsCount++;
            }

            return uniquePosts;
        }
    }
}
