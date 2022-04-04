using SocialMedia1.Models;

namespace SocialMedia1.Services
{
    public interface IGroupService
    {
        GroupViewModel[] GetGroups(string userId);

        Task<ICollection<JoinGroupRequestViewModel>> GetJoinGroupRequestsAsync(string groupId);

        Task<GroupMembersViewModel> GetMembersAsync(string groupId);

        Task CreateGroupAsync(string name, string description, bool isPrivate ,string creatorId);

        GroupViewModel GetGroup(string id);
    }
}
