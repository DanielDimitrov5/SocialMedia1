using SocialMedia1.Models;

namespace SocialMedia1.Services
{
    public interface ISearchService
    {
        ICollection<ProfileViewModel> GetProfilesBySearchTerm(string searchTerm);

        ICollection<GroupViewModel> GetGroupsBySearchTerm(string searchTerm);

    }
}
