namespace SocialMedia1.Services
{
    public interface IGroupMemberActionsService
    {
        void JoinGroup(string groupId, string userId);

        void SendJoinRequest(string groupId, string userId);

        void ApproveJoinRequest(string requesterId, string groupId);

        void DeleteJoinRequest(string requesterId, string groupId);

        void LeaveGroup(string groupId, string userId);

        bool IsUserGroupMember(string userId, string groupId);

        bool IsGroupPrivate(string groupId);

        bool IsJoinRequstSent(string groupId, string userId);

        bool IsUserGroupCreator(string userId, string groupId);
    }
}
