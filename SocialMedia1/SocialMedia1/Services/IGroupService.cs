using SocialMedia1.Models;

namespace SocialMedia1.Services
{
    public interface IGroupService
    {
        ICollection<GroupViewModel> GetGroups(string userId);

        ICollection<JoinGroupRequestViewModel> GetJoinGroupRequests(string groupId);

        void CreateGroup(string name, string description, bool isPrivate ,string creatorId);

        GroupViewModel GetGroup(string id);

        void JoinGroup(string groupId, string userId);

        void SendJoinRequest(string groupId, string userId);

        void LeaveGroup(string groupId, string userId);

        void CreatePost(string groupId, string userId, string content);

        bool IsUserGroupMember(string userId, string groupId);

        bool IsGroupPrivate(string groupId);

        bool IsJoinRequstSent(string groupId, string userId);

        ICollection<GroupViewModel> GetGroupsBySearchTerm(string searchTerm);
    }
}
