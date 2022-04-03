using SocialMedia1.Models;

namespace SocialMedia1.Services
{
    public interface IPostService
    {
        void CreatePost(string userId, string content);

        void CreateGroupPost(string groupId, string userId, string content);

        Task DeletePostAsync(string postId, string userId);

        Task ReportPostAsync(string postId, string userId);

        ICollection<PostViewModel> GetAllPosts(string userId);

        ICollection<PostViewModel> GetAllPostsByFollowedUsers(string userId);

        ICollection<PostViewModel> GetAllPostsInUsersGroups(string userId);
    }
}
