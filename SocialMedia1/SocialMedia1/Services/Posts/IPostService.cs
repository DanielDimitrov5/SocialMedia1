using SocialMedia1.Models;
using SocialMedia1.Models.Posts;

namespace SocialMedia1.Services.Posts
{
    public interface IPostService
    {
        Task CreatePostAsync(string userId, string content);

        Task CreateGroupPostAsync(string groupId, string userId, string content);

        Task DeletePostAsync(string postId, string userId);

        Task ReportPostAsync(string postId, string userId);

        ICollection<PostViewModel> GetAllPosts(string userId);

        ICollection<PostViewModel> GetAllPostsByFollowedUsers(string userId);

        ICollection<PostViewModel> GetAllPostsInUsersGroups(string userId);
    }
}
