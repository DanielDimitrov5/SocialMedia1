namespace SocialMedia1.Models.Users
{
    public class UsersProfilesViewModel
    {
        public string Id { get; set; }

        public ICollection<ProfileViewModel> Profiles { get; set; }
    }
}
