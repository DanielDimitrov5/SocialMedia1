namespace SocialMedia1.Models
{
    public class GroupMembersViewModel
    {
        public string GroupCreatorId { get; set; }

        public string GroupId { get; set; }

        public ICollection<ProfileViewModel> Members { get; set; }
    }
}
