using SocialMedia1.Data;
using SocialMedia1.Data.Models;
using SocialMedia1.Models;
using SocialMedia1.Models.Posts;

namespace SocialMedia1.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext context;

        public PostService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CreatePostAsync(string userId, string content)
        {
            Post post = new Post
            {
                Content = content,
                UserProfileId = userId,
                CreatedOn = DateTime.UtcNow,
            };

            await context.Posts.AddAsync(post);

            await context.SaveChangesAsync();
        }

        public async Task CreateGroupPostAsync(string groupId, string userId, string content)
        {
            Group group = await context.Groups.FindAsync(groupId);

            if (group is null)
            {
                return;
            }

            Post post = new Post
            {
                UserProfileId = userId,
                Content = content,
                CreatedOn = DateTime.UtcNow,
                GroupId = groupId
            };

            group.Posts.Add(post);

            await context.SaveChangesAsync();
        }

        public ICollection<PostViewModel> GetAllPosts(string userId)
        {
            var posts = context.Posts.Where(x => x.UserProfileId == userId && !x.IsDeleted).Select(x => new PostViewModel
            {
                Author = x.UserProfile.Nickname,
                AuthorId = x.UserProfile.Id,
                ImageUrl = x.UserProfile.ImageUrl,
                Id = x.Id,
                Content = x.Content,
                CreatedOn = x.CreatedOn,
                GroupId = x.GroupId,
                GroupName = context.Groups.FirstOrDefault(g => g.Id == x.GroupId).Name,
            }).ToList();

            return posts;
        }

        public ICollection<PostViewModel> GetAllPostsByFollowedUsers(string userId)
        {
            if (userId is null)
            {
                return new List<PostViewModel>();
            }

            var currentUser = context.UserProfiles.First(x => x.Id == userId);

            var posts = context.UserProfiles.Where(x => x.FollowedBy.Contains(currentUser))
            .SelectMany(x => x.Posts)
            .ToList()
            .Where(x => x.GroupId is null && !x.IsDeleted)
            .Select(x => new PostViewModel
            {
                Author = context.UserProfiles.Find(x.UserProfileId).Nickname,
                AuthorId = x.UserProfile.Id,
                ImageUrl = x.UserProfile.ImageUrl,
                Content = x.Content,
                Id = x.Id,
                CreatedOn = x.CreatedOn,
            })
            .ToList();

            return posts;
        }

        public ICollection<PostViewModel> GetAllPostsInUsersGroups(string userId)
        {
            if (userId is null)
            {
                return new List<PostViewModel>();
            }

            var posts = context.Groups.Where(x => x.Users.Any(x => x.UserProfileId == userId))
                .SelectMany(x => x.Posts)
                .ToList()
                .Where(x => !x.IsDeleted)
                .Select(x => new PostViewModel
                {
                    Author = context.UserProfiles.Find(x.UserProfileId).Nickname,
                    AuthorId = x.UserProfile.Id,
                    ImageUrl = x.UserProfile.ImageUrl,
                    Content = x.Content,
                    Id = x.Id,
                    CreatedOn = x.CreatedOn,
                    GroupId = x.GroupId,
                    GroupName = context.Groups.Find(x.GroupId).Name,
                })
                .ToList();

            return posts;
        }

        public async Task DeletePostAsync(string postId, string userId)
        {
            var post = await context.Posts.FindAsync(postId);

            if (post.UserProfileId != userId)
            {
                return;
            }

            post.IsDeleted = true;

            await context.SaveChangesAsync();
        }

        public async Task ReportPostAsync(string postId, string userId)
        {
            if (context.PostCommunityReports.Any(x => x.PostId == postId && x.ReporterId == userId))
            {
                return;
            }

            PostCommunityReport report = new PostCommunityReport
            {
                PostId = postId,
                ReporterId = userId,
            };

            await context.PostCommunityReports.AddAsync(report);

            await context.SaveChangesAsync();
        }
    }
}
