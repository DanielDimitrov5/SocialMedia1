using SocialMedia1.Models.Users;

namespace SocialMedia1.Models.Groups
{
    public class GroupMembersViewModel
    {
        public string? GroupCreatorId { get; set; }

        public string? GroupId { get; set; }

        public ICollection<ProfileViewModel>? Members { get; set; }
    }
}
