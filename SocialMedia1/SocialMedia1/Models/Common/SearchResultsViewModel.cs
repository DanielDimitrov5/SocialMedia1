using SocialMedia1.Models.Groups;
using SocialMedia1.Models.Users;

namespace SocialMedia1.Models.Common
{
    public class SearchResultsViewModel
    {
        public ICollection<ProfileViewModel> Profiles { get; set; }

        public ICollection<GroupViewModel> Groups { get; set; }
    }
}
