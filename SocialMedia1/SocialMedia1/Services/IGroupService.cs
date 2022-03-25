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

        void JoinGroup(string groupId, string userId);

        void SendJoinRequest(string groupId, string userId);

        void ApproveJoinRequest(string requesterId, string groupId);

        void DeleteJoinRequest(string requesterId, string groupId);

        void LeaveGroup(string groupId, string userId);

        //void CreateGroupPost(string groupId, string userId, string content);

        bool IsUserGroupMember(string userId, string groupId);

        bool IsGroupPrivate(string groupId);

        bool IsJoinRequstSent(string groupId, string userId);

        bool IsUserGroupCreator(string userId, string groupId);

        ICollection<GroupViewModel> GetGroupsBySearchTerm(string searchTerm);
    }
}
