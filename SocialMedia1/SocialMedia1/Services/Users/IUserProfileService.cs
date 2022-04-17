using SocialMedia1.Models.Users;

namespace SocialMedia1.Services.Users
{
    public interface IUserProfileService : IService
    {
        Task AddUserProfileAsync(string Id);

        Task<bool> EditUserProfileAsync(string id, string nickname, string name, string surename, bool IsPrivate, string bio, string image);

        Task<string> GetUsernameAsync(string id);

        Task<ProfileViewModel> GetUserProfileDataAsync(string id);

        Task<ICollection<FollowRequestViewModel>> GetAllFollowRequests(string currentUserId);

        Task<UsersProfilesViewModel> GetAllFollowersAsync(string userId);

        Task<UsersProfilesViewModel> GetAllFollowingAsync(string userId);
    }
}
