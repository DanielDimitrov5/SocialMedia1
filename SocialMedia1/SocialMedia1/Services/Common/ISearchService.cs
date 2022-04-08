using SocialMedia1.Models;
using SocialMedia1.Models.Groups;
using SocialMedia1.Models.Users;

namespace SocialMedia1.Services.Common
{
    public interface ISearchService
    {
        ICollection<ProfileViewModel> GetProfilesBySearchTerm(string searchTerm);

        ICollection<GroupViewModel> GetGroupsBySearchTerm(string searchTerm);

    }
}
