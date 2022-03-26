using SocialMedia1.Models;

namespace SocialMedia1.Services
{
    public interface IGroupService
    {
        GroupViewModel[] GetGroups(string userId);

        ICollection<JoinGroupRequestViewModel> GetJoinGroupRequests(string groupId);

        GroupMembersViewModel GetMembers(string groupId);

        void CreateGroup(string name, string description, bool isPrivate ,string creatorId);

        GroupViewModel GetGroup(string id);
    }
}
