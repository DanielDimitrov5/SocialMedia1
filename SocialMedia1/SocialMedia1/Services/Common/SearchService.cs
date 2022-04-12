using Microsoft.EntityFrameworkCore;
using SocialMedia1.Data;
using SocialMedia1.Models;
using SocialMedia1.Models.Groups;
using SocialMedia1.Models.Users;

namespace SocialMedia1.Services.Common
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext context;

        public SearchService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<GroupViewModel>> GetGroupsBySearchTerm(string searchTerm)
        {
            var  groups = await context.Groups
               .Where(x => x.Name.Contains(searchTerm)
                        || x.Description.Contains(searchTerm)
                        || (x.Name + x.Description).Contains(searchTerm))
               .Select(x => new GroupViewModel
               {
                   Id = x.Id,
                   Name = x.Name,
                   Description = x.Description,
                   Members = x.MembersCount,
                   Status = x.IsPrivate ? "Private" : "Public",
               })
               .ToListAsync();

            return groups;
        }

        public async Task<ICollection<ProfileViewModel>> GetProfilesBySearchTerm(string searchTerm)
        {
            var profiles = await context.UserProfiles
                .Where(x => x.Nickname.Contains(searchTerm)
                         || x.Name.Contains(searchTerm)
                         || x.Surname.Contains(searchTerm)
                         || (x.Name + x.Surname).Contains(searchTerm))
                .Select(x => new ProfileViewModel
                {
                    Id = x.Id,
                    Nickname = x.Nickname,
                    Name = x.Name,
                    Surname = x.Surname,
                    Bio = x.Bio,
                    ImageUrl = x.ImageUrl,
                })
                .ToListAsync();

            return profiles;
        }
    }
}
