using SocialMedia1.Data;
using SocialMedia1.Data.Models;
using SocialMedia1.Services.Posts;
using SocialMedia1.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace SocialMedia1.Services.Users
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ApplicationDbContext context;
        private readonly IPostService postService;

        public UserProfileService(ApplicationDbContext context, IPostService postService)
        {
            this.context = context;
            this.postService = postService;
        }

        public async Task AddUserProfileAsync(string Id)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            string email = user.Email;

            string nickname = email.Split('@')[0];

            await context.UserProfiles.AddAsync(new UserProfile { Id = Id, Nickname = nickname, EmailAddress = email });

            await context.SaveChangesAsync();
        }

        public async Task<bool> EditUserProfileAsync(string id, string nickname, string name, string surename, bool IsPrivate, string bio, string image)
        {
            UserProfile userProfile = await context.UserProfiles.FirstAsync(x => x.Id == id);

            bool isChanged =
                userProfile.Nickname != nickname || userProfile.Name != name || userProfile.Surname != surename ||
                userProfile.IsPrivate != IsPrivate || userProfile.Bio != bio || image != null;

            if (!isChanged)
            {
                return false;
            }

            userProfile.Nickname = nickname;
            userProfile.Name = name;
            userProfile.Surname = surename;
            userProfile.IsPrivate = IsPrivate;
            userProfile.Bio = bio;

            if (image != null)
            {
                userProfile.ImageUrl = image;
            }

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<ProfileViewModel> GetUserProfileDataAsync(string id)
        {
            UserProfile user = await context.UserProfiles.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return null;
            }

            await context.Entry(user).Collection(x => x.FollowedBy).LoadAsync();
            await context.Entry(user).Collection(x => x.Follows).LoadAsync();

            var posts = postService.GetAllPosts(id).Where(x => x.GroupId == null).ToList();
            var groupPosts = postService.GetAllPosts(id).Where(x => x.GroupId != null).ToList();

            ProfileViewModel model = new ProfileViewModel
            {
                Id = user.Id,
                Nickname = user.Nickname,
                Name = user.Name,
                Surname = user.Surname,
                Bio = user.Bio,
                Posts = posts,
                FollowersCount = user.FollowedBy.Count,
                FollowingCount = user.Follows.Count,
                GroupPosts = groupPosts,
                IsPrivate = user.IsPrivate,
                ImageUrl = user.ImageUrl,
            };

            return model;
        }

        public async Task<ICollection<FollowRequestViewModel>> GetAllFollowRequests(string currentUserId)
        {
            return await context.FollowRequests.Where(x => x.UserId == currentUserId).Select(x => new FollowRequestViewModel
            {
                RequesterId = x.UserRequester.Id,
                Nickname = x.UserRequester.Nickname,
                Name = x.User.Name + " " + x.User.Surname,
                ImageUrl = x.UserRequester.ImageUrl,
                Bio = x.UserRequester.Bio,
                CurrentUserId = currentUserId,
            }).ToListAsync();
        }

        public async Task<UsersProfilesViewModel> GetAllFollowersAsync(string currentUserId)
        {
            var followers = await context.UserProfiles.Where(x => x.Follows.Any(x => x.Id == currentUserId))
                .Select(x => new ProfileViewModel
                {
                    Id = x.Id,
                    Nickname = x.Nickname,
                    Name = x.Name + " " + x.Surname,
                    ImageUrl = x.ImageUrl,
                    Bio = x.Bio,
                }).ToListAsync();

            UsersProfilesViewModel model = new UsersProfilesViewModel
            {
                Id = currentUserId,
                Profiles = followers,
            };

            return model;
        }

        public async Task<UsersProfilesViewModel> GetAllFollowingAsync(string currentUserId)
        {
            var user = await context.UserProfiles.FindAsync(currentUserId);

            await context.Entry(user).Collection(x => x.Follows).LoadAsync();

            var following = user.Follows
                 .Select(x => new ProfileViewModel
                 {
                     Id = x.Id,
                     Nickname = x.Nickname,
                     Name = x.Name + " " + x.Surname,
                     ImageUrl = x.ImageUrl,
                     Bio = x.Bio,
                 }).ToList();

            UsersProfilesViewModel model = new UsersProfilesViewModel
            {
                Id = currentUserId,
                Profiles = following,
            };

            return model;
        }

        public async Task<string> GetUsernameAsync(string id)
        {
            var user = await context.UserProfiles.FindAsync(id);

            return user.Nickname;
        }
    }
}
