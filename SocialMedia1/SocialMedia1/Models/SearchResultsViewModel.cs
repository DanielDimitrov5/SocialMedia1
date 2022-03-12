namespace SocialMedia1.Models
{
    public class SearchResultsViewModel
    {
        public ICollection<ProfileViewModel> Profiles { get; set; }

        public ICollection<GroupViewModel> Groups { get; set; }
    }
}
