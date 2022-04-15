using SocialMedia1.Models;
using SocialMedia1.Models.Groups;
using SocialMedia1.Models.Users;

namespace SocialMedia1.Services.Groups
{
    public interface IGroupService : IService
    {
        GroupViewModel[] GetGroups(string userId);

        Task<ICollection<JoinGroupRequestViewModel>> GetJoinGroupRequestsAsync(string groupId);

        Task<GroupMembersViewModel> GetMembersAsync(string groupId);

        Task CreateGroupAsync(string name, string description, bool isPrivate ,string creatorId);

        GroupViewModel GetGroup(string id);
    }
}
