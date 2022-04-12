using SocialMedia1.Models;
using SocialMedia1.Models.Groups;
using SocialMedia1.Models.Users;

namespace SocialMedia1.Services.Common
{
    public interface ISearchService
    {
        Task<ICollection<ProfileViewModel>> GetProfilesBySearchTerm(string searchTerm);

        Task<ICollection<GroupViewModel>> GetGroupsBySearchTerm(string searchTerm);
    }
}
