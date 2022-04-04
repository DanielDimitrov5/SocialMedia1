using SocialMedia1.Data.Models;
using SocialMedia1.Models;

namespace SocialMedia1.Services
{
    public interface IUserProfileService
    {
        Task AddUserProfileAsync(string Id);

        Task EditUserProfileAsync(string id, string nickname, string name, string surename, bool IsPrivate, string city, DateTime birthday, string emailaddress, string bio);

        Task<ProfileViewModel> GetUserProfileDataAsync(string id);

        ICollection<FollowRequestViewModel> GetAllFollowRequests(string currentUserId);

        UsersProfilesViewModel GetAllFollowers(string userId);

        Task<UsersProfilesViewModel> GetAllFollowingAsync(string userId);
    }
}
